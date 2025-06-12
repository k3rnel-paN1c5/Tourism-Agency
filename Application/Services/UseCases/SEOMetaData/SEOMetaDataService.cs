using System;
using System.Resources;
using Application.DTOs.SEOMetaData;
using Application.IServices.UseCases.SEOMetaData;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.Extensions.Logging;

namespace Application.Services.UseCases.SEOMetaData;

/// <summary>
/// Provides business logic for SEO meta data management.
/// </summary>
public class SEOMetaDataService : ISEOMetaDataService
{
    private readonly IRepository<SEOMetadata, int> _seoRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SEOMetaDataService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="SEOMetaDataService"/> class.
    /// </summary>
    /// <param name="seoRepository">The repository for SEO metadata entities.</param>
    /// <param name="mapper">The AutoMapper instance for DTO-entity mapping.</param>
    /// <param name="logger">The logger for this service.</param>
    public SEOMetaDataService(
        IRepository<SEOMetadata, int> seoRepository,
        IMapper mapper,
        ILogger<SEOMetaDataService> logger)
    {
        _seoRepository = seoRepository ?? throw new ArgumentNullException(nameof(seoRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<GetSEOMetaDataDTO> CreateSEOMetaDataAsync(CreateSEOMetaDataDTO createSEOMetaDataDto)
    {
        _logger.LogInformation("Attempting to create SEO metadata: {title}", createSEOMetaDataDto.MetaTitle);
        try
        {
            if (createSEOMetaDataDto is null)
            {
                _logger.LogError("CreateSEOMetaDataAsync called with null DTO.");
                throw new ArgumentNullException(nameof(createSEOMetaDataDto), "SEO creation DTO cannot be null.");
            }


            var seoEntity = _mapper.Map<SEOMetadata>(createSEOMetaDataDto);

            await _seoRepository.AddAsync(seoEntity).ConfigureAwait(false);
            await _seoRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("SEO '{title}' (ID: {id}) created successfully.", seoEntity.MetaTitle, seoEntity.Id);
            return _mapper.Map<GetSEOMetaDataDTO>(seoEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while adding SEO with Title: {title}", createSEOMetaDataDto.MetaTitle);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<GetSEOMetaDataDTO> GetSEOMetaDataByIdAsync(int id)
    {
        _logger.LogInformation("Attempting to retrieve SEO with ID: {id}", id);
        try
        {
            var seoEntity = await _seoRepository.GetByIdAsync(id).ConfigureAwait(false);

            if (seoEntity is null)
            {
                _logger.LogWarning("SEO enitityy with ID: {id} not found.", id);
                throw new KeyNotFoundException($"SEO with ID '{id}' not found.");
            }

            _logger.LogInformation("SEO with ID: {id} retrieved successfully.", id);
            return _mapper.Map<GetSEOMetaDataDTO>(seoEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving SEO with id: {id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GetSEOMetaDataDTO>> GetAllSEOMetaDataAsync()
    {
        _logger.LogInformation("Attempting to retrieve all SEO entities.");
        try
        {
            var seoEntities = await _seoRepository.GetAllAsync().ConfigureAwait(false);
            _logger.LogInformation("Retrieved all seo Entities successfully");
            return _mapper.Map<IEnumerable<GetSEOMetaDataDTO>>(seoEntities);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving all seo Entities");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GetSEOMetaDataDTO>> GetSEOMetaDataByPostIdAsync(int id)
    {
        _logger.LogInformation("Attempting to retrieve all SEO entities belongig to post {id}.", id);
        try
        {
            var seoEntities = await _seoRepository.GetAllByPredicateAsync(s => s.PostId == id).ConfigureAwait(false);
            _logger.LogInformation("Retrieved all seo Entities belonging to post {id} successfully", id);
            return _mapper.Map<IEnumerable<GetSEOMetaDataDTO>>(seoEntities);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving all seo Entities with post {id}", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateSEOMetaDataAsync(UpdateSEOMetaDataDTO UpdateSEOMetaDataDTO)
    {
        _logger.LogInformation("Attempting to update seo with ID: {id}", UpdateSEOMetaDataDTO.Id);
        if (UpdateSEOMetaDataDTO is null)
        {
            _logger.LogWarning("UpdateSEOMetaDataAsync called with null DTO.");
            throw new ArgumentNullException(nameof(UpdateSEOMetaDataDTO), "SEO update DTO cannot be null.");
        }
        try
        {
            var existingSEO = await _seoRepository.GetByIdAsync(UpdateSEOMetaDataDTO.Id).ConfigureAwait(false);
            if (existingSEO is null)
            {
                _logger.LogWarning("SEO entity with ID: {id} not found for update.", UpdateSEOMetaDataDTO.Id);
                throw new KeyNotFoundException($"SEO entity with ID '{UpdateSEOMetaDataDTO.Id}' not found.");
            }


            _mapper.Map(UpdateSEOMetaDataDTO, existingSEO);
            _seoRepository.Update(existingSEO);
            await _seoRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("SEO entity '{title}' updated successfully.", existingSEO.MetaTitle);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating SEO entity with ID {Id}.", UpdateSEOMetaDataDTO.Id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteSEOMetaDataAsync(int id)
    {
        _logger.LogInformation("Attempting to delete SEO with ID: {ids}", id);
        try
        {
            var seoEntity = await _seoRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (seoEntity is null)
            {
                _logger.LogWarning("SEO entity with ID: {id} not found for deletion.", id);
                throw new KeyNotFoundException($"SEO entity ID '{id}' not found.");
            }
            _seoRepository.Delete(seoEntity);
            await _seoRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("SEO entity with ID: {id} deleted successfully.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting SEO entity with ID {Id}.", id);
            throw;
        }
    }


}
