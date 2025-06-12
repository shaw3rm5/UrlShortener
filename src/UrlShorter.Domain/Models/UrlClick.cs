namespace UrlShorter.Domain.Models;

public class UrlClick
{
    public string ShortCode { get; set; } // primary key
    public string UserAgent { get; set; }
    public string IpAddress { get; set; }
    
    public DateTimeOffset ClickedAt { get; set; }
}