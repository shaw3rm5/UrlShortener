using UrlShorter.Domain.Models;

namespace UrlShortener.Application.UseCases.Redirect;

public interface IRedirectUseCase
{
    public Task<string> ExecuteAsync(RedirectCommand command, UrlClick urlClick, CancellationToken cancellationToken);
}