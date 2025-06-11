using System.ComponentModel.DataAnnotations;


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
        [Display(Name = "Category ID")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title or name of the category.
        /// </summary>
        [Display(Name = "Title")]
        public string? Title { get; set; }
        
    }
}
