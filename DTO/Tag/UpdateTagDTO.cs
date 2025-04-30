using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Tag
{
   public class UpdateTagDTO
    {
        [Required(ErrorMessage = "Tag ID is required")]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
