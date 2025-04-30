using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.PostTag
{
   public class CreatePostTagDTO
    {
        [Required(ErrorMessage = "Post ID is required")]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Tag ID is required")]
        public int TagId { get; set; }
    }
}
