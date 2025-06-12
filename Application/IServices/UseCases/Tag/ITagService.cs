using Application.DTOs.Tag;

namespace Application.IServices.UseCases;

/// <summary>
/// Defines the contract for Tag-related business logic operations.
/// </summary>
public interface ITagService
{
    /// <summary>
    /// Creates a new tag asynchronously.
    /// </summary>
    /// <param name="tagDto">The DTO containing information for the new tag.</param>
    /// <returns>A <see cref="GetTagDTO"/> representing the newly created tag.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="tagDto"/> is null.</exception>
    /// <exception cref="ArgumentNullException">Thrown if the tag name is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown if a tag with the same name already exists.</exception>
    public Task<GetTagDTO> CreateTagAsync(CreateTagDTO tagDto);
    
    /// <summary>
    /// Retrieves a tag by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the tag.</param>
    /// <returns>A <see cref="GetTagDTO"/> representing the found tag.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the tag with the specified ID is not found.</exception>
    public Task<GetTagDTO> GetTagByIdAsync(int id);

    /// <summary>
    /// Retrieves all tags asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="GetTagDTO"/> representing all tags.</returns>
    /// <exception cref="Exception">Thrown if an error occurs while retrieving tags.</exception>
    public Task<IEnumerable<GetTagDTO>> GetAllTagsAsync();

    /// <summary>
    /// Retrieves a tag by its name asynchronously.
    /// </summary>
    /// <param name="tagName">The name of the tag to retrieve.</param>
    /// <returns>A <see cref="GetTagDTO"/> representing the found tag.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the tag with the specified name is not found.</exception>
    public Task<GetTagDTO> GetTagByNameAsync(string tagName);

    /// <summary>
    /// Checks whether a tag exists by its name asynchronously.
    /// </summary>
    /// <param name="tagName">The name of the tag to check for existence.</param>
    /// <returns><c>true</c> if the tag exists; otherwise, <c>false</c>.</returns>
    /// <exception cref="Exception">Thrown if an error occurs while checking the tag's existence.</exception>
    public Task<bool> CheckIfTagExistsAsync(string tagName);

    /// <summary>
    /// Updates an existing tag asynchronously.
    /// </summary>
    /// <param name="tagDto">The DTO containing updated information for the tag.</param>
    /// <returns>A <see cref="GetTagDTO"/> representing the updated tag.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="tagDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the tag with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if a tag with the same name already exists.</exception>
    public Task<GetTagDTO> UpdateTagAsync(UpdateTagDTO tagDto);

    /// <summary>
    /// Deletes a tag by its unique identifier asynchronously.
    /// </summary>
    /// <param name="tagId">The unique identifier of the tag to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the tag with the specified ID is not found.</exception>
    /// <remarks>
    /// Deleting a tag will also remove all associated posts due to ON DELETE CASCADE.
    /// </remarks>
    public Task DeleteTagAsync(int tagId);

}


