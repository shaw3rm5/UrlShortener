using FluentValidation;
using UrlShortener.Infrastructure.CassandraRepository;
using UrlShorter.Domain.Models;

namespace UrlShortener.Application.UseCases.GetClicksInformation;

public class GetClicksInformation : IGetClicksInformation
{
    private readonly IValidator<GetClicksInformationCommand> _validator;
    private readonly IRepository _repository;

    public GetClicksInformation(
        IValidator<GetClicksInformationCommand> validator,
        IRepository repository)
    {
        _validator = validator;
        _repository = repository;
    }
    
    public async Task<(IEnumerable<UrlClick>, int count)> ExecuteAsync(GetClicksInformationCommand command, CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(command, cancellationToken);
        
        var clicksInformationList = await _repository.GetClicksAsync(command.ShortCode, cancellationToken);
        
        return (clicksInformationList, clicksInformationList.Count());
    }
}