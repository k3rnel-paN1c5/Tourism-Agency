using System;
using AutoMapper;
using Domain.Entities;
using Application.DTOs.TripPlan;

namespace Application.MappingProfiles;

public class TripPlanProfile : Profile
{
    public TripPlanProfile(){
        CreateMap<TripPlan, GetTripPlanDTO>();
        CreateMap<CreateTripPlanDTO, TripPlan>();
        CreateMap<UpdateTripPlanDTO, TripPlan>();
    }
}
