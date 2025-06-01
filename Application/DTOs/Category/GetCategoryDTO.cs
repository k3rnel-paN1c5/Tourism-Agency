using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Category
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for retrieving category information.
    /// Used to display category details.
    /// </summary>
    public class GetCategoryDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the category.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title or name of the category.
        /// </summary>
        public string? Title { get; set; }
        
    }
}
