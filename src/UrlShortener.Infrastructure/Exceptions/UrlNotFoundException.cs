namespace UrlShortener.Infrastructure.Exceptions;

public class UrlNotFoundException : DataAccessException
{
    public UrlNotFoundException(string message) : base(message) { }
}