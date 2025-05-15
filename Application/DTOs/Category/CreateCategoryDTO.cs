using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Category
{
   public class CreateCategoryDTO
    {
        [Required(ErrorMessage = "Name cannot be empty")]
        [StringLength(100)]
        public string? Title;

    }
}
