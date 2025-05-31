
using System.Text.RegularExpressions;

namespace Application.Utilities;

/// <summary>
/// Provides utility methods for generating URL-friendly slugs.
/// </summary>
public static class SlugHelper
{
    /// <summary>
    /// Generates a URL-friendly slug from a given phrase.
    /// The slug will be lowercase, with spaces and '&' replaced by hyphens,
    /// and invalid characters removed.
    /// </summary>
    /// <param name="phrase">The input string from which to generate the slug.</param>
    /// <returns>A URL-friendly slug. Returns an empty string if the input phrase is null or whitespace.</returns>
    public static string GenerateSlug(string? phrase)
    {
        if (string.IsNullOrWhiteSpace(phrase))
            return string.Empty;

        // Convert to lowercase
        string normalizedPhrase = phrase.ToLowerInvariant();

        // Replace spaces with hyphens
        normalizedPhrase = Regex.Replace(normalizedPhrase, @"\s", "-");

        // Replace '&' with "and"
        normalizedPhrase = normalizedPhrase.Replace("&", "and");

        // Remove all invalid characters (non-alphanumeric except hyphen)
        normalizedPhrase = Regex.Replace(normalizedPhrase, @"[^a-z0-9\-]", "");

        // Replace multiple hyphens with a single hyphen
        normalizedPhrase = Regex.Replace(normalizedPhrase, @"-+", "-");

        // Trim leading/trailing hyphens
        return normalizedPhrase.Trim('-');
    }
}
