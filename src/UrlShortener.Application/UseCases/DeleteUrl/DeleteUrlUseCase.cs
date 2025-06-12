using FluentValidation;
using UrlShortener.Infrastructure.CassandraRepository;

namespace UrlShortener.Application.UseCases.DeleteUrl;

public class DeleteUrlUseCase : IDeleteUrlUseCase
{
    private readonly IValidator<DeleteUrlCommand> _validator;
    private readonly IRepository _repository;

    public DeleteUrlUseCase(
        IValidator<DeleteUrlCommand> validator,
        IRepository repository)
    {
        _validator = validator;
        _repository = repository;
    }
    public async Task ExecuteAsync(DeleteUrlCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);

        await _repository.DeleteUrlAsync(command.ShortUrl, cancellationToken);
    }
}