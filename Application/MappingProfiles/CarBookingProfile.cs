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
    /// <summary>
    /// AutoMapper profile for mapping between CarBooking entities and CarBooking DTOs.
    /// Defines mappings for retrieving, creating, and updating car booking information.
    /// </summary>
    public class CarBookingProfile:Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CarBookingProfile"/> class.
        /// Configures the mapping rules for CarBooking related objects.
        /// </summary>
        public CarBookingProfile() {

            // Map from CarBooking Entity to GetCarBookingDTO
            // This mapping is used when retrieving car booking data to be sent to the client.
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
            .ReverseMap();// Also allows mapping from GetCarBookingDTO back to CarBooking entity


            // Map from CreateCarBookingDTO to CarBooking Entity
            // This mapping is used when creating a new car booking from client-provided data.
            CreateMap<CreateCarBookingDTO, CarBooking>()
                 .ForMember(dest => dest.BookingId, opt => opt.Ignore())
                .ForMember(dest => dest.CarId, opt => opt.MapFrom(src => src.CarId))
                .ForMember(dest => dest.WithDriver, opt => opt.MapFrom(src => src.WithDriver))
                .ForMember(dest => dest.PickUpLocation, opt => opt.MapFrom(src => src.PickUpLocation))
                .ForMember(dest => dest.DropOffLocation, opt => opt.MapFrom(src => src.DropOffLocation))
                .ForMember(dest => dest.Booking, opt => opt.Ignore())
                .ForMember(dest =>dest.Car, opt=> opt.Ignore())
                .ForMember(dest =>dest.ImageShots, opt=>opt.Ignore());


            // Map from UpdateCarBookingDTO to CarBooking Entity
            // This mapping is used when updating an existing car booking with client-provided data.
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
