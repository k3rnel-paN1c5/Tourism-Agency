using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Category
{
   public class CreateCategoryDTO
    {
        [Required(ErrorMessage = "Name cannot be empty")]
        [StringLength(100)]
        public string? Title;

    }
}
