using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Represents a category for cars (e.g., Sedan, SUV, Luxury).
/// </summary>
public partial class Category
{
    /// <summary>
    /// Unique identifier for the category.
    /// </summary>
    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    /// <summary>
    /// Title or name of the car category.
    /// </summary>
    [Required]
    [Column("title", TypeName = "nvarchar(50)")]
    public string? Title { get; set; }


    // Navigation Properties

    /// <summary>
    /// Collection of cars belonging to this category.
    /// </summary>
    public ICollection<Car>? Cars { get; set; }
}