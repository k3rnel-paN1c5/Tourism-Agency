using Application.DTOs.Car;
using AutoMapper;
using Domain.Entities;
namespace Application.MappingProfiles
{
    /// <summary>
    /// AutoMapper profile for mapping between Car entities and Car DTOs.
    /// Defines mappings for retrieving, creating, and updating car information.
    /// </summary>
    public class CarProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CarProfile"/> class.
        /// Configures the mapping rules for Car related objects.
        /// </summary>
        public CarProfile()
        {

            // Map from Car Entity to GetCarDTO
            // This mapping is used when retrieving car data to be sent to the client.
            CreateMap<Car, GetCarDTO>();

            // Map from CreateCarDTO to Car Entity
            // This mapping is used when creating a new car from client-provided data.
            CreateMap<CreateCarDTO, Car>();

            // Map from UpdateCarDTO to Car Entity
            // This mapping is used when updating an existing car with client-provided data.
            CreateMap<UpdateCarDTO, Car>();
        }
    }
}
