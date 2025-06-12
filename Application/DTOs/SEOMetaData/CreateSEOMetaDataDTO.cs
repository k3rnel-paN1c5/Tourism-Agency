using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.SEOMetaData;

/// <summary>
/// Data Transfer Object for creating a new SEO metadata entity.
/// </summary>
public class CreateSEOMetaDataDTO
{
    /// <summary>
    /// URL slug for which this metadata applies.
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "URL slug   cannot exceed 100 characters.")]
    public string? UrlSlug { get; set; }

    /// <summary>
    /// Meta title for the web page.
    /// </summary>
    [Required]
    [StringLength(50, ErrorMessage = "Meta title  cannot exceed 50 characters.")]
    public string? MetaTitle { get; set; }

    /// <summary>
    /// Meta description for the web page.
    /// </summary>
    [StringLength(200, ErrorMessage = "Meta Description  cannot exceed 200 characters.")]
    public string MetaDescription { get; set; } = string.Empty;

    /// <summary>
    /// Meta keywords for the web page.
    /// </summary>
    [StringLength(50, ErrorMessage = "Meta key words  cannot exceed 50 characters.")]
    public string MetaKeywords { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key for the associated post.
    /// </summary>
    public int PostId { get; set; }
}
