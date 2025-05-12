using System;
using AutoMapper;
using Domain.Entities;
using Application.DTOs.TripBooking;

namespace Application.MappingProfiles;

public class TripBookingProfile : Profile
{
    public TripBookingProfile(){
        CreateMap<TripBooking, GetTripBookingDTO>();
        CreateMap<CreateTripBookingDTO, TripBooking>();
        CreateMap<UpdateTripBookingDTO, TripBooking>();
    }
}
