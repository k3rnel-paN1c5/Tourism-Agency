using System;
using Application.DTOs.SEOMetaData;

namespace Application.IServices.UseCases.SEOMetaData;
/// <summary>
/// Defines the contract for SEO MetaData-related business logic operations.
/// </summary>
public interface ISEOMetaDataService
{
     /// <summary>
    /// Creates a new SEO metadata etity asynchronously.
    /// </summary>
    /// <param name="createSEOMetaDataDto">The DTO containing information for the new SEO metadata.</param>
    /// <returns>A <see cref="GetSEOMetaDataDTO"/> representing the newly created SEO metadata.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="createSEOMetaDataDto"/> is null.</exception>
    public Task<GetSEOMetaDataDTO> CreateSEOMetaDataAsync(CreateSEOMetaDataDTO createSEOMetaDataDto);

    /// <summary>
    /// Retrieves an SEO metadata by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the SEO metadata.</param>
    /// <returns>A <see cref="GetSEOMetaDataDTO"/> representing the found SEO metadata entity, or null if not found.</returns>
     /// <exception cref="KeyNotFoundException">Thrown if the SEO metadata entity with the specified ID is not found.</exception>
    Task<GetSEOMetaDataDTO> GetSEOMetaDataByIdAsync(int id);
    /// <summary>
    /// Retrieves the SEO metadata entities that belong to the specified post
    /// </summary>
    /// <param name="id">The unique identifier of the Post.</param>
    /// <returns>A collection of <see cref="GetSEOMetaDataDTO"/> representing all SEO metadata entities.</returns>
     /// <exception cref="KeyNotFoundException">Thrown if the Post with  the specified ID is not found.</exception>
    Task<IEnumerable<GetSEOMetaDataDTO>> GetSEOMetaDataByPostIdAsync(int id);

    /// <summary>
    /// Retrieves all SEO metadata entities asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="GetSEOMetaDataDTO"/> representing all SEO metadata entities.</returns>
    Task<IEnumerable<GetSEOMetaDataDTO>> GetAllSEOMetaDataAsync();

    /// <summary>
    /// Updates an existing SEO metadata entity asynchronously.
    /// </summary>
    /// <param name="UpdateSEOMetaDataDTO">The DTO containing updated information for the SEO metadata entity.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="UpdateSEOMetaDataDTO"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the SEO metadata entity with the specified ID is not found.</exception>
    public Task UpdateSEOMetaDataAsync(UpdateSEOMetaDataDTO UpdateSEOMetaDataDTO);

    /// <summary>
    /// Deletes an SEO metadata entity by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the SEO metadata entity to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the SEO metadata entity with the specified ID is not found.</exception>
    public Task DeleteSEOMetaDataAsync(int id);
}   
