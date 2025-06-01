using System;
using AutoMapper;
using Domain.Entities;
using Application.DTOs.TripBooking;
using Application.DTOs.Booking;
using Application.DTOs.Payment;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Domain.Enums;
using Application.Utilities;

namespace Application.MappingProfiles;

/// <summary>
/// AutoMapper profile for mapping between Booking entities and Booking DTOs.
/// Defines mappings for retrieving, creating, and updating booking information.
/// </summary>
public class BookingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BookingProfile"/> class.
    /// Configures the mapping rules for Booking related objects.
    /// </summary>
    public BookingProfile()
    {

        // Map from Booking Entity to GetBookingDTO
        // This mapping is used when retrieving booking data to be sent to the client.
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

        // Map from CreateBookingDTO to Booking Entity
        // This mapping is used when creating a new booking from client-provided data.
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

        CreateMap<Booking, CreatePaymentDTO>()
            .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.AmountDue,
                opt => opt.MapFrom(src => src.BookingType ?
                    TripBookingAmountDueCalculator.CalculateAmountDue((decimal)6.0, src.NumOfPassengers):
                    CarBookingAmountDueCalculator.CalculateAmountDue(src.StartDate, src.EndDate, src.CarBooking!.Car!.Ppd, src.CarBooking.Car.Ppd)));

        // Map from UpdateBookingDTO to Booking Entity
        // This mapping is used when updating an existing booking with client-provided data.
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
