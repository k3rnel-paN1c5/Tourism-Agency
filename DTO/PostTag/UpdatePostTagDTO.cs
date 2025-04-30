using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.PostTag
{
    public class UpdatePostTagDTO
    {
        [Required(ErrorMessage = "Original Post ID is required")]
        public int OriginalPostId { get; set; }

        [Required(ErrorMessage = "Original Tag ID is required")]
        public int OriginalTagId { get; set; }

        [Required(ErrorMessage = "New Post ID is required")]
        public int NewPostId { get; set; }

        [Required(ErrorMessage = "New Tag ID is required")]
        public int NewTagId { get; set; }
    }
}
