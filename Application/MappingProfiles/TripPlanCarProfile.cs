using System;
using AutoMapper;
using Domain.Entities;
using Application.DTOs.TripPlanCar;

namespace Application.MappingProfiles;

public class TripPlanCarProfile : Profile
{
    public TripPlanCarProfile(){
        CreateMap<TripPlanCar, GetTripPlanCarDTO>();
        CreateMap<CreateTripPlanCarDTO, TripPlanCar>();
        CreateMap<UpdateTripPlanCarDTO, TripPlanCar>();
    }
}
