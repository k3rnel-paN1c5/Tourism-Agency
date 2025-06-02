using System;
using AutoMapper;
using Domain.Entities;
using Application.DTOs.TripBooking;
using Application.DTOs.Booking;

namespace Application.MappingProfiles;

public class TripBookingProfile : Profile
{
    public TripBookingProfile()
    {
        // Map from Entity -> Get DTO
        CreateMap<TripBooking, GetTripBookingDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BookingId))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Booking!.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Booking!.EndDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Booking!.Status))
            .ForMember(dest => dest.NumOfPassengers, opt => opt.MapFrom(src => src.Booking!.NumOfPassengers))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Booking!.CustomerId))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Booking!.EmployeeId))
            .ForMember(dest => dest.TripPlanId, opt => opt.MapFrom(src => src.TripPlanId))
            .ForMember(dest => dest.WithGuide, opt => opt.MapFrom(src => src.WithGuide))
            .ReverseMap();

        CreateMap<CreateTripBookingDTO, TripBooking>()
                .ForMember(dest => dest.BookingId, opt => opt.Ignore())
                .ForMember(dest => dest.TripPlanId, opt => opt.MapFrom(src => src.TripPlanId))
                .ForMember(dest => dest.WithGuide, opt => opt.MapFrom(src => src.WithGuide))
                .ForMember(dest => dest.TripPlan, opt => opt.Ignore())
                .ForMember(dest => dest.Booking, opt => opt.Ignore());

        CreateMap<CreateTripBookingDTO, CreateBookingDTO>()
                .ForMember(dest => dest.BookingType, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.NumOfPassengers, opt => opt.MapFrom(src => src.NumOfPassengers));

        // Map from Update DTO -> TripBooking
        CreateMap<UpdateTripBookingDTO, TripBooking>()
            .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.TripPlanId, opt => opt.Ignore())
            .ForMember(dest => dest.WithGuide, opt => opt.MapFrom(src => src.WithGuide))

            // Ignore navigation properties
            .ForMember(dest => dest.TripPlan, opt => opt.Ignore())
            .ForMember(dest => dest.Booking, opt => opt.Ignore());
    }
}
