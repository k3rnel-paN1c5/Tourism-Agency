using System.ComponentModel.DataAnnotations;


namespace Application.DTOs.Car
{
    public class UpdateCarDTO
    {
        [Required(ErrorMessage = "Car ID Is Required")]
        public int Id { get; set; }

        [Required(ErrorMessage = "This Feild Is Required")]
        public int CategoryId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Must be a positive integer")]
        public int Seats { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Invalid price per hour")]
        public decimal Pph { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Invalid price per day")]
        public decimal Ppd { get; set; }

        [Range(0.01, 9999.99, ErrorMessage = "Invalid Maximum baggage weight")]
        public decimal Mbw { get; set; }
        public string? Color { get; set; }
        public string? Image { get; set; }
        public string? Model { get; set; }
    }
}
