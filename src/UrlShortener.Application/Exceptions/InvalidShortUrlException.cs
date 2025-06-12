namespace UrlShortener.Application.Exceptions;

public class InvalidShortUrlException : ApplicationLayerException
{
    public InvalidShortUrlException(ErrorCodes errorCode, string message) 
        : base(errorCode, message) { }
}