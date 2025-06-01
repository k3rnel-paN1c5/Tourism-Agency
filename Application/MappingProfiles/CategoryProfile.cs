using AutoMapper;
using System;
using Domain.Entities;
using Application.DTOs.Category;
namespace Application.MappingProfiles
{
    /// <summary>
    /// AutoMapper profile for mapping between Category entities and Category DTOs.
    /// Defines mappings for retrieving, creating, and updating category information.
    /// </summary>
    public class CategoryProfile:Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryProfile"/> class.
        /// Configures the mapping rules for Category related objects.
        /// </summary>
        public CategoryProfile() {
            // Map from Category Entity to GetCategoryDTO
            // This mapping is used when retrieving category data to be sent to the client.
            CreateMap<Category, GetCategoryDTO>();

            // Map from CreateCategoryDTO to Category Entity
            // This mapping is used when creating a new category from client-provided data.
            CreateMap<CreateCategoryDTO, Category>();

            // Map from UpdateCategoryDTO to Category Entity
            // This mapping is used when updating an existing category with client-provided data.
            CreateMap<UpdateCategoryDTO, Category>();
 
        
        }
    }
}
