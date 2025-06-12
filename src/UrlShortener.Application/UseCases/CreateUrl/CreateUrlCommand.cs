namespace UrlShortener.Application.UseCases.CreateUrl;

public record CreateUrlCommand(string OriginalUrl, DateTimeOffset? ExpiredAt, string? Alias);