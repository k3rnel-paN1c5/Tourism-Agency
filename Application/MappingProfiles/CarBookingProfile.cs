using AutoMapper;
using Domain.Entities;
using Application.DTOs.CarBooking;

namespace Application.MappingProfiles{
    public class CarBookingProfile : Profile
    {
        public CarBookingProfile()
        {
            // Map ReturnCarBookingDTO to CarBooking
            CreateMap<CarBooking, ReturnCarBookingDTO>();
        }
    }   
}
    
