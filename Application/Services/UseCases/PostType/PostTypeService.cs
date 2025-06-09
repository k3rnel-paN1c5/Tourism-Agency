using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Application.DTOs.PostType;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Services.UseCases;

/// <summary>
/// Provides business logic for managing post types.
/// </summary>
public class PostTypeService : IPostTypeService
{
    private readonly IRepository<PostType, int> _postTypeRepository;
    private readonly IRepository<Post, int> _postRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<PostTypeService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="PostTypeService"/> class.
    /// </summary>
    /// <param name="postTypeRepository">The repository for managing post type entities.</param>
    /// <param name="postRepository">The repository for managing posts associated with types.</param>
    /// <param name="mapper">The AutoMapper instance for DTO-entity mapping.</param>
    /// <param name="logger">The logger for tracking operations.</param>
    /// <param name="httpContextAccessor">The accessor for the current HTTP context.</param>

    public PostTypeService(
        IRepository<PostType, int> postTypeRepository,
        IRepository<Post, int> postRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<PostTypeService> logger)
    {
        _postTypeRepository = postTypeRepository ?? throw new ArgumentNullException(nameof(postTypeRepository));
        _postRepository = postRepository ?? throw new ArgumentNullException(nameof(postRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    /// <inheritdoc />
    public async Task<PostTypeDto> CreatePostTypeAsync(CreatePostTypeDTO postTypeDto)
    {
        if (postTypeDto is null)
        {
            _logger.LogError("CreatePostTypeAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(postTypeDto), "Post type creation DTO cannot be null.");
        }

        _logger.LogInformation("Attempting to create post type with title: {Title}", postTypeDto.Title);

        try
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is unavailable.");

            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                _logger.LogWarning("Unauthorized attempt to create post type '{Title}' by a non-admin user.", postTypeDto.Title);
                throw new UnauthorizedAccessException("Only administrators can create post types.");
            }

            if (string.IsNullOrWhiteSpace(postTypeDto.Title))
            {
                _logger.LogWarning("Post type creation failed due to empty title.");
                throw new ValidationException("Post type title cannot be empty!");
            }

            if (string.IsNullOrWhiteSpace(postTypeDto.Description))
            {
                _logger.LogWarning("Post type creation failed due to empty description.");
                throw new ValidationException("Post type description cannot be empty!");
            }

            var postType = _mapper.Map<PostType>(postTypeDto);
            await _postTypeRepository.AddAsync(postType).ConfigureAwait(false);
            await _postTypeRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Post type '{Title}' created successfully with ID {Id}.", postType.Title, postType.Id);

            return _mapper.Map<PostTypeDto>(postType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating post type.");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PostTypeDto> GetPostTypeByIdAsync(int id)
    {
        _logger.LogInformation("Attempting to retrieve post type with ID: {Id}", id);
        try
        {
            var postType = await _postTypeRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (postType is null)
            {
                _logger.LogWarning("Post type with ID {Id} was not found.", id);
                throw new KeyNotFoundException($"Post type with ID {id} was not found.");
            }

            _logger.LogInformation("Post type '{Id}' retrieved successfully.", id);
            return _mapper.Map<PostTypeDto>(postType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving post type with ID {Id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<PostTypeDto>> GetAllPostTypeAsync()
    {
        _logger.LogInformation("Attempting to retrieve all post types.");
        try
        {
            var postTypes = await _postTypeRepository.GetAllAsync().ConfigureAwait(false);

            _logger.LogDebug("{Count} post types retrieved.", postTypes?.Count() ?? 0);

            return _mapper.Map<IEnumerable<PostTypeDto>>(postTypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all post types.");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<PostTypeDto> UpdatePostTypeAsync(UpdatePostTypeDTO postTypeDto)
    {
        if (postTypeDto is null)
        {
            _logger.LogError("UpdatePostTypeAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(postTypeDto), "Post Type Update DTO cannot be null.");
        }

        _logger.LogInformation("Attempting to update post type with ID: {Id}", postTypeDto.Id);

        try
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is unavailable.");

            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                _logger.LogWarning("Unauthorized attempt to update post type ID {Id} by a non-admin user.", postTypeDto.Id);
                throw new UnauthorizedAccessException("Only administrators can update post types.");
            }

            var existingPostType = await _postTypeRepository.GetByIdAsync(postTypeDto.Id).ConfigureAwait(false);

            if (existingPostType is null)
            {
                _logger.LogWarning("Post type with ID {Id} was not found for update.", postTypeDto.Id);
                throw new KeyNotFoundException($"Post type with ID {postTypeDto.Id} was not found.");
            }

            // Check title change and notify to review description
            if (!string.Equals(existingPostType.Title, postTypeDto.Title, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Title changed for post type ID {Id}. Ensure that the description aligns with the new title.", postTypeDto.Id);
            }

            // Check description change and notify to review title
            if (!string.Equals(existingPostType.Description, postTypeDto.Description, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Description changed for post type ID {Id}. Consider reviewing the title for consistency.", postTypeDto.Id);
            }

            _mapper.Map(postTypeDto, existingPostType);

            _postTypeRepository.Update(existingPostType);
            await _postTypeRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Post type '{Id}' updated successfully.", postTypeDto.Id);

            return _mapper.Map<PostTypeDto>(existingPostType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating post type with ID {Id}.", postTypeDto.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeletePostTypeAsync(int postTypeId)
    {
        _logger.LogInformation("Attempting to delete post type with ID: {Id}", postTypeId);

        try
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is unavailable.");

            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            if (role != "Admin")
            {
                _logger.LogWarning("Unauthorized attempt to delete post type ID {Id} by a non-admin user.", postTypeId);
                throw new UnauthorizedAccessException("Only administrators can delete post types.");
            }

            var postType = await _postTypeRepository.GetByIdAsync(postTypeId).ConfigureAwait(false);
            if (postType is null)
            {
                _logger.LogWarning("Post type with ID {Id} was not found for deletion.", postTypeId);
                throw new KeyNotFoundException($"Post type with ID {postTypeId} was not found.");
            }

            // Check for associated posts
            var associatedPosts = await _postRepository.GetAllByPredicateAsync(p => p.PostTypeId == postTypeId).ConfigureAwait(false);
            if (associatedPosts.Any())
            {
                // Ensure Default PostType exists
                var defaultPostType = await _postTypeRepository.GetByPredicateAsync(pt => pt.Title == "Normal").ConfigureAwait(false);
                if (defaultPostType is null)
                {
                    _logger.LogWarning("Default post type ('Normal') does not exist. Please create it first.");
                    throw new InvalidOperationException("Default post type ('Normal') is required before deleting a post type with associated posts.");
                }

                _logger.LogInformation("{Count} posts are linked to post type ID {Id}. Reassigning them to 'Normal'.", associatedPosts.Count(), postTypeId);

                // Reassign all posts to Default PostType
                foreach (var post in associatedPosts)
                {
                    post.PostTypeId = defaultPostType.Id;
                    _postRepository.Update(post);
                }
                await _postRepository.SaveAsync().ConfigureAwait(false);
            }

            // Delete post type after reassigning posts
            _postTypeRepository.Delete(postType);
            await _postTypeRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Post type '{Id}' deleted successfully after reassigning linked posts.", postTypeId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting post type with ID {Id}.", postTypeId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> CheckIfPostTypeExistsAsync(int id)
    {
        _logger.LogInformation("Checking existence of post type with ID: {Id}", id);
        try
        {
            var postType = await _postTypeRepository.GetByIdAsync(id).ConfigureAwait(false);
            var exists = postType is not null;

            if (!exists)
            {
                _logger.LogWarning("Post type with ID {Id} does not exist.", id);
            }

            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while checking existence of post type with ID {Id}.", id);
            throw;
        }
    }

         


}

