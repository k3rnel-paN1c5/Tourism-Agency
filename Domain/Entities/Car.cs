using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Represents a car available for booking in the tourism agency.
/// </summary>
public partial class Car
{
    /// <summary>
    /// Unique identifier for the car.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Model name of the car.
    /// </summary>
    [Required]
    [Column("model", TypeName = "nvarchar(50)")]
    public string? Model { get; set; }

    /// <summary>
    /// Number of seats available in the car.
    /// </summary>
    [Required]
    [Column("seats")]
    public int Seats { get; set; }

    /// <summary>
    /// Color of the car.
    /// </summary>
    [Required]
    [Column("color", TypeName = "nvarchar(50)")]
    public string? Color { get; set; }

    /// <summary>
    /// URL or path to the car's image.
    /// </summary>
    [Required]
    [Column("image", TypeName = "nvarchar(50)")]
    public string? Image { get; set; }

    /// <summary>
    /// Price per hour for renting the car.
    /// </summary>
    [Required]
    [Column("pph", TypeName = "decimal(16,2)")]
    public decimal Pph { get; set; }

    /// <summary>
    /// Price per day for renting the car.
    /// </summary>
    [Required]
    [Column("ppd", TypeName = "decimal(16,2)")]
    public decimal Ppd { get; set; }

    /// <summary>
    /// Max baggage weight of the car
    /// </summary>
    [Required]
    [Column("mbw", TypeName = "decimal(16,2)")]
    public decimal Mbw { get; set; }

    /// <summary>
    /// Foreign key for the category the car belongs to.
    /// </summary>
    [Column("categoryId")]
    [ForeignKey("Category")]
    public int CategoryId { get; set; }


    // Navigation Properties

    /// <summary>
    /// Navigation property to the Category of the car.
    /// </summary>
    public Category? Category { get; set; }

    /// <summary>
    ///Collection of car bookings associated with this car.
    /// </summary>
    public ICollection<CarBooking>? CarBookings { get; set; }

    /// <summary>
    /// Collection of trip plans that include this car.
    /// </summary>
    public ICollection<TripPlanCar>? TripPlanCars { get; set; }
}

