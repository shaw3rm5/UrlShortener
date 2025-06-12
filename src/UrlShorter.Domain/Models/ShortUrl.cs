namespace UrlShorter.Domain.Models;

public class ShortUrl
{
    public string ShortCode { get; set; }
    public string OriginalUrl { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    public int Clicks { get; set; }
    public string? Alias { get; set; }
}