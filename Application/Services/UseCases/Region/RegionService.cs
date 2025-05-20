using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using Application.DTOs.Region;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;


namespace Application.Services.UseCases;

public class RegionService : IRegionService
{
    private readonly IRepository<Region, int> _regionRepo;
    private readonly IMapper _mapper;
    public RegionService(IRepository<Region, int> regionRepo, IMapper mapper){
        _regionRepo = regionRepo;
        _mapper = mapper;
    }
    public async Task<GetRegionDTO> CreateRegionAsync(CreateRegionDTO dto)
    {
        if (await _regionRepo.GetByPredicateAsync(r => dto.Name!.Equals(r.Name)) is not null)
            throw new ValidationException("Region with this name already exists.");

        Region region = _mapper.Map<Region>(dto);
        await _regionRepo.AddAsync(region);
        await _regionRepo.SaveAsync();
        return _mapper.Map<GetRegionDTO>(region);

    }
    public async Task UpdateRegionAsync(UpdateRegionDTO dto){
        var region = await _regionRepo.GetByIdAsync(dto.Id) 
            ?? throw new Exception($"Region {dto.Id} not found.");

        if (await _regionRepo.GetByPredicateAsync(r => dto.Name!.Equals(r.Name)) is not null)
            throw new ValidationException("Region with this name already exists.");

        _regionRepo.Update(_mapper.Map<Region>(dto));
        await _regionRepo.SaveAsync();
    }
    public async Task DeleteRegionAsync(int id){
        var region = await _regionRepo.GetByIdAsync(id) 
            ?? throw new Exception("Region not found");
        _regionRepo.Delete(region);
        await _regionRepo.SaveAsync();
    }

    public async Task<IEnumerable<GetRegionDTO>> GetAllRegionsAsync()
    {
        var regions = await _regionRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<GetRegionDTO>>(regions);
    }

    public async Task<GetRegionDTO> GetRegionByIdAsync(int id)
    {
        var region = await _regionRepo.GetByIdAsync(id) 
            ?? throw new Exception($"Region {id} not found.");
        return _mapper.Map<GetRegionDTO>(region);
    }
}
