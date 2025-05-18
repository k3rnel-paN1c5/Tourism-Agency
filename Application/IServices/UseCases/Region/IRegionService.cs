using System;
using Application.DTOs.Region;
namespace Application.IServices.UseCases;

public interface IRegionService
{
    public Task<GetRegionDTO> CreateRegionAsync(CreateRegionDTO dto);
    public Task UpdateRegionAsync(UpdateRegionDTO dto);
    public Task DeleteRegionAsync(int id);
    Task<IEnumerable<GetRegionDTO>> GetAllRegionsAsync();
    Task<GetRegionDTO> GetRegionByIdAsync(int id);
}
