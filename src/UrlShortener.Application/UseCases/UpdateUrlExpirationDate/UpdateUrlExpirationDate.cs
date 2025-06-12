using FluentValidation;
using UrlShortener.Infrastructure.CassandraRepository;

namespace UrlShortener.Application.UseCases.UpdateUrlExpirationDate;

public class UpdateUrlExpirationDate : IUpdateUrlExpirationDate
{
    private readonly IValidator<UpdateUrlExpirationDateCommand> _validator;
    private readonly IRepository _repository;

    public UpdateUrlExpirationDate(
        IValidator<UpdateUrlExpirationDateCommand> validator,
        IRepository repository)
    {
        _validator = validator;
        _repository = repository;
    }
    
    public async Task ExecuteAsync(UpdateUrlExpirationDateCommand dateCommand, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(dateCommand, cancellationToken);
        await _repository.UpdateUrlAsync(dateCommand.ShortUrl, dateCommand.ExpirationDate, cancellationToken);
    }
}