using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

/// <summary>
/// Represents a category or type for blog posts.
/// </summary>
public partial class PostType
{
    /// <summary>
    /// Unique identifier for the post type.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Title or name of the post type.
    /// </summary>
    [Required]
    [Column("title", TypeName = "nvarchar(50)")]
    public string? Title { get; set; }

    /// <summary>
    /// A description of the post type.
    /// </summary>
    [Column("description", TypeName = "nvarchar(200)")]
    public string? Description { get; set; }


    // Navigation Properties

    /// <summary>
    /// Collection of posts belonging to this type.
    /// </summary>
    public ICollection<Post>? Posts { get; set; }

}

