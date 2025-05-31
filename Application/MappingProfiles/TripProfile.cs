using System;
using Application.DTOs.Trip;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles;

/// <summary>
/// AutoMapper profile for mapping between Trip entities and their DTOs.
/// </summary>
public class TripProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TripProfile"/> class.
    /// Configures the mappings for Trip-related DTOs and entities.
    /// </summary>
    public TripProfile()
    {
        CreateMap<Trip, GetTripDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
            .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(src => src.IsPrivate))
            .ReverseMap();

        CreateMap<CreateTripDTO, Trip>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug))
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(src => src.IsPrivate))
            .ForMember(dest => dest.Plans, opt => opt.Ignore());

        CreateMap<UpdateTripDTO, Trip>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug))
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(src => src.IsPrivate))
            .ForMember(dest => dest.Plans, opt => opt.Ignore());
    }
}