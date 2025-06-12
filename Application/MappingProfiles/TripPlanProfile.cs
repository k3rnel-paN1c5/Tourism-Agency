using System;
using AutoMapper;
using Domain.Entities;
using Application.DTOs.TripPlan;

namespace Application.MappingProfiles;

/// <summary>
/// AutoMapper profile for mapping between Trip Plan entities and their DTOs.
/// </summary>
public class TripPlanProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TripPlanProfile"/> class.
    /// Configures the mappings for Trip Plan-related DTOs and entities.
    /// </summary>
    public TripPlanProfile()
    {
        CreateMap<TripPlan, GetTripPlanDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TripId, opt => opt.MapFrom(src => src.TripId))
                .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.RegionId))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.IncludedServices, opt => opt.MapFrom(src => src.IncludedServices))
                .ForMember(dest => dest.Stops, opt => opt.MapFrom(src => src.Stops))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.MealsPlan, opt => opt.MapFrom(src => src.MealsPlan))
                .ForMember(dest => dest.HotelStays, opt => opt.MapFrom(src => src.HotelStays))


                .ForMember(dest => dest.Trip, opt => opt.Ignore())
                .ForMember(dest => dest.Region, opt => opt.Ignore())
                .ForMember(dest => dest.TripPlanCars, opt => opt.Ignore()) 
                .ReverseMap();

        // Map from Create DTO -> Entity
        CreateMap<CreateTripPlanDTO, TripPlan>()
            .ForMember(dest => dest.TripId, opt => opt.MapFrom(src => src.TripId))
            .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.RegionId))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.IncludedServices, opt => opt.MapFrom(src => src.IncludedServices))
            .ForMember(dest => dest.Stops, opt => opt.MapFrom(src => src.Stops))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.MealsPlan, opt => opt.MapFrom(src => src.MealsPlan))
            .ForMember(dest => dest.HotelStays, opt => opt.MapFrom(src => src.HotelStays))

            // Ignore navigation properties or set manually later
            .ForMember(dest => dest.Trip, opt => opt.Ignore())
            .ForMember(dest => dest.Region, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore())
            .ForMember(dest => dest.PlanCars, opt => opt.Ignore())
            .ReverseMap();


        // Map from Update DTO -> Entity
        CreateMap<UpdateTripPlanDTO, TripPlan>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TripId, opt => opt.MapFrom(src => src.TripId))
            .ForMember(dest => dest.RegionId, opt => opt.MapFrom(src => src.RegionId))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.IncludedServices, opt => opt.MapFrom(src => src.IncludedServices))
            .ForMember(dest => dest.Stops, opt => opt.MapFrom(src => src.Stops))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.MealsPlan, opt => opt.MapFrom(src => src.MealsPlan))
            .ForMember(dest => dest.HotelStays, opt => opt.MapFrom(src => src.HotelStays))

            // Ignore navigation properties
            .ForMember(dest => dest.Trip, opt => opt.Ignore())
            .ForMember(dest => dest.Region, opt => opt.Ignore())
            .ForMember(dest => dest.Bookings, opt => opt.Ignore())
            .ForMember(dest => dest.PlanCars, opt => opt.Ignore());
    }
}
