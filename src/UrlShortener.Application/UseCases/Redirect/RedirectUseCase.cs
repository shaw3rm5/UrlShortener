using FluentValidation;
using UrlShortener.Application.Exceptions;
using UrlShortener.Infrastructure.CassandraRepository;
using UrlShorter.Domain.Models;

namespace UrlShortener.Application.UseCases.Redirect;

public class RedirectUseCase : IRedirectUseCase
{
    private readonly IValidator<RedirectCommand> _validator;
    private readonly IRepository _repository;

    public RedirectUseCase(
        IValidator<RedirectCommand> validator,
        IRepository repository)
    {
        _validator = validator;
        _repository = repository;
    }
    
    public async Task<string> ExecuteAsync(RedirectCommand command, UrlClick urlClick, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);
        
        var url = await _repository.GetUrlAsync(command.ShortUrl, cancellationToken);
        
        if (url == null)
            throw new InvalidShortUrlException(ErrorCodes.BadRequest, $"url {command.ShortUrl} is invalid");
        
        await _repository.IncrementClicksAsync(url.ShortCode, cancellationToken); 
        await _repository.AddClicksInformationAsync(urlClick, cancellationToken);
        

        return url.OriginalUrl;
    }
}