using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Tag
{
   public class CreateTagDTO
    {
        [Required(ErrorMessage = "Tag name is required")]
         public string Name { get; set; } = string.Empty;
    }
}
