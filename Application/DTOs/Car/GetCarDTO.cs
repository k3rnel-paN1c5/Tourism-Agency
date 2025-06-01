namespace Application.DTOs.Car
{
    /// <summary>
    /// Represents a Data Transfer Object (DTO) for retrieving car information.
    /// Used to display car details.
    /// </summary>
    public class GetCarDTO
    {
        /// <summary>
        /// Gets or sets the unique identifier of the car.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the model name of the car.
        /// </summary>
        public string Model { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of seats in the car.
        /// </summary>
        public int Seats { get; set; }

        /// <summary>
        /// Gets or sets the color of the car.
        /// </summary>
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the image URL or path for the car.
        /// </summary>
        public string Image { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the price per hour for renting the car.
        /// </summary>
        public decimal Pph { get; set; } // Price per hour

        /// <summary>
        /// Gets or sets the price per day for renting the car.
        /// </summary>
        public decimal Ppd { get; set; } // Price per day

        /// <summary>
        /// Gets or sets the maximum baggage weight allowed for the car.
        /// </summary>
        public decimal Mbw { get; set; }// Maximum baggage weight

        /// <summary>
        /// Gets or sets the ID of the category this car belongs to.
        /// </summary>
        public int CategoryId { get; set; }
    }
}
