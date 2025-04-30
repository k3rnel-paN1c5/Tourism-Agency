using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Car
{
   public class UpdateCarDTO
    {
        [Required(ErrorMessage = "Car ID is required")]
        public int Id { get; set; }
        public string? Model { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Must be a positive integer")]
        public int? Seats { get; set; }

        public string? Color { get; set; }

        public string? Image { get; set; } // Optional update

        [Range(0.01, double.MaxValue, ErrorMessage = "Invalid price per hour")]
        public decimal? Pph { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Invalid price per day")] 
        public decimal? Ppd { get; set; }

        [Range(0.01, 9999.99, ErrorMessage = "Invalid Maximum baggage weight")]
        public decimal? Mbw { get; set; }

        public int? CategoryId { get; set; }
    }
}
