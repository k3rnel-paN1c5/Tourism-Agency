using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Represents a specific plan or itinerary for a trip.
/// </summary>
public partial class TripPlan
{
    /// <summary>
    /// Unique identifier for the trip plan.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Foreign key for the general trip this plan belongs to.
    /// </summary>
    [Required]
    [Column("tripId")]
    [ForeignKey("Trip")]
    public int TripId { get; set; }

    /// <summary>
    /// Foreign key for the region covered by this trip plan.
    /// </summary>
    [Required]
    [Column("regionId")]
    [ForeignKey("Region")]
    public int RegionId { get; set; }

    /// <summary>
    /// Start date of the trip plan.
    /// </summary>
    [Required]
    [Column("startDate", TypeName = "datetime2(7)")]
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End date of the trip plan.
    /// </summary>
    [Required]
    [Column("endDate", TypeName = "datetime2(7)")]
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Duration of the trip plan.
    /// Stored as nvarchar in the database.
    /// </summary>
    [Column("duration", TypeName = "nvarchar(50)")]
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// A description of services included in the trip plan.
    /// </summary>
    [Column("includedServices", TypeName = "nvarchar(200)")]
    public string? IncludedServices { get; set; }

    /// <summary>
    /// A description of the stops included in the trip plan.
    /// </summary>
    [Column("stops", TypeName = "nvarchar(200)")]
    public string? Stops { get; set; }

    /// <summary>
    /// A description of the meals plan for the trip.
    /// </summary>
    [Column("mealsPlan", TypeName = "nvarchar(200)")]
    public string? MealsPlan { get; set; }

    /// <summary>
    /// A description of hotel stays included in the trip plan.
    /// </summary>
    [Column("hotelStays", TypeName = "nvarchar(200)")]
    public string? HotelStays { get; set; }


    // Navigation Properties

    /// <summary>
    /// Navigation property to the Region associated with this trip plan.
    /// </summary>
    public Region? Region { get; set; }

    /// <summary>
    /// Navigation property to the main Trip entity this plan belongs to.
    /// </summary>
    public Trip? Trip { get; set; }

    /// <summary>
    /// Collection of trip bookings associated with this trip plan.
    /// </summary>
    public ICollection<TripBooking>? Bookings { get; set; }

    /// <summary>
    /// Collection of cars associated with this trip plan.
    /// </summary>
    public ICollection<TripPlanCar>? PlanCars { get; set; }

}