using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ImageShot
{
    public class UpdateImageShotDTO
    {
        [Required(ErrorMessage = "Image shot ID is required")]
        public int Id { get; set; }

        public string? Image { get; set; }

        public bool? IsPickupPhoto { get; set; }
    }
}
