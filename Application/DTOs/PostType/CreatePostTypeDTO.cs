using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.PostType
{
    public class CreatePostTypeDTO
    {
        [Required]
        [StringLength(50)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(200)] 
        public string Description { get; set; } = string.Empty;
    }
}

