namespace UrlShortener.Application.UseCases.DeleteUrl;

public interface IDeleteUrlUseCase
{
    public Task ExecuteAsync(DeleteUrlCommand command, CancellationToken cancellationToken);
}