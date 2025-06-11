using System.ComponentModel.DataAnnotations;


namespace Application.DTOs.Category
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for updating an existing category.
    /// Used to modify the details of a category.
    /// </summary>
    public class UpdateCategoryDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the category to be updated. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Specify which category to edit")]
        [Display(Name = "Category ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the new title or name of the category.
        /// This field is required and has a maximum length of 100 characters.
        /// </summary>
        [Required(ErrorMessage = "New category name cannot be empty")]
        [StringLength(100, ErrorMessage = "Category title cannot exceed 100 characters.")]
        [Display(Name="Title")]
        public string? Title { get; set; }
    }
}
