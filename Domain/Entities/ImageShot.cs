using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Represents an image shot related to a car booking, typically for documenting car condition.
/// </summary
public partial class ImageShot
{
    /// <summary>
    /// Unique identifier for the image shot.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// File path or URL to the image.
    /// </summary>
    [Required]
    [Column("path", TypeName = "nvarchar(50)")]
    public string? Path { get; set; }

    /// <summary>
    /// A boolean value indicating the type of image (e.g., true for 'before', false for 'after').
    /// </summary>
    [Required]
    [Column("type")]
    public bool Type { get; set; }

    /// <summary>
    /// Foreign key referencing the associated car booking.
    /// </summary>
    [Required]
    [Column("carBookingId")]
    [ForeignKey("CarBooking")]
    public int CarBookingId { get; set; }


    // Navigation Properties

    /// <summary>
    /// Navigation property to the CarBooking this image shot belongs to.
    /// </summary>
    public CarBooking? CarBooking { get; set; }


}

