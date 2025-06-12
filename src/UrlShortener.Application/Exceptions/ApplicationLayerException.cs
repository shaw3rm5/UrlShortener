namespace UrlShortener.Application.Exceptions;

public abstract class ApplicationLayerException : Exception
{
    public ErrorCodes ErrorCode { get; }

    protected ApplicationLayerException(ErrorCodes errorCode, string message) 
        : base(message)
    {
        ErrorCode = errorCode;
    }
}