using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Represents the specific details of a trip booking.
/// </summary>
public partial class TripBooking
{
    /// <summary>
    /// Foreign key for the associated general booking.
    /// This also serves as the primary key for TripBooking.
    /// </summary>
    [Key]
    [Column("bookingId")]
    [ForeignKey("Booking")]
    public int BookingId { get; set; }

    /// <summary>
    /// Foreign key for the trip plan being booked.
    /// </summary>
    [Required]
    [Column("tripPlanId")]
    [ForeignKey("TripPlan")]
    public int TripPlanId { get; set; }

    /// <summary>
    /// A value indicating whether the trip is booked with a guide.
    /// </summary>
    [Required]
    [Column("withGuide")]
    public bool WithGuide { get; set; }


    // Navigation Properties

    /// <summary>
    /// Navigation property to the TripPlan being booked.
    /// </summary>
    public TripPlan? TripPlan { get; set; }

    /// <summary>
    /// Navigation property to the main Booking entity.
    /// </summary>
    public Booking? Booking { get; set; }

}

