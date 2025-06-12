namespace UrlShortener.Application.UseCases.UpdateOriginalUrl;

public record UpdateOriginalUrlCommand(string ShortUrl, string OriginalUrl);