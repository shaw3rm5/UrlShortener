using UrlShorter.Domain.Models;

namespace UrlShortener.Application.UseCases.CreateUrl;

public interface ICreateUrlUseCase
{
    public Task<ShortUrl> ExecuteAsync(CreateUrlCommand command, CancellationToken cancellationToken); 
}