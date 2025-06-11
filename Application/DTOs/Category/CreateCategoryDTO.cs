using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Title cannot be empty")]
        [StringLength(100, ErrorMessage = "Category title cannot exceed 100 characters.")]
        [Display(Name = "Category Title")]
        public string? Title { get; set; }

    }
}
