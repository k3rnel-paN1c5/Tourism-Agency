using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities;
/// <summary>
/// Represents SEO (Search Engine Optimization) metadata for a post.
/// </summary>
public partial class SEOMetadata
{
    /// <summary>
    /// Unique identifier for the SEO metadata.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// URL slug for which this metadata applies.
    /// </summary>
    [Required]
    [Column("urlSlug", TypeName = "nvarchar(100)")]
    public string UrlSlug { get; set; } = string.Empty;

    /// <summary>
    /// Meta title for the web page.
    /// </summary>
    [Required]
    [Column("metaTitle", TypeName = "nvarchar(50)")]
    public string MetaTitle { get; set; } = string.Empty;

    /// <summary>
    /// Meta description for the web page.
    /// </summary>
    [Column("metaDescription", TypeName = "nvarchar(200)")]
    public string MetaDescription { get; set; } = string.Empty;

    /// <summary>
    /// Meta keywords for the web page.
    /// </summary>
    [Column("metaKeywords", TypeName = "nvarchar(50)")]
    public string MetaKeywords { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key for the associated post.
    /// </summary>
    [Column("postId")]
    [ForeignKey("Post")]
    public int PostId { get; set; }


    // Navigation Properties
    /// <summary>
    /// Navigation property to the Post this SEO metadata belongs to.
    /// </summary>
    public Post? Post { get; set; }
}

