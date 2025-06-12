using FluentValidation;
using UrlShortener.Infrastructure.CassandraRepository;

namespace UrlShortener.Application.UseCases.UpdateOriginalUrl;

public class UpdateOriginalUrlUseCase : IUpdateOriginalUrlUseCase
{
    private readonly IValidator<UpdateOriginalUrlCommand> _validator;
    private readonly IRepository _repository;

    public UpdateOriginalUrlUseCase(
        IValidator<UpdateOriginalUrlCommand> validator,
        IRepository repository)
    {
        _validator = validator;
        _repository = repository;
    }
    
    public async Task ExecuteAsync(UpdateOriginalUrlCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);
        await _repository.UpdateUrlAsync(command.ShortUrl, command.OriginalUrl, cancellationToken);
    }
}