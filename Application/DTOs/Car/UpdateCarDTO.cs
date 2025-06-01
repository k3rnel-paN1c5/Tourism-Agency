using System.ComponentModel.DataAnnotations;


namespace Application.DTOs.Car
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for updating an existing car entry.
    /// Used to modify the details of a car.
    /// </summary>
    public class UpdateCarDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the car to be updated. This field is required.
        /// </summary>
        [Required(ErrorMessage = "Car ID Is Required")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the new ID of the category this car belongs to. This field is required.
        /// </summary>
        [Required(ErrorMessage = "This Feild Is Required")]
        public int CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the new number of seats in the car.
        /// Must be a positive integer.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Must be a positive integer")]
        public int Seats { get; set; }

        /// <summary>
        /// Gets or sets the new price per hour for renting the car.
        /// Must be a positive decimal.
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Invalid price per hour")]
        public decimal Pph { get; set; }

        /// <summary>
        /// Gets or sets the new price per day for renting the car.
        /// Must be a positive decimal.
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Invalid price per day")]
        public decimal Ppd { get; set; }

        /// <summary>
        /// Gets or sets the new maximum baggage weight allowed for the car.
        /// Must be a positive decimal.
        /// </summary>
        [Range(0.01, 9999.99, ErrorMessage = "Invalid Maximum baggage weight")]
        public decimal Mbw { get; set; }

        /// <summary>
        /// Gets or sets the new color of the car.
        /// </summary>
        public string? Color { get; set; }

        /// <summary>
        /// Gets or sets the new image URL or path for the car.
        /// </summary>
        public string? Image { get; set; }

        /// <summary>
        /// Gets or sets the new model name of the car.
        /// </summary>
        public string? Model { get; set; }
    }
}
