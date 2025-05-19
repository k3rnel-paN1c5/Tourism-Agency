using System;
using System.Collections.Generic;
using System;
using Application.DTOs.Car;

namespace Application.IServices.UseCases
{
    public interface ICarService
    {
        Task<GetCarDTO> CreateCarAsync(CreateCarDTO dto);
        Task UpdateCarAsync(UpdateCarDTO dto);
        Task DeleteCarAsync(int id);
        Task<IEnumerable<GetCarDTO>> GetAllTripAsync();
        Task<GetCarDTO> GetCarByIdAsync(int id);
        Task<IEnumerable<GetCarDTO>> GetCarsByCategoryAsync(int categoryId);
        
        //add later
        Task<IEnumerable<GetCarDTO>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate);
        

    }
}
