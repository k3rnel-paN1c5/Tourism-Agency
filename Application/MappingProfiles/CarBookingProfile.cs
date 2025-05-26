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

            CreateMap<CarBooking, GetCarBookingDTO>();
            CreateMap<CreateCarBookingDTO, CarBooking>();
            CreateMap<UpdateCarBookingDTO, CarBooking>();

        
        }

    }
}
