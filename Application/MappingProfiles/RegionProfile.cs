using System;
using AutoMapper;
using Domain.Entities;
using Application.DTOs.Region;

namespace Application.MappingProfiles;

public class RegionProfile : Profile
{
    public RegionProfile(){
        CreateMap<Region, GetRegionDTO>()
        .ForMember(dest => dest.Id, opt=>opt.MapFrom(src=>src.Id))
        .ForMember(dest => dest.Name, opt=>opt.MapFrom(src=>src.Name))
        .ReverseMap();
        CreateMap<CreateRegionDTO, Region>()
        .ForMember(dest=>dest.Name, opt=>opt.MapFrom(src=>src.Name))
        .ForMember(dest=>dest.Id, opt=>opt.Ignore());

        CreateMap<UpdateRegionDTO, Region>()
        .ForMember(dest => dest.Id, opt=>opt.MapFrom(src=>src.Id))
        .ForMember(dest => dest.Name, opt=>opt.MapFrom(src=>src.Name));
    }
}
