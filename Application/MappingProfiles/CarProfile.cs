using Application.DTOs.Car;
using AutoMapper;
using Domain.Entities;
namespace Application.MappingProfiles
{
    public class CarProfile : Profile
    {
        public CarProfile() { 
        CreateMap<Car, GetCarDTO>();
        CreateMap<CreateCarDTO, Car>();
        CreateMap<UpdateCarDTO, Car>();}
    }
}
