namespace UrlShortener.Application.UseCases.UpdateUrlExpirationDate;

public interface IUpdateUrlExpirationDate
{
    public Task ExecuteAsync(UpdateUrlExpirationDateCommand dateCommand, CancellationToken cancellationToken);
}