using System.ComponentModel.DataAnnotations;
using Application.DTOs.Region;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.Extensions.Logging;


namespace Application.Services.UseCases;

public class RegionService : IRegionService
{
    private readonly IRepository<Region, int> _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<RegionService> _logger;
    public RegionService(IRepository<Region, int> repository, IMapper mapper, ILogger<RegionService> logger)
    {
        _repo = repository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<GetRegionDTO> CreateRegionAsync(CreateRegionDTO dto)
    {
        if (dto == null)
        {
            _logger.LogError("CreateRegionAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(dto));
        }
        try
        {
            if (await _repo.GetByPredicateAsync(r => r.Name!.Equals(dto.Name)) is not null)
            {
                _logger.LogWarning("A region with the name '{Name}' already exists.", dto.Name);
                throw new ValidationException("A region with the same name already exists.");
            }

            var regionEntity = _mapper.Map<Region>(dto);

            await _repo.AddAsync(regionEntity).ConfigureAwait(false);
            await _repo.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Region '{Name}' created successfully with ID {Id}.", regionEntity.Name, regionEntity.Id);

            return _mapper.Map<GetRegionDTO>(regionEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a region.");
            throw;
        }

    }
    public async Task UpdateRegionAsync(UpdateRegionDTO dto)
    {
        if (dto == null)
        {
            _logger.LogError("UpdateRegionAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(dto));
        }

        try
        {
            var existingRegion = await _repo.GetByIdAsync(dto.Id).ConfigureAwait(false)
                ?? throw new ArgumentException($"Region with ID {dto.Id} was not found.");

            // Only check for uniqueness if name has changed
            if (!existingRegion.Name!.Equals(dto.Name, StringComparison.OrdinalIgnoreCase))
            {
                if (await _repo.GetByPredicateAsync(r => r.Name!.Equals(dto.Name)) is not null)
                {
                    _logger.LogWarning("A region with the name '{Name}' already exists.", dto.Name);
                    throw new ValidationException("A region with the same name already exists.");
                }
            }

            existingRegion = _mapper.Map<Region>(dto); // Update existing entity
            _repo.Update(existingRegion);
            await _repo.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Region '{Name}' updated successfully.", existingRegion.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating region with ID {Id}.", dto.Id);
            throw;
        }
    }
    public async Task DeleteRegionAsync(int id)
    {
        try
        {
            var region = await _repo.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new ArgumentException($"Region with ID {id} was not found.");

            _repo.Delete(region);
            await _repo.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Region '{Name}' deleted successfully.", region.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting region with ID {Id}.", id);
            throw;
        }
    }

    public async Task<IEnumerable<GetRegionDTO>> GetAllRegionsAsync()
    {
        try
        {
            var regions = await _repo.GetAllAsync().ConfigureAwait(false);
            _logger.LogDebug("Retrieved {Count} regions.", regions?.Count() ?? 0);
            return _mapper.Map<IEnumerable<GetRegionDTO>>(regions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all regions.");
            throw;
        }
    }

    public async Task<GetRegionDTO> GetRegionByIdAsync(int id)
    {
        try
        {
            var region = await _repo.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new ArgumentException($"Region with ID {id} was not found.");

            _logger.LogDebug("Region '{Name}' retrieved successfully.", region.Name);
            return _mapper.Map<GetRegionDTO>(region);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving region with ID {Id}.", id);
            throw;
        }
    }
}
