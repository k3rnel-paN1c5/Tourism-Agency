using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
/// <summary>
/// Represents a many-to-many relationship between a trip plan and a car, including the price for that car in the specific plan.
/// </summary>
public partial class TripPlanCar
{
    /// <summary>
    /// Unique identifier for the trip plan car entry.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Foreign key for the associated trip plan.
    /// </summary>
    [Required]
    [Column("tripPlanId")]
    [ForeignKey("TripPlan")]
    public int TripPlanId { get; set; }

    /// <summary>
    /// Foreign key for the associated car.
    /// </summary>
    [Required]
    [Column("carId")]
    [ForeignKey("Car")]
    public int CarId { get; set; }


    /// <summary>
    /// Navigation property to the Car.
    /// </summary>
    public Car? Car { get; set; }

    /// <summary>
    /// Navigation property to the TripPlan.
    /// </summary>
    public TripPlan? TripPlan { get; set; }
}