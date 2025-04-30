using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Category
{
    public class UpdateCategoryDTO
    {
        [Required(ErrorMessage = "Category ID is required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        
        public string Title { get; set; } = string.Empty;
    }
}
