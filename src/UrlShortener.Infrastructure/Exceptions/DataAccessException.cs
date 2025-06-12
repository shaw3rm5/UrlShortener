namespace UrlShortener.Infrastructure.Exceptions;

public abstract class DataAccessException : Exception
{
    public DataAccessException(string message) 
        : base(message) { }
    
}