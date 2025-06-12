using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Tag
{
    public class GetTagDTO
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}

