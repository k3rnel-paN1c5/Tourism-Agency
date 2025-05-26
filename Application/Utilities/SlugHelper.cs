
namespace Application.Utilities;

public static class SlugHelper
{
    public static string GenerateSlug(string? phrase)
    {
        if (string.IsNullOrWhiteSpace(phrase)) return string.Empty;

        var stripped = phrase
            .ToLower()
            .Replace(" ", "-")
            .Replace("&", "and")
            .Replace("--", "-");

        // Remove any invalid characters
        var result = new string(Array.FindAll(stripped.ToCharArray(),
            c => (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '-'));

        return result.Trim('-');
    }
}
