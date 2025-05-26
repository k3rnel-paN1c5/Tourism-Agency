using System.ComponentModel.DataAnnotations;


namespace Application.DTOs.Car
{
    public class CreateCarDTO
    {

        [Required(ErrorMessage = "Model cannot be empty")]
        public string?  Model { get; set; }

        [Required(ErrorMessage = "Seat count is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Must be a positive integer")]
        public int Seats { get; set; }


        [Required(ErrorMessage = "Color is cannot be empty")]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "Image is required")]
        public string? Image { get; set; }

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
