//namespace Tourism-Agency;

using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Tag
{
    public class CreateTagDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}

