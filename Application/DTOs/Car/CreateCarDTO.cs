using System.ComponentModel.DataAnnotations;


namespace Application.DTOs.Car
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for creating a new car entry.
    /// Used to add new car details to the system.
    /// </summary>
    public class CreateCarDTO
    {
        /// <summary>
        /// Gets or sets the model name of the car. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Model cannot be empty")]
        public string?  Model { get; set; }

        /// <summary>
        /// Gets or sets the number of seats in the car.
        /// Must be a positive integer. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Seat count is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Must be a positive integer")]
        public int Seats { get; set; }

        /// <summary>
        /// Gets or sets the color of the car. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Color is cannot be empty")]
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the image URL or path for the car. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Image is required")]
        public string? Image { get; set; }

        /// <summary>
        /// Gets or sets the price per hour for renting the car.
        /// Must be a positive decimal. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Price per hour is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Invalid price per hour")]
        public decimal Pph { get; set; }

        /// <summary>
        /// Gets or sets the price per day for renting the car.
        /// Must be a positive decimal. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Price per day is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Invalid price per day")] public decimal Ppd { get; set; }

        /// <summary>
        /// Gets or sets the maximum baggage weight allowed for the car.
        /// Must be a positive decimal. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Maximum baggage weight is required")]
        [Range(0.01, 9999.99, ErrorMessage = "Invalid Maximum baggage weight")]
        public decimal Mbw { get; set; }

        /// <summary>
        /// Gets or sets the ID of the category this car belongs to. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }


    }
}
