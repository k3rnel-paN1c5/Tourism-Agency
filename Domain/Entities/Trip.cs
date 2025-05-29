using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Represents a general trip offered by the tourism agency.
/// </summary>
public partial class Trip
{
    /// <summary>
    /// Unique identifier for the trip.
    /// </summary>
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Name of the trip.
    /// </summary>
    [Required]
    [Column("name", TypeName = "nvarchar(50)")]
    public string? Name { get; set; }

    /// <summary>
    /// URL-friendly slug for the trip.
    /// </summary>
    [Required]
    [Column("slug", TypeName = "nvarchar(100)")]
    public string? Slug { get; set; }

    /// <summary>
    /// A value indicating whether the trip is currently available for booking.
    /// </summary>
    [Required]
    [Column("isAvailable")]
    public bool IsAvailable { get; set; }

    /// <summary>
    /// A brief description of the trip.
    /// </summary>
    [Column("description", TypeName = "nvarchar(200)")]
    public string? Description { get; set; }

    /// <summary>
    /// A value indicating whether the trip is private (e.g., custom tours).
    /// </summary>
    [Required]
    [Column("isPrivate")]
    public bool IsPrivate { get; set; }


    // Navigation Properties

    /// <summary>
    /// Collection of trip plans associated with this trip.
    /// </summary>
    public ICollection<TripPlan>? Plans { get; set; }


}