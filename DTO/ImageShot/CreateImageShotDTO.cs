using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ImageShot
{
    public class CreateImageShotDTO
    {
        [Required(ErrorMessage = "Image path is required")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Image type is required")]
        public bool Type { get; set; } 

        [Required(ErrorMessage = "Car booking ID is required")]
        public int CarBookingId { get; set; }



    }
}
