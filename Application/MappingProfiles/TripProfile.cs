using System;
using Application.DTOs.Trip;
using AutoMapper;
using Domain.Entities;

namespace Application.MappingProfiles;

public class TripProfile : Profile
{
    public TripProfile()
    {
        CreateMap<Trip, GetTripDTO>();
        CreateMap<CreateTripDTO, Trip>();
        CreateMap<UpdateTripDTO, Trip>();
    }
}