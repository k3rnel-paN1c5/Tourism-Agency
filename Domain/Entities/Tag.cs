using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Represents a tag that can be associated with posts.
/// </summary>
public partial class Tag
{
    /// <summary>
    /// Unique identifier for the tag.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Name of the tag.
    /// </summary>
    [Required]
    [Column("name", TypeName = "nvarchar(50)")]
    public string? Name { get; set; }

    
    // Navigation Properties

    /// <summary>
    /// Collection of post-tag relationships associated with this tag.
    /// </summary>
    public ICollection<PostTag>? PostTags { get; set; }
}

