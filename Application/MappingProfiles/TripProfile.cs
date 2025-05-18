using System;
using Application.DTOs.Trip;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles;

public class TripProfile : Profile
{
    public TripProfile()
    {
        // Map from Entity -> DTO (Get)
        CreateMap<Trip, GetTripDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
            .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(src => src.IsPrivate))
            .ReverseMap();

        // Map from DTO -> Entity (Create)
        CreateMap<CreateTripDTO, Trip>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Slug, opt => opt.MapFrom(src => src.Slug))
            .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsPrivate, opt => opt.MapFrom(src => src.IsPrivate))
            // Ignore navigation properties or set defaults if needed
            .ForMember(dest => dest.Plans, opt => opt.Ignore());

        // Map from DTO -> Entity (Update)
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