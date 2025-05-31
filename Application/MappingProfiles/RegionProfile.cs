using System;
using AutoMapper;
using Domain.Entities;
using Application.DTOs.Region;

namespace Application.MappingProfiles;

/// <summary>
/// AutoMapper profile for mapping between Region entities and their DTOs.
/// </summary>
public class RegionProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RegionProfile"/> class.
    /// Configures the mappings for Region-related DTOs and entities.
    /// </summary>
    public RegionProfile()
    {
        CreateMap<Region, GetRegionDTO>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
        .ReverseMap();
        CreateMap<CreateRegionDTO, Region>()
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
        .ForMember(dest => dest.Id, opt => opt.Ignore());

        CreateMap<UpdateRegionDTO, Region>()
        .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}
