using UrlShorter.Domain.Models;

namespace UrlShortener.Application.UseCases.GetClicksInformation;

public interface IGetClicksInformation
{
    public Task<(IEnumerable<UrlClick>, int count)> ExecuteAsync(GetClicksInformationCommand command, CancellationToken cancellationToken);
}