using Domain.Entities;
using Application.IServices.UseCases;
using Application.DTOs.Category;
using Domain.IRepositories;
using AutoMapper;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;


namespace Application.Services.UseCases
{
    /// <summary>
    /// Provides business logic for Category management.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category, int> _categoryRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryService"/> class.
        /// </summary>
        /// <param name="categoryRepo">The repository for Category entities.</param>
        /// <param name="mapper">The AutoMapper instance for DTO-entity mapping.</param>
        /// <param name="logger">The logger for this service.</param>
        public CategoryService(IRepository<Category, int> categoryRepo, IMapper mapper, ILogger<CategoryService> logger)
        {
            _categoryRepo = categoryRepo ?? throw new ArgumentNullException(nameof(categoryRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public async Task<GetCategoryDTO> CreateCategoryAsync(CreateCategoryDTO dto)
        {
            _logger.LogInformation("Attempting to create category: {CategoryTitle}", dto.Title);
            try
            {
                if(dto is null)
                {
                    _logger.LogError("CreateCategoryAsync called with null DTO.");
                    throw new ArgumentNullException(nameof(dto), "Category creation DTO cannot be null.");
                }
                var existingCategory = await _categoryRepo.GetByPredicateAsync(c => dto.Title!.Equals(c.Title)).ConfigureAwait(false);
                if (existingCategory is not null)
                {
                    _logger.LogWarning("Category with title '{CategoryTitle}' already exists. Creation failed.", dto.Title);
                    throw new InvalidOperationException($"Category with title '{dto.Title}' already exists.");
                }
                Category category = _mapper.Map<Category>(dto);

                await _categoryRepo.AddAsync(category).ConfigureAwait(false); ;
                await _categoryRepo.SaveAsync().ConfigureAwait(false);

                _logger.LogInformation("Category '{CategoryTitle}' (ID: {CategoryId}) created successfully.", category.Title, category.Id);
                return _mapper.Map<GetCategoryDTO>(category);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while adding category with Title: {CategoryTitle}", dto.Title);
                throw;
            }

        }

        /// <inheritdoc />
        public async Task DeleteCategoryAsync(int id)
        {
            _logger.LogInformation("Attempting to delete category with ID: {CategoryId}", id);
            try
            {
                var category = await _categoryRepo.GetByIdAsync(id).ConfigureAwait(false);
                if (category is null)
                {
                    _logger.LogWarning("Category with ID: {CategoryId} not found for deletion.", id);
                    throw new KeyNotFoundException($"Category with ID '{id}' not found.");
                }
                if (category.Cars is not null && category.Cars.Count != 0)
                {
                    _logger.LogWarning("Can't delete Category with ID: {CategoryId} This Category is a foriegn key in Cars.", id);
                    throw new DbUpdateException($"Category with ID '{id}' is used by a car or more.");
                }
                _categoryRepo.Delete(category);
                await _categoryRepo.SaveAsync().ConfigureAwait(false);

                _logger.LogInformation("Category with ID: {CategoryId} deleted successfully.", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting category with ID {Id}.", id);
                throw;
            }
        
        }

        /// <inheritdoc />
        public async Task<IEnumerable<GetCategoryDTO>> GetAllCategoriesAsync()
        {
            _logger.LogInformation("Attempting to retrieve all categories.");
            try
            {
                var categories = await _categoryRepo.GetAllAsync().ConfigureAwait(false);
                _logger.LogInformation("Retrieved all categories successfully");
                return _mapper.Map<IEnumerable<GetCategoryDTO>>(categories);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving all categories");
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<GetCategoryDTO> GetCategoryByIdAsync(int id)
        {
            _logger.LogInformation("Attempting to retrieve category with ID: {CategoryId}", id);
            try
            {
                var category = await _categoryRepo.GetByIdAsync(id).ConfigureAwait(false);

                if (category is null)
                {
                    _logger.LogWarning("Category with ID: {CategoryId} not found.", id);
                    throw new KeyNotFoundException($"Category with ID '{id}' not found.");
                }

                _logger.LogInformation("Category with ID: {CategoryId} retrieved successfully.", id);
                return _mapper.Map<GetCategoryDTO>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving category with id: {CategoryID}.", id);
                throw;
            }

        }

        /// <inheritdoc />
        public async Task UpdateCategoryAsync(UpdateCategoryDTO dto)
        {
            _logger.LogInformation("Attempting to update category with ID: {CategoryId}", dto.Id);
            if (dto is null)
            {
                _logger.LogWarning("UpdateCategoryAsync called with null DTO.");
                throw new ArgumentNullException(nameof(dto), "Category update DTO cannot be null.");
            }
            try
            {
                var category = await _categoryRepo.GetByIdAsync(dto.Id).ConfigureAwait(false);
                if (category is null)
                {
                    _logger.LogWarning("Category with ID: {CategoryId} not found for update.", dto.Id);
                    throw new KeyNotFoundException($"Category with ID '{dto.Id}' not found.");
                }

                // if title is updated , we check for uniqueness
                if (!category.Title!.Equals(dto.Title, StringComparison.OrdinalIgnoreCase))
                {
                    var categoryWithSameTitle = await _categoryRepo.GetByPredicateAsync(c => c.Title!.Equals(dto.Title) && c.Id != dto.Id).ConfigureAwait(false);
                    if (categoryWithSameTitle is not null)
                    {
                        _logger.LogWarning("Another category with title '{CategoryTitle}' already exists. Update failed for ID: {CategoryId}.", dto.Title, dto.Id);
                        throw new InvalidOperationException($"Another category with title '{dto.Title}' already exists.");
                    }
                }
                _mapper.Map(dto, category);
                _categoryRepo.Update(category);
                await _categoryRepo.SaveAsync().ConfigureAwait(false);

                _logger.LogInformation("Category '{Title}' updated successfully.", category.Title);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category with ID {Id}.", dto.Id);
                throw;
            }

        }
    }
}
