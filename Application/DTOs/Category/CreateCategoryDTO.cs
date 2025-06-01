using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Category
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for creating a new category.
    /// Used to add new category entries (e.g., for cars, trips) to the system.
    /// </summary>
    public class CreateCategoryDTO
    {
        /// <summary>
        /// Gets or sets the title or name of the category.
        /// This field is required and has a maximum length of 100 characters.
        /// </summary>
        [Required(ErrorMessage = "Name cannot be empty")]
        [StringLength(100)]
        [Display(Name = "Category Name")]
        public string? Title { get; set; }

    }
}
