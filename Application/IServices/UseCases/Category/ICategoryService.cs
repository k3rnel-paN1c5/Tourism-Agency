using Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IServices.UseCases
{
    public interface ICategoryService
    {
        public Task<GetCategoryDTO> CreateCategoryAsync(CreateCategoryDTO dto);
        public Task UpdateCategoryAsync(UpdateCategoryDTO dto);
        public Task DeleteCategoryAsync(int id);
        Task<IEnumerable<GetCategoryDTO>> GetAllCategoriesAsync();
        Task <GetCategoryDTO> GetCategoryByIdAsync(int id);
        

    }
}
