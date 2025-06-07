using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Application.DTOs.Post;
using Application.IServices.UseCases;
using Application.Utilities;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Services.UseCases;

/// <summary>
/// Provides business logic for managing Posts.
/// </summary>
public class PostService : IPostService
{
    private readonly IRepository<Post, int> _postRepository;
    private readonly IRepository<Employee, string> _employeeRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILogger<PostService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostService"/> class.
    /// </summary>
    /// <param name="postRepository">The repository for Post entities.</param>
    /// <param name="employeeRepository">The repository for Employee entities.</param>
    /// <param name="httpContextAccessor">The accessor for the current HTTP context.</param>
    /// <param name="mapper">The AutoMapper instance for DTO-entity mapping.</param>
    /// <param name="logger">The logger for this service.</param>
    public PostService(
        IRepository<Post, int> postRepository,
        IRepository<Employee, string> employeeRepository,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ILogger<PostService> logger)
    {
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<GetPostDTO> CreatePostAsync(CreatePostDTO createPostDto)
    {
        if (createPostDto is null)
        {
            _logger.LogError("CreatePostAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(createPostDto), "Post creation DTO cannot be null.");
        }

        _logger.LogInformation("Attempting to create post with title: {Title}", createPostDto.Title);

        try
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is unavailable.");

            var userIdClaim = httpContext.User.Claims
                .FirstOrDefault(c => c.Type == "UserId" || c.Type == "sub" || c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                _logger.LogError("Failed to retrieve User ID from HttpContext.");
                throw new UnauthorizedAccessException("User ID not found in the request.");
            }

            _logger.LogInformation("User ID retrieved successfully: {EmployeeId}", userIdClaim);

            var postEntity = _mapper.Map<Post>(createPostDto);
            postEntity.EmployeeId = userIdClaim;
            postEntity.Slug = SlugHelper.GenerateSlug(createPostDto.Title);
            postEntity.PublishDate = DateTime.UtcNow;
            postEntity.Status = PostStatus.Draft;

            await _postRepository.AddAsync(postEntity).ConfigureAwait(false);
            await _postRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Post '{Title}' created successfully with ID {Id} and status '{Status}'.", createPostDto.Title, postEntity.Id, postEntity.Status);

            return _mapper.Map<GetPostDTO>(postEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating post.");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<GetPostDTO> GetPostByIdAsync(int id)
    {
        _logger.LogInformation("Attempting to retrieve post with ID: {Id}", id);

        try
        {
            var post = await _postRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (post is null)
            {
                _logger.LogWarning("Post with ID {Id} was not found.", id);
                throw new KeyNotFoundException($"Post with ID {id} was not found.");
            }

            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is unavailable.");
            var userIdClaim = httpContext.User.Claims
                .FirstOrDefault(c => c.Type == "UserId" || c.Type == "sub" || c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (role != "Admin" && post.EmployeeId != userIdClaim)
            {
                _logger.LogWarning("Unauthorized access attempt to post ID {Id} by user {UserId}.", id, userIdClaim);
                throw new UnauthorizedAccessException($"User {userIdClaim} does not have permission to view this post.");
            }

            _logger.LogInformation("Post '{Id}' retrieved successfully.", id);
            return _mapper.Map<GetPostDTO>(post);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving post with ID {Id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GetPostDTO>> GetAllPostsAsync()
    {
        _logger.LogInformation("Attempting to retrieve all posts.");
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                _logger.LogError("HTTP context is unavailable during GetAllPostsAsync.");
                throw new InvalidOperationException("HTTP context is unavailable.");
            }

            var userIdClaim = httpContext.User.Claims
                .FirstOrDefault(c => c.Type == "UserId" || c.Type == "sub" || c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            IEnumerable<Post> posts;

            if (role == "Employee")
            {
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    _logger.LogWarning("Employee role detected but UserId claim is missing.");
                    throw new UnauthorizedAccessException("Employee ID not found for retrieving posts.");
                }

                _logger.LogDebug("Retrieving posts for Employee ID: {EmployeeId}", userIdClaim);
                posts = await _postRepository.GetAllByPredicateAsync(p => p.EmployeeId == userIdClaim).ConfigureAwait(false);
            }
            else if (role == "Admin")
            {
                _logger.LogDebug("Retrieving all posts.");
                posts = await _postRepository.GetAllAsync().ConfigureAwait(false);
            }
            else
            {
                _logger.LogWarning("Unauthorized attempt to retrieve all posts.");
                throw new UnauthorizedAccessException("You do not have permission to view all posts.");
            }

            _logger.LogDebug("{Count} posts retrieved.", posts?.Count() ?? 0);

            return _mapper.Map<IEnumerable<GetPostDTO>>(posts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all posts.");
            throw;
        }
    }


    /// <inheritdoc />
    public async Task SubmitPostAsync(int id)
    {
        _logger.LogInformation("Attempting to submit post with ID: {Id}.", id);
        try
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is unavailable.");

            var userIdClaim = httpContext.User.Claims
                .FirstOrDefault(c => c.Type == "UserId" || c.Type == "sub" || c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                _logger.LogWarning("User ID retrieval failed. Submission attempt rejected.");
                throw new UnauthorizedAccessException("User ID not found in the request.");
            }

            if (role != "Employee")
            {
                _logger.LogWarning("Unauthorized submission attempt for post ID {Id} by a non-employee user.", id);
                throw new UnauthorizedAccessException("Only employees can submit posts.");
            }

            var post = await _postRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (post is null)
            {
                _logger.LogWarning("Post with ID {Id} was not found for submission.", id);
                throw new KeyNotFoundException($"Post with ID {id} was not found.");
            }

            if (post.EmployeeId != userIdClaim)
            {
                _logger.LogWarning("User ID {UserId} attempted to submit post {Id}, but they are not the owner.", userIdClaim, id);
                throw new UnauthorizedAccessException("Only the original creator can submit this post.");
            }

            if (post.Status != PostStatus.Draft)
            {
                _logger.LogWarning("Post with ID {Id} is in '{CurrentStatus}' state and cannot be submitted.", id, post.Status);
                throw new InvalidOperationException($"Post with ID {id} is in '{post.Status}' state and cannot be submitted.");
            }

            post.Status = PostStatus.Pending;
            _postRepository.Update(post);
            await _postRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Post '{Id}' submitted successfully and set to '{Status}'.", id, post.Status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while submitting post with ID {Id}.", id);
            throw;
        }
    }


    
    /// <inheritdoc />
    public async Task ApprovePostAsync(int id)
    {
        _logger.LogInformation("Attempting to approve post with ID: {Id}.", id);
        try
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is unavailable.");

            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                _logger.LogWarning("Unauthorized approval attempt for post ID {Id} by a non-admin user.", id);
                throw new UnauthorizedAccessException("Only administrators can approve posts.");
            }

            var post = await _postRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (post is null)
            {
                _logger.LogWarning("Post with ID {Id} was not found for approval.", id);
                throw new KeyNotFoundException($"Post with ID {id} was not found.");
            }

            if (post.Status != PostStatus.Pending)
            {
                _logger.LogWarning("Post with ID {Id} is in '{CurrentStatus}' state and cannot be approved.", id, post.Status);
                throw new InvalidOperationException($"Post with ID {id} is in '{post.Status}' state and cannot be approved.");
            }

            post.Status = PostStatus.Published;
            _postRepository.Update(post);
            await _postRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Post '{Id}' approved successfully and set to '{Status}'.", id, post.Status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while approving post with ID {Id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task RejectPostAsync(int id)
    {
        _logger.LogInformation("Attempting to reject post with ID: {Id}.", id);
        try
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is unavailable.");

            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                _logger.LogWarning("Unauthorized rejection attempt for post ID {Id} by a non-admin user.", id);
                throw new UnauthorizedAccessException("Only administrators can reject posts.");
            }

            var post = await _postRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (post is null)
            {
                _logger.LogWarning("Post with ID {Id} was not found for rejection.", id);
                throw new KeyNotFoundException($"Post with ID {id} was not found.");
            }

            if (post.Status != PostStatus.Pending)
            {
                _logger.LogWarning("Post with ID {Id} is in '{CurrentStatus}' state and cannot be rejected.", id, post.Status);
                throw new InvalidOperationException($"Post with ID {id} is in '{post.Status}' state and cannot be rejected.");
            }

            post.Status = PostStatus.Unpublished;
            _postRepository.Update(post);
            await _postRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Post '{Id}' rejected successfully and set to '{Status}'.", id, post.Status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while rejecting post with ID {Id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UnpublishPostAsync(int id)
    {
        _logger.LogInformation("Attempting to unpublish post with ID: {Id}.", id);
        try
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is unavailable.");

            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                _logger.LogWarning("Unauthorized attempt to unpublish post ID {Id} by a non-admin user.", id);
                throw new UnauthorizedAccessException("Only administrators can unpublish posts.");
            }

            var post = await _postRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (post is null)
            {
                _logger.LogWarning("Post with ID {Id} was not found for unpublishing.", id);
                throw new KeyNotFoundException($"Post with ID {id} was not found.");
            }

            if (post.Status != PostStatus.Published)
            {
                _logger.LogWarning("Post with ID {Id} is in '{CurrentStatus}' state and cannot be unpublished.", id, post.Status);
                throw new InvalidOperationException($"Post with ID {id} is in '{post.Status}' state and cannot be unpublished.");
            }

            post.Status = PostStatus.Unpublished;
            _postRepository.Update(post);
            await _postRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Post '{Id}' unpublished successfully and set to '{Status}'.", id, post.Status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while unpublishing post with ID {Id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task RestorePostAsync(int id)
    {
        _logger.LogInformation("Attempting to restore post with ID: {Id}.", id);
        try
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is unavailable.");

            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                _logger.LogWarning("Unauthorized restore attempt for post ID {Id} by a non-admin user.", id);
                throw new UnauthorizedAccessException("Only administrators can restore posts.");
            }

            var post = await _postRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (post is null)
            {
                _logger.LogWarning("Post with ID {Id} was not found for restoration.", id);
                throw new KeyNotFoundException($"Post with ID {id} was not found.");
            }

            if (post.Status != PostStatus.Unpublished)
            {
                _logger.LogWarning("Post with ID {Id} is in '{CurrentStatus}' state and cannot be restored.", id, post.Status);
                throw new InvalidOperationException($"Post with ID {id} is in '{post.Status}' state and cannot be restored.");
            }

            post.Status = PostStatus.Published;
            _postRepository.Update(post);
            await _postRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Post '{Id}' restored successfully and set to '{Status}'.", id, post.Status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while restoring post with ID {Id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<GetPostDTO> UpdatePostAsync(UpdatePostDTO updatePostDto)
    {
        if (updatePostDto is null)
        {
            _logger.LogError("UpdatePostAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(updatePostDto), "Post Update DTO cannot be null.");
        }

        _logger.LogInformation("Attempting to update post with ID: {Id}", updatePostDto.Id);

        try
        {
            var existingPost = await _postRepository.GetByIdAsync(updatePostDto.Id).ConfigureAwait(false);

            if (existingPost is null)
            {
                _logger.LogWarning("Post with ID {Id} was not found for update.", updatePostDto.Id);
                throw new KeyNotFoundException($"Post with ID {updatePostDto.Id} was not found.");
            }

            // Validate consistency between Title, Body, and Summary
            if (!string.Equals(existingPost.Title, updatePostDto.Title, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Title changed for post ID {Id}. Ensure that Body and Summary align with the new title.", updatePostDto.Id);
            }

            if (!string.Equals(existingPost.Body, updatePostDto.Body, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Body changed for post ID {Id}. Consider reviewing Title and Summary for consistency.", updatePostDto.Id);
            }

            if (!string.Equals(existingPost.Summary, updatePostDto.Summary, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Summary changed for post ID {Id}. Ensure alignment with Body and Title.", updatePostDto.Id);
            }

            // Apply updates to the entity
            _mapper.Map(updatePostDto, existingPost);
            _postRepository.Update(existingPost);
            await _postRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Post '{Id}' updated successfully.", updatePostDto.Id);

            return _mapper.Map<GetPostDTO>(existingPost);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating post with ID {Id}.", updatePostDto.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeletePostAsync(int id)
    {
        _logger.LogInformation("Attempting to delete post with ID: {Id}.", id);
        try
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is unavailable.");

            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                _logger.LogWarning("Unauthorized attempt to delete post ID {Id} by a non-admin user.", id);
                throw new UnauthorizedAccessException("Only administrators can delete posts.");
            }

            var post = await _postRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (post is null)
            {
                _logger.LogWarning("Post with ID {Id} was not found for deletion.", id);
                throw new KeyNotFoundException($"Post with ID {id} was not found.");
            }

            if (post.Status != PostStatus.Unpublished)
            {
                _logger.LogWarning("Post with ID {Id} is in '{CurrentStatus}' state and cannot be deleted.", id, post.Status);
                throw new InvalidOperationException($"Post with ID {id} is in '{post.Status}' state and cannot be deleted.");
            }

            post.Status = PostStatus.Deleted;
            _postRepository.Update(post);
            await _postRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Post '{Id}' deleted successfully and set to '{Status}'.", id, post.Status);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting post with ID {Id}.", id);
            throw;
        }
    }


}

       
