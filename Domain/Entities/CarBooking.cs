using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Represents the specific details of a car booking.
/// </summary>
public partial class CarBooking
{
    /// <summary>
    /// Foreign key for the associated general booking.
    /// This also serves as the primary key for CarBooking.
    /// </summary>
    [Key]
    [Column("bookingId")]
    [ForeignKey("Booking")]
    public int BookingId { get; set; }

    /// <summary>
    /// Foreign key for the car being booked.
    /// </summary>
    [Required]
    [Column("carId")]
    [ForeignKey("Car")]
    public int CarId { get; set; }

    /// <summary>
    /// Pick-up location for the car.
    /// </summary>
    [Required]
    [Column("pickUpLocation", TypeName = "nvarchar(50)")]
    public string? PickUpLocation { get; set; }

    /// <summary>
    /// Drop-off location for the car.
    /// </summary>
    [Required]
    [Column("dropOffLocation", TypeName = "nvarchar(50)")]
    public string? DropOffLocation { get; set; }

    /// <summary>
    /// A value indicating whether the car is booked with a driver.
    /// </summary>
    [Required]
    [Column("withDriver")]
    public bool WithDriver { get; set; }


    // Navigation Properties

    /// <summary>
    /// Navigation property to the Car being booked.
    /// </summary>
    public Car? Car { get; set; }

    /// <summary>
    /// Navigation property to the main Booking entity.
    /// </summary>
    public Booking? Booking { get; set; }

    /// <summary>
    /// Collection of image shots related to the car booking (e.g., condition before/after).
    /// </summary>
    public ICollection<ImageShot>? ImageShots { get; set; }

}
