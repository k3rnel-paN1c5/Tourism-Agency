using Application.DTOs.Category;


namespace Application.IServices.UseCases
{

    /// <summary>
    /// Defines the contract for Category-related business logic operations.
    /// </summary>
    public interface ICategoryService
    {

        /// <summary>
        /// Creates a new category asynchronously.
        /// </summary>
        /// <param name="dto">The DTO containing information for the new category.</param>
        /// <returns>A <see cref="GetCategoryDTO"/> representing the newly created category.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dto"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if a category with the same name already exists.</exception>
        public Task<GetCategoryDTO> CreateCategoryAsync(CreateCategoryDTO dto);

        /// <summary>
        /// Updates an existing category asynchronously.
        /// </summary>
        /// <param name="dto">The DTO containing updated information for the category.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dto"/> is null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the category with the specified ID is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown if another category with the updated name already exists.</exception>
        public Task UpdateCategoryAsync(UpdateCategoryDTO dto);

        /// <summary>
        /// Deletes a category by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the category to delete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the category with the specified ID is not found.</exception>
        public Task DeleteCategoryAsync(int id);

        /// <summary>
        /// Retrieves all categories asynchronously.
        /// </summary>
        /// <returns>A collection of <see cref="GetCategoryDTO"/> representing all categories.</returns>
        Task<IEnumerable<GetCategoryDTO>> GetAllCategoriesAsync();

        /// <summary>
        /// Retrieves a category by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the category.</param>
        /// <returns>A <see cref="GetCategoryDTO"/> representing the found category.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the category with the specified ID is not found.</exception>
        Task<GetCategoryDTO> GetCategoryByIdAsync(int id);
        

    }
}
