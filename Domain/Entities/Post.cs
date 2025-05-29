using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities;
/// <summary>
/// Represents a blog post or article.
/// </summary>
public partial class Post
{
    /// <summary>
    /// Unique identifier for the post.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Title of the post.
    /// </summary>
    [Required]
    [Column("title", TypeName = "nvarchar(50)")]
    public string? Title { get; set; }

    /// <summary>
    /// Main content body of the post.
    /// </summary>
    [Required]
    [Column("body", TypeName = "nvarchar(500)")]
    public string? Body { get; set; }

    /// <summary>
    /// URL or path to the main image of the post.
    /// </summary>
    [Column("image", TypeName = "nvarchar(100)")]
    public string? Image { get; set; }

    /// <summary>
    /// URL-friendly slug for the post.
    /// </summary>
    [Required]
    [Column("slug", TypeName = "nvarchar(100)")]
    public string? Slug { get; set; }

    /// <summary>
    /// Number of views the post has received.
    /// </summary>
    [Column("views")]
    public int Views { get; set; }

    /// <summary>
    /// Status of the post (e.g., Draft, Published, Archived).
    /// </summary>
    [Column("status")]
    [EnumDataType(typeof(PostStatus))] // Assuming PostStatus enum exists
    public PostStatus Status { get; set; }

    /// <summary>
    /// A brief summary or excerpt of the post.
    /// </summary>
    [Column("summary", TypeName = "nvarchar(200)")]
    public string? Summary { get; set; }

    /// <summary>
    /// Date when the post was published.
    /// </summary>
    [Column("publishDate")]
    public DateTime PublishDate { get; set; }

    /// <summary>
    /// Foreign key for the type of post.
    /// </summary>
    [Column("postTypeId")]
    [ForeignKey("PostType")]
    public int PostTypeId { get; set; }

    /// <summary>
    /// Foreign key for the employee who authored the post.
    /// </summary>
    [Required]
    [Column("employeeId")]
    [ForeignKey("Employee")]
    public string? EmployeeId { get; set; }

    /// <summary>
    /// Navigation property to the Employee who authored this post.
    /// </summary>
    public Employee? Employee { get; set; }

    /// <summary>
    /// Navigation property to the PostType of this post.
    /// </summary>
    public PostType? PostType { get; set; }

    /// <summary>
    /// Collection of tags associated with this post.
    /// </summary>
    public ICollection<PostTag>? PostTags { get; set; }

    /// <summary>
    /// Collection of SEO metadata associated with this post.
    /// </summary>
    public ICollection<SEOMetadata>? SEOMetadata { get; set; }

}

