using System;
using Domain.Entities;
using Application.IServices.UseCases;
using Application.DTOs.Category;
using Domain.IRepositories;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Application.Services.UseCases
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category, int> _categoryRepo;
        private readonly IMapper _mapper;
        public CategoryService(IRepository<Category, int> categoryRepo, IMapper mapper){
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }
        public async Task<GetCategoryDTO> CreateCategoryAsync(CreateCategoryDTO dto)
        {
            if(await _categoryRepo.GetByPredicateAsync(c=> dto.Title!.Equals(c.Title)) is not null)
                throw new ValidationException("Category with this title already exists.");
            
            Category category = _mapper.Map<Category>(dto);

            await _categoryRepo.AddAsync(category);
            await _categoryRepo.SaveAsync();

            return _mapper.Map<GetCategoryDTO>(category);

        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id)  ?? throw new Exception("Category not found");
            _categoryRepo.Delete(category);
            await _categoryRepo.SaveAsync();
        }

        public async Task<IEnumerable<GetCategoryDTO>> GetAllCategoriesAsync()
        {
            var categories =await  _categoryRepo.GetAllAsync();

            return _mapper.Map<IEnumerable<GetCategoryDTO>>(categories);
        }

        public async Task<GetCategoryDTO> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepo.GetByIdAsync(id) ?? throw new Exception($"Category {id} not found");
            return _mapper.Map<GetCategoryDTO>(category);

        }

        public async Task UpdateCategoryAsync(UpdateCategoryDTO dto)
        {
            var category = await _categoryRepo.GetByIdAsync(dto.Id) ??throw new Exception($"Category {dto.Id} not found");

            if (await _categoryRepo.GetByPredicateAsync(c => dto.Title!.Equals(c.Title)) is not null)
                throw new ValidationException("Category with this title already exists.");

            _categoryRepo.Update(_mapper.Map<Category>(dto));
            await _categoryRepo.SaveAsync();

        }
    }
}
