namespace UrlShortener.Application.UseCases.UpdateOriginalUrl;

public interface IUpdateOriginalUrlUseCase
{
    public Task ExecuteAsync(UpdateOriginalUrlCommand command, CancellationToken cancellationToken);
}