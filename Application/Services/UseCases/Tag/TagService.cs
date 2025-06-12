using System;
using System.Security.Claims;
using Application.DTOs.Tag;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Services.UseCases;

/// <summary>
/// Provides business logic for managing Tags.
/// </summary>
public class TagService : ITagService
{
    private readonly IRepository<Tag, int> _tagRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILogger<TagService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TagService"/> class.
    /// </summary>
    /// <param name="tagRepository">The repository for Tag entities.</param>
    /// <param name="httpContextAccessor">The accessor for the current HTTP context.</param>
    /// <param name="mapper">The AutoMapper instance for DTO-entity mapping.</param>
    /// <param name="logger">The logger for this service.</param>
    public TagService(
        IRepository<Tag, int> tagRepository,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ILogger<TagService> logger)
    {
        _tagRepository = tagRepository ?? throw new ArgumentNullException(nameof(tagRepository));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    /// <inheritdoc />
    public async Task<GetTagDTO> CreateTagAsync(CreateTagDTO tagDto)
    {
        if (tagDto is null)
        {
            _logger.LogError("CreateTagAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(tagDto), "Tag creation DTO cannot be null.");
        }

        _logger.LogInformation("Attempting to create tag with name: {TagName}", tagDto.Name);

        try
        {
            if (string.IsNullOrWhiteSpace(tagDto.Name))
            {
                _logger.LogError("CreateTagAsync: Tag name is empty.");
                throw new ArgumentNullException(nameof(tagDto.Name), "Tag name cannot be empty.");
            }

            var tagExists = await CheckIfTagExistsAsync(tagDto.Name).ConfigureAwait(false);
            if (tagExists)
            {
                _logger.LogWarning("Tag '{TagName}' already exists. Duplicate creation prevented.", tagDto.Name);
                throw new InvalidOperationException($"Tag '{tagDto.Name}' already exists.");
            }

            var tagEntity = _mapper.Map<Tag>(tagDto);
            await _tagRepository.AddAsync(tagEntity).ConfigureAwait(false);
            await _tagRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Tag '{TagName}' created successfully with ID {TagId}.", tagDto.Name, tagEntity.Id);

            return _mapper.Map<GetTagDTO>(tagEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating tag '{TagName}'.", tagDto.Name);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<GetTagDTO> GetTagByIdAsync(int id)
    {
        _logger.LogInformation("Attempting to retrieve tag with ID: {Id}", id);

        try
        {
            var tag = await _tagRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (tag is null)
            {
                _logger.LogWarning("Tag with ID {Id} was not found.", id);
                throw new KeyNotFoundException($"Tag with ID {id} was not found.");
            }

            _logger.LogInformation("Tag '{Id}' retrieved successfully.", id);
            return _mapper.Map<GetTagDTO>(tag);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving tag with ID {Id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GetTagDTO>> GetAllTagsAsync()
    {
        _logger.LogInformation("Attempting to retrieve all tags.");

        try
        {
            var tags = await _tagRepository.GetAllAsync().ConfigureAwait(false);

            _logger.LogDebug("{Count} tags retrieved.", tags?.Count() ?? 0);

            return _mapper.Map<IEnumerable<GetTagDTO>>(tags);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all tags.");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<GetTagDTO> GetTagByNameAsync(string tagName)
    {
        _logger.LogInformation("Attempting to retrieve tag with Name: {TagName}", tagName);

        try
        {
            var tag = await _tagRepository.GetByPredicateAsync(t => t.Name == tagName)
                .ConfigureAwait(false);

            if (tag is null)
            {
                _logger.LogWarning("Tag with Name '{TagName}' was not found.", tagName);
                throw new KeyNotFoundException($"Tag with Name '{tagName}' was not found.");
            }

            _logger.LogInformation("Tag '{TagName}' retrieved successfully.", tagName);
            return _mapper.Map<GetTagDTO>(tag);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving tag with Name '{TagName}'.", tagName);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> CheckIfTagExistsAsync(string tagName)
    {
        _logger.LogInformation("Checking existence of tag with Name: {TagName}", tagName);

        try
        {
            var tag = await _tagRepository.GetByPredicateAsync(t => t.Name == tagName)
                .ConfigureAwait(false);

            var exists = tag is not null;

            if (!exists)
            {
                _logger.LogWarning("Tag with Name '{TagName}' does not exist.", tagName);
            }

            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while checking existence of tag with Name '{TagName}'.", tagName);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<GetTagDTO> UpdateTagAsync(UpdateTagDTO tagDto)
    {
        if (tagDto is null)
        {
            _logger.LogError("UpdateTagAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(tagDto), "Tag update DTO cannot be null.");
        }

        _logger.LogInformation("Attempting to update tag with ID: {TagId}, Name: {TagName}", tagDto.Id, tagDto.Name);

        try
        {
            var existingTag = await _tagRepository.GetByIdAsync(tagDto.Id).ConfigureAwait(false);
            if (existingTag is null)
            {
                _logger.LogWarning("Tag with ID {TagId} not found.", tagDto.Id);
                throw new KeyNotFoundException($"Tag with ID {tagDto.Id} not found.");
            }

            var tagExists = await CheckIfTagExistsAsync(tagDto.Name).ConfigureAwait(false);
            if (tagExists && existingTag.Name != tagDto.Name)
            {
                _logger.LogWarning("Tag name '{TagName}' already exists. Duplicate update prevented.", tagDto.Name);
                throw new InvalidOperationException($"Tag name '{tagDto.Name}' already exists.");
            }

            _mapper.Map(tagDto, existingTag);
            _tagRepository.Update(existingTag);
            await _tagRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Tag '{TagId}' updated successfully.", tagDto.Id);

            return _mapper.Map<GetTagDTO>(existingTag);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating tag '{TagId}'.", tagDto.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteTagAsync(int tagId)
    {
        _logger.LogInformation("Attempting to delete tag with ID: {TagId}", tagId);

        try
        {
            var existingTag = await _tagRepository.GetByIdAsync(tagId).ConfigureAwait(false);
            if (existingTag is null)
            {
                _logger.LogWarning("Tag with ID {TagId} was not found for deletion.", tagId);
                throw new KeyNotFoundException($"Tag with ID {tagId} was not found.");
            }

            _logger.LogInformation("Deleting tag '{TagId}' and all associated posts due to ON DELETE CASCADE.", tagId);

            await _tagRepository.DeleteByIdAsync(tagId).ConfigureAwait(false);
            await _tagRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Tag '{TagId}' deleted successfully, along with all related posts.", tagId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting tag '{TagId}'.", tagId);
            throw;
        }
    }

}

