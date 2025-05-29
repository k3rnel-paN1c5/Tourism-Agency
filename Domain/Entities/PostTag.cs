using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;

/// <summary>
/// Represents a many-to-many relationship between a post and a tag.
/// </summary>
public partial class PostTag
{
    /// <summary>
    /// Foreign key for the associated post.
    /// </summary>
    [ForeignKey("Post")]
    public int PostId { get; set; }

    /// <summary>
    /// Foreign key for the associated tag.
    /// </summary>
    [ForeignKey("Tag")]
    public int TagId { get; set; }


    // Navigation Properties

    /// <summary>
    /// Navigation property to the Post.
    /// </summary>
    public Post? Post { get; set; }

    /// <summary>
    /// Navigation property to the Tag.
    /// </summary>
    public Tag? Tag { get; set; }

}

