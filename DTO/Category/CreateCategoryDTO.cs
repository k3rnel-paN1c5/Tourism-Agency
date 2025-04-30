using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DTO.Category
{
   public class CreateCategoryDTO
    {
        [Required(ErrorMessage = "Category title is required")]
        public string Title { get; set; } = string.Empty;
    }
}
