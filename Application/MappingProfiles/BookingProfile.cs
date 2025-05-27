using System;
using AutoMapper;
using Domain.Entities;
using Application.DTOs.TripBooking;
using Application.DTOs.Booking;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Domain.Enums;

namespace Application.MappingProfiles;

public class BookingProfile : Profile
{

    public BookingProfile()
    {


        // Map from Entity -> Get DTO
        CreateMap<Booking, GetBookingDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.BookingType, opt => opt.MapFrom(src => src.BookingType))
            .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
            .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.NumOfPassengers, opt => opt.MapFrom(src => src.NumOfPassengers))
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
            .ReverseMap();

        // Map from Create DTO -> Booking Entity
        CreateMap<CreateBookingDTO, Booking>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.BookingType, opt => opt.Ignore()) // Or set manually if needed
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => BookingStatus.Pending)) // Default value
            .ForMember(dest => dest.NumOfPassengers, opt => opt.MapFrom(src => src.NumOfPassengers))
            .ForMember(dest => dest.CustomerId, opt => opt.Ignore()) 
            .ForMember(dest => dest.EmployeeId, opt => opt.Ignore()) //  assigned later
            .ForMember(dest => dest.CarBooking, opt => opt.Ignore())
            .ForMember(dest => dest.TripBooking, opt => opt.Ignore())
            .ForMember(dest => dest.Payment, opt => opt.Ignore());

        CreateMap<UpdateBookingDTO, Booking>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.NumOfPassengers, opt => opt.MapFrom(src => src.NumOfPassengers))
                .ForMember(dest => dest.BookingType, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.CarBooking, opt => opt.Ignore())
                .ForMember(dest => dest.TripBooking, opt => opt.Ignore())
                .ForMember(dest => dest.Payment, opt => opt.Ignore());
    }
}
