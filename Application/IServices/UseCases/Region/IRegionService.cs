using System;
using Application.DTOs.Region;
namespace Application.IServices.UseCases;

/// <summary>
/// Defines the contract for Region-related business logic operations.
/// </summary>
public interface IRegionService
{
    /// <summary>
    /// Creates a new region asynchronously.
    /// </summary>
    /// <param name="createRegionDto">The DTO containing information for the new region.</param>
    /// <returns>A <see cref="GetRegionDTO"/> representing the newly created region.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="createRegionDto"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if a region with the same name already exists.</exception>
    public Task<GetRegionDTO> CreateRegionAsync(CreateRegionDTO createRegionDto);

    /// <summary>
    /// Retrieves a region by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the region.</param>
    /// <returns>A <see cref="GetRegionDTO"/> representing the found region, or null if not found.</returns>
    Task<GetRegionDTO> GetRegionByIdAsync(int id);

    /// <summary>
    /// Retrieves all regions asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="GetRegionDTO"/> representing all regions.</returns>
    Task<IEnumerable<GetRegionDTO>> GetAllRegionsAsync();

    /// <summary>
    /// Updates an existing region asynchronously.
    /// </summary>
    /// <param name="updateRegionDto">The DTO containing updated information for the region.</param>
    /// <returns>A <see cref="GetRegionDTO"/> representing the updated region.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="updateRegionDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the region with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if another region with the updated name already exists.</exception>
    public Task UpdateRegionAsync(UpdateRegionDTO updateRegionDto);

    /// <summary>
    /// Deletes a region by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the region to delete.</param>
    /// <returns>A boolean indicating whether the deletion was successful.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the region with the specified ID is not found.</exception>
    public Task DeleteRegionAsync(int id);
}
