using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Car
{
  public  class CreateCarDTO
    {

        [Required(ErrorMessage = "Model is required")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Seat count is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Must be a positive integer")]
        public int Seats { get; set; }

 
        [Required(ErrorMessage = "Color is required")]
            public string Color { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public string Image { get; set; }

        [Required(ErrorMessage = "Price per hour is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Invalid price per hour")]
        public decimal Pph { get; set; }

        [Required(ErrorMessage = "Price per day is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Invalid price per day")] public decimal Ppd { get; set; }

        [Required(ErrorMessage = "Maximum baggage weight is required")]
        [Range(0.01, 9999.99, ErrorMessage = "Invalid Maximum baggage weight")]
        public decimal Mbw { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

      
    }


}

