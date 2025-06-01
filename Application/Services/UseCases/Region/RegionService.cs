using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Application.IServices.UseCases;
using Application.DTOs.Region;
using Domain.Entities;
using Domain.IRepositories;

namespace Application.Services.UseCases;

/// <summary>
/// Provides business logic for Region management.
/// </summary>
public class RegionService : IRegionService
{
    private readonly IRepository<Region, int> _regionRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<RegionService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="RegionService"/> class.
    /// </summary>
    /// <param name="regionRepository">The repository for Region entities.</param>
    /// <param name="mapper">The AutoMapper instance for DTO-entity mapping.</param>
    /// <param name="logger">The logger for this service.</param>
    public RegionService(IRepository<Region, int> regionRepository, IMapper mapper, ILogger<RegionService> logger)
    {
        _regionRepository = regionRepository ?? throw new ArgumentNullException(nameof(regionRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<GetRegionDTO> CreateRegionAsync(CreateRegionDTO createRegionDto)
    {
        _logger.LogInformation("Attempting to create region: {RegionName}", createRegionDto.Name);
        try
        {
            if (createRegionDto is null)
            {
                _logger.LogError("CreateRegionAsync called with null DTO.");
                throw new ArgumentNullException(nameof(createRegionDto), "Region creation DTO cannot be null.");
            }

            var existingRegion = await _regionRepository.GetByPredicateAsync(r => r.Name!.Equals(createRegionDto.Name)).ConfigureAwait(false);
            if (existingRegion is not null)
            {
                _logger.LogWarning("Region with name '{RegionName}' already exists. Creation failed.", createRegionDto.Name);
                throw new InvalidOperationException($"Region with name '{createRegionDto.Name}' already exists.");
            }
            var regionEntity = _mapper.Map<Region>(createRegionDto);

            await _regionRepository.AddAsync(regionEntity).ConfigureAwait(false);
            await _regionRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Region '{RegionName}' (ID: {RegionId}) created successfully.", regionEntity.Name, regionEntity.Id);
            return _mapper.Map<GetRegionDTO>(regionEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while adding region with Name: {RegionName}, {ErrorMessage}", createRegionDto.Name, ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<GetRegionDTO> GetRegionByIdAsync(int id)
    {
        _logger.LogInformation("Attempting to retrieve region with ID: {RegionId}", id);
        try
        {
            var regionEntity = await _regionRepository.GetByIdAsync(id).ConfigureAwait(false);

            if (regionEntity is null)
            {
                _logger.LogWarning("Region with ID: {RegionId} not found.", id);
                throw new KeyNotFoundException($"Region with ID '{id}' not found.");
            }

            _logger.LogInformation("Region with ID: {RegionId} retrieved successfully.", id);
            return _mapper.Map<GetRegionDTO>(regionEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while retrieving region with id: {RegionID}. Error: {ErrorMessage}", id, ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GetRegionDTO>> GetAllRegionsAsync()
    {
        _logger.LogInformation("Attempting to retrieve all regions.");
        try
        {
            var regions = await _regionRepository.GetAllAsync().ConfigureAwait(false);
            _logger.LogInformation("Retrieved all regions successfully");
            return _mapper.Map<IEnumerable<GetRegionDTO>>(regions);

        }
        catch (Exception ex)
        {
            _logger.LogError("Error while retrieving all regions. Error: {ErrorMessage}", ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateRegionAsync(UpdateRegionDTO updateRegionDto)
    {
        _logger.LogInformation("Attempting to update region with ID: {RegionId}", updateRegionDto.Id);
        if (updateRegionDto is null)
        {
            _logger.LogWarning("UpdateRegionAsync called with null DTO.");
            throw new ArgumentNullException(nameof(updateRegionDto), "Region update DTO cannot be null.");
        }
        try
        {
            var existingRegion = await _regionRepository.GetByIdAsync(updateRegionDto.Id).ConfigureAwait(false);
            if (existingRegion is null)
            {
                _logger.LogWarning("Region with ID: {RegionId} not found for update.", updateRegionDto.Id);
                throw new KeyNotFoundException($"Region with ID '{updateRegionDto.Id}' not found.");
            }

            // Only check for uniqueness if name has changed
            if (!existingRegion.Name!.Equals(updateRegionDto.Name, StringComparison.OrdinalIgnoreCase))
            {
                var existingRegionWithSameName = await _regionRepository.GetByPredicateAsync(r => r.Name!.Equals(updateRegionDto.Name) && r.Id != updateRegionDto.Id).ConfigureAwait(false);
                if (existingRegionWithSameName is not null)
                {
                    _logger.LogWarning("Another region with name '{RegionName}' already exists. Update failed for ID: {RegionId}.", updateRegionDto.Name, updateRegionDto.Id);
                    throw new InvalidOperationException($"Another region with name '{updateRegionDto.Name}' already exists.");
                }
            }
            _mapper.Map(updateRegionDto, existingRegion);
            _regionRepository.Update(existingRegion);
            await _regionRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Region '{Name}' updated successfully.", existingRegion.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while updating region with ID {Id}. Error: {Message}", updateRegionDto.Id, ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteRegionAsync(int id)
    {
        _logger.LogInformation("Attempting to delete region with ID: {RegionId}", id);
        try
        {
            var region = await _regionRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (region is null)
            {
                _logger.LogWarning("Region with ID: {RegionId} not found for deletion.", id);
                throw new KeyNotFoundException($"Region with ID '{id}' not found.");
            }
            _regionRepository.Delete(region);
            await _regionRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Region with ID: {RegionId} deleted successfully.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while deleting region with ID {Id}. Error: {errorMessage}", id, ex.Message);
            throw;
        }
    }
}