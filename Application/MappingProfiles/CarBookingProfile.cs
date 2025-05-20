using Application.DTOs.CarBooking;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MappingProfiles
{
    public class CarBookingProfile:Profile
    {
        public CarBookingProfile() {

            CreateMap<CarBooking, GetCarBookingDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BookingId))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Booking!.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.Booking!.EndDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Booking!.Status))
                .ForMember(dest => dest.NumOfPassengers, opt => opt.MapFrom(src => src.Booking!.NumOfPassengers))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.Booking!.CustomerId))
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Booking!.EmployeeId))
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.WithDriver, opt => opt.MapFrom(src => src.WithDriver))
                .ForMember(dest => dest.PickUpLocation, opt =>opt.MapFrom(src =>src.PickUpLocation))
                .ForMember(dest => dest.DropOffLocation, opt=>opt.MapFrom(src =>src.DropOffLocation))
            .ReverseMap();



            CreateMap<CreateCarBookingDTO, CarBooking>()
                 .ForMember(dest => dest.BookingId, opt => opt.Ignore())
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.WithDriver, opt => opt.MapFrom(src => src.WithDriver))
                .ForMember(dest => dest.PickUpLocation, opt => opt.MapFrom(src => src.PickUpLocation))
                .ForMember(dest => dest.DropOffLocation, opt => opt.MapFrom(src => src.DropOffLocation))
                .ForMember(dest => dest.Booking, opt => opt.Ignore())
                .ForMember(dest =>dest.Car, opt=> opt.Ignore())
                .ForMember(dest =>dest.ImageShots, opt=>opt.Ignore());

           
           CreateMap<UpdateCarBookingDTO, CarBooking>()
             .ForMember(dest => dest.BookingId, opt => opt.Ignore())
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.WithDriver, opt => opt.MapFrom(src => src.WithDriver))
                .ForMember(dest => dest.PickUpLocation, opt => opt.MapFrom(src => src.PickUpLocation))
                .ForMember(dest => dest.DropOffLocation, opt => opt.MapFrom(src => src.DropOffLocation))
                .ForMember(dest => dest.Booking, opt => opt.Ignore())
                .ForMember(dest => dest.Car, opt => opt.Ignore())
                .ForMember(dest => dest.ImageShots, opt => opt.Ignore());

        }

    }
}
