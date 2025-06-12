using Application.DTOs.PostType;

namespace Application.IServices.UseCases;

/// <summary>
/// Defines the contract for PostType-related business logic operations.
/// </summary>
public interface IPostTypeService
{

    /// <summary>
    /// Creates a new post type asynchronously.
    /// </summary>
    /// <param name="postTypeDto">The DTO containing information for the new post type.</param>
    /// <returns>A <see cref="PostTypeDto"/> representing the newly created post type.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="postTypeDto"/> is null.</exception>
    /// <exception cref="ValidationException">Thrown if the post type title or description is empty.</exception>
    public Task<PostTypeDto> CreatePostTypeAsync(CreatePostTypeDTO postTypeDto);

    /// <summary>
    /// Retrieves a post type by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the post type.</param>
    /// <returns>A <see cref="PostTypeDto"/> representing the found post type.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the post type with the specified ID is not found.</exception>
    public Task<PostTypeDto> GetPostTypeByIdAsync(int id);

    /// <summary>
    /// Retrieves all post types asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="PostTypeDto"/> representing all available post types.</returns>
    public Task<IEnumerable<PostTypeDto>> GetAllPostTypeAsync();

    /// <summary>
    /// Updates an existing post type asynchronously.
    /// </summary>
    /// <param name="postTypeDto">The DTO containing updated information for the post type.</param>
    /// <returns>A <see cref="PostTypeDto"/> representing the updated post type.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="postTypeDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the post type with the specified ID is not found.</exception>
    public Task<PostTypeDto> UpdatePostTypeAsync(UpdatePostTypeDTO postTypeDto);

    /// <summary>
    /// Deletes a post type asynchronously.
    /// If the post type has associated posts, they are reassigned to the default post type ('Normal') before deletion.
    /// </summary>
    /// <param name="postTypeId">The unique identifier of the post type to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the post type with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if there are associated posts and the default post type ('Normal') does not exist.
    /// </exception>
    public Task DeletePostTypeAsync(int postTypeId);

    /// <summary>
    /// Checks whether a post type exists asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the post type.</param>
    /// <returns>
    /// A boolean value indicating whether the post type exists.
    /// Returns <c>true</c> if the post type exists, otherwise <c>false</c>.
    /// </returns>
    public Task<bool> CheckIfPostTypeExistsAsync(int id);


}



