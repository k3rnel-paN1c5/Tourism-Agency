using System;

namespace Application.DTOs.SEOMetaData;
/// <summary>
/// Data Transfer Object for retrieving SEO metadata details.
/// </summary>
public class GetSEOMetaDataDTO
{
    /// <summary>
    /// Unique identifier for the SEO metadata.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// URL slug for which this metadata applies.
    /// </summary>
    public string UrlSlug { get; set; } = string.Empty;

    /// <summary>
    /// Meta title for the web page.
    /// </summary>
    public string MetaTitle { get; set; } = string.Empty;

    /// <summary>
    /// Meta description for the web page.
    /// </summary>
    public string MetaDescription { get; set; } = string.Empty;

    /// <summary>
    /// Meta keywords for the web page.
    /// </summary>
    public string MetaKeywords { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key for the associated post.
    /// </summary>
    public int PostId { get; set; }

}
