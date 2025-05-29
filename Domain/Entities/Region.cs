using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

/// <summary>
/// Represents a geographical region relevant to trip plans.
/// </summary>
public partial class Region
{
    /// <summary>
    /// Unique identifier for the region.
    /// </summary>
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Name of the region.
    /// </summary>
    [Required]
    [Column("name", TypeName = "nvarchar(50)")]
    public string? Name { get; set; }


    // Navigation Properties

    /// <summary>
    /// Collection of trip plans associated with this region.
    /// </summary>
    public ICollection<TripPlan>? Plans { get; set; }


}

