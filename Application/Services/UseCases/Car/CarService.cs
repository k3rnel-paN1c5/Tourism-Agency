using System;
using Domain.Entities;
using Application.DTOs.Car;
using Application.IServices.UseCases.Car;
using AutoMapper;
using Domain.IRepositories;
using Application.IServices.UseCases.Category;



namespace Application.Services.UseCases
{
    public class CarService: ICarService
    {
        private readonly IRepository<Car, int> _repo;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public CarService(IMapper mapper ,IRepository<Car, int> repo , ICategoryService categoryService)
        {

            _mapper = mapper;
            _repo = repo;
            _categoryService = categoryService;
        }

        public async Task<GetCarDTO> CreateCarAsync(CreateCarDTO dto)
        {
            var category = _categoryService.GetCategoryByIdAsync(dto.CategoryId)
                ?? throw new Exception($"Category {dto.CategoryId} was not found");

            var car = _mapper.Map<Car>(dto);
            
            car.Category=_mapper.Map<Category>(category); //remove later if found unnecessary 
            

            await _repo.AddAsync(car);
            await _repo.SaveAsync();

            return _mapper.Map<GetCarDTO>(car);

        }

        public async Task UpdateCarAsync(UpdateCarDTO dto)
        {
            var category = _categoryService.GetCategoryByIdAsync(dto.CategoryId)
                ?? throw new Exception($"Category {dto.CategoryId} was not found");

            var car = _mapper.Map<Car>(dto);
            car.Category = _mapper.Map<Category>(category);
             _repo.Update(car);
            await _repo.SaveAsync();


        }


        public async Task DeleteCarAsync(int id)
        {
            _repo.DeleteByIdAsync(id);
            await _repo.SaveAsync();
        }

        public async Task<IEnumerable<GetCarDTO>> GetAllTripAsync()
        {
            var cars= await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<GetCarDTO>>(cars);

        }

        public async Task<GetCarDTO> GetCarByIdAsync(int id)
        {
            var car= await _repo.GetByIdAsync(id)
                ?? throw new Exception($"Category {id} was not found");
            return _mapper.Map<GetCarDTO>(car);
        }

        public async Task<IEnumerable<GetCarDTO>> GetCarByCategoryAsync(int categoryId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GetCarDTO>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate)
        {
            throw new NotImplementedException();
        }

        
    }
}
