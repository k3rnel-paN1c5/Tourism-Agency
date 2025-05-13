using System;
using AutoMapper;
using Domain.Entities;
using Application.DTOs.Region;

namespace Application.MappingProfiles;

public class RegionProfile : Profile
{
    public RegionProfile(){
        CreateMap<Region, GetRegionDTO>();
        CreateMap<CreateRegionDTO, Region>();
        CreateMap<UpdateRegionDTO, Region>();
    }
}
