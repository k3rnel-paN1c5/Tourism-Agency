using System;
using DataAccess.Entities;
using DTO.CarBooking;
using AutoMapper;
namespace BusinessLogic.MappingProfiles;

public class CarBookingProfile : Profile
{
    public CarBookingProfile()
    {
        // Map ReturnCarBookingDTO to CarBooking
        CreateMap<CarBooking, ReturnCarBookingDTO>();
    }
}
