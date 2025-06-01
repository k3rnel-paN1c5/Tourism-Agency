using System;
using AutoMapper;
using Domain.Entities;
using Application.DTOs.TripPlanCar;

namespace Application.MappingProfiles;

public class TripPlanCarProfile : Profile
{
    public TripPlanCarProfile()
    {
        CreateMap<CreateTripPlanCarFromTripPlanDTO, CreateTripPlanCarDTO>()
            .ForMember(dest => dest.TripPlanId, opt => opt.MapFrom(src => src.TripPlanId))
            .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.TripPlan.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.TripPlan.EndDate));

        CreateMap<TripPlanCar, GetTripPlanCarDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TripPlanId, opt => opt.MapFrom(src => src.TripPlanId))
            .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ReverseMap();

        // Map from Create DTO -> Entity
        CreateMap<CreateTripPlanCarDTO, TripPlanCar>()
            .ForMember(dest => dest.TripPlanId, opt => opt.MapFrom(src => src.TripPlanId))
            .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))

            // Ignore navigation properties
            .ForMember(dest => dest.Car, opt => opt.Ignore())
            .ForMember(dest => dest.TripPlan, opt => opt.Ignore());

        // Map from Update DTO -> Entity
        CreateMap<UpdateTripPlanCarDTO, TripPlanCar>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TripPlanId, opt => opt.MapFrom(src => src.TripPlanId))
            .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))

            // Ignore navigation properties
            .ForMember(dest => dest.Car, opt => opt.Ignore())
            .ForMember(dest => dest.TripPlan, opt => opt.Ignore());
    }
}
