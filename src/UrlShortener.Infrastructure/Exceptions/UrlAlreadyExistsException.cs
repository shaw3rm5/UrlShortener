namespace UrlShortener.Infrastructure.Exceptions;

public class UrlAlreadyExistsException : DataAccessException
{
    public UrlAlreadyExistsException(string message) : base(message) { }
}