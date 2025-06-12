using System;
using AutoMapper;
using Domain.Entities;
using Application.DTOs.TripPlanCar;

namespace Application.MappingProfiles;

/// <summary>
/// AutoMapper profile for mapping between Trip Plan Car entities and their DTOs.
/// </summary>
public class TripPlanCarProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TripPlanCarProfile"/> class.
    /// Configures the mappings for Trip Plan-related DTOs and entities.
    /// </summary>
    public TripPlanCarProfile()
    {
        CreateMap<CreateTripPlanCarFromTripPlanDTO, CreateTripPlanCarDTO>()
            .ForMember(dest => dest.TripPlanId, opt => opt.MapFrom(src => src.TripPlanId))
            .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.TripPlan!.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.TripPlan!.EndDate));

        CreateMap<TripPlanCar, GetTripPlanCarDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TripPlanId, opt => opt.MapFrom(src => src.TripPlanId))
            .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
            .ReverseMap();

        // Map from Create DTO -> Entity
        CreateMap<CreateTripPlanCarDTO, TripPlanCar>()
            .ForMember(dest => dest.TripPlanId, opt => opt.MapFrom(src => src.TripPlanId))
            .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))

            // Ignore navigation properties
            .ForMember(dest => dest.Car, opt => opt.Ignore())
            .ForMember(dest => dest.TripPlan, opt => opt.Ignore());

        // Map from Update DTO -> Entity
        CreateMap<UpdateTripPlanCarDTO, TripPlanCar>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TripPlanId, opt => opt.MapFrom(src => src.TripPlanId))
            .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))

            // Ignore navigation properties
            .ForMember(dest => dest.Car, opt => opt.Ignore())
            .ForMember(dest => dest.TripPlan, opt => opt.Ignore());
    }
}
