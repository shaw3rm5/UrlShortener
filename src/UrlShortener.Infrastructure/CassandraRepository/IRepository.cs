using UrlShorter.Domain.Models;

namespace UrlShortener.Infrastructure.CassandraRepository;

public interface IRepository
{
    public Task AddNewUrlAsync(ShortUrl newUrl, CancellationToken cancellationToken);
    public Task AddClicksInformationAsync(UrlClick newClickInformation, CancellationToken cancellationToken);
    public Task<IEnumerable<UrlClick>> GetClicksAsync (string shortCode, CancellationToken cancellationToken);
    public Task<ShortUrl?> GetUrlAsync(string shortCode, CancellationToken cancellationToken);
    public Task IncrementClicksAsync(string shortCode, CancellationToken cancellationToken);
    public Task UpdateUrlAsync(string shortCode, string newUrl, CancellationToken cancellationToken);
    public Task UpdateUrlAsync(string shortCode, DateTimeOffset newExpirationDate, CancellationToken cancellationToken);
    public Task DeleteUrlAsync(string shortCode, CancellationToken cancellationToken);
}