using System.Collections.Immutable;
using Cassandra;
using UrlShortener.Infrastructure.CassandraBuilder;
using UrlShortener.Infrastructure.Exceptions;
using UrlShorter.Domain.Models;

namespace UrlShortener.Infrastructure.CassandraRepository;

public class Repository : IRepository
{
    private readonly ISession _session;
    
    public Repository(SessionFactory factory)
    {
        _session = factory.GetSession();
    }

    public async Task AddNewUrlAsync(ShortUrl entity, CancellationToken cancellationToken)
    {
        var checkExistsQuery = "SELECT * FROM short_urls WHERE shortcode = ?";
        var rows = await _session.ExecuteAsync(new SimpleStatement(checkExistsQuery, entity.ShortCode));
        if (rows is not null)
            throw new UrlAlreadyExistsException($"URL {entity.ShortCode} is already exists");
        var query = "INSERT INTO short_urls (shortcode, originalurl, createdat, expiresat, clicks, isactive)"
                    + $"VALUES (?, ?, ?, ?, ?, ?)";
        await _session.ExecuteAsync(new SimpleStatement(query,
            entity.ShortCode, entity.OriginalUrl, entity.CreatedAt,  entity.ExpiresAt, entity.Clicks, entity.IsActive));
    }

    public async Task AddClicksInformationAsync(UrlClick newClickInformation, CancellationToken cancellationToken)
    {
        var query = "INSERT INTO url_clicks (shortcode, ipaddress, clickedat, useragent)" +
                    "VALUES (?, ?, ?, ?)";
        await _session.ExecuteAsync(new SimpleStatement(
            query, newClickInformation.ShortCode,
            newClickInformation.IpAddress,
            DateTimeOffset.UtcNow, 
            newClickInformation.UserAgent));
    }

    public async Task<ShortUrl?> GetUrlAsync(string shortCode, CancellationToken cancellationToken)
    {
        var query = "SELECT * FROM short_urls WHERE shortcode = ? AND isActive = TRUE ALLOW FILTERING";
        var rowSet = await _session.ExecuteAsync(new SimpleStatement(query, shortCode));
        var row = rowSet.FirstOrDefault();
        if (row is null) 
            return null;
        return new ShortUrl
        {
            ShortCode = row.GetValue<string>("shortcode"),
            OriginalUrl = row.GetValue<string>("originalurl"),
            CreatedAt = row.GetValue<DateTimeOffset>("createdat"),
            ExpiresAt = row.GetValue<DateTimeOffset?>("expiresat"),
            Clicks = row.GetValue<int>("clicks"),
            IsActive = row.GetValue<bool>("isactive")
        };
    }

    public async Task IncrementClicksAsync(string shortCode, CancellationToken cancellationToken)
    {
        await ThrowIfRowsNotFound(shortCode);
        var getQuery = "SELECT clicks FROM short_urls WHERE shortcode = ?";
        var clicksRowsSet = await _session.ExecuteAsync(new SimpleStatement(getQuery, shortCode));
        var clicks = clicksRowsSet.FirstOrDefault()?.GetValue<int>("clicks");
        var setQuery = "UPDATE short_urls SET clicks = ? WHERE shortcode = ?";
        await _session.ExecuteAsync(new SimpleStatement(setQuery, clicks + 1, shortCode));
    }

    public async Task<IEnumerable<UrlClick>> GetClicksAsync(string shortCode, CancellationToken cancellationToken)
    {
        await ThrowIfRowsNotFound(shortCode);
        var query = "SELECT useragent, ipaddress, clickedat FROM url_clicks WHERE shortcode = ? ";
        var rowSet = await _session.ExecuteAsync(new SimpleStatement(query, shortCode));
        return rowSet.Select(r => new UrlClick
        {
            ShortCode = shortCode,
            UserAgent = r.GetValue<string>("useragent"),
            IpAddress = r.GetValue<string>("ipaddress"),
            ClickedAt = r.GetValue<DateTimeOffset>("clickedat"),
        }).ToImmutableArray();
    }
    
    /// <summary>
    /// Update Url signature, you can write new Original Url for this code
    /// </summary>
    /// <param name="shortCode"></param>
    /// <param name="newUrl"></param>
    /// <param name="cancellationToken"></param>
    
    public async Task UpdateUrlAsync(string shortCode, string newUrl, CancellationToken cancellationToken)
    {
        await ThrowIfRowsNotFound(shortCode);
        var updateQuery = "UPDATE short_urls SET originalurl = ? WHERE shortcode = ? ";
        await _session.ExecuteAsync(new SimpleStatement(updateQuery, newUrl, shortCode));
    }
    
    /// <summary>
    /// Update Url Expiration date
    /// </summary>
    /// <param name="shortCode"></param>
    /// <param name="newExpirationDate"></param>
    /// <param name="cancellationToken"></param>
    public async Task UpdateUrlAsync(string shortCode, DateTimeOffset newExpirationDate, CancellationToken cancellationToken)
    {
        await ThrowIfRowsNotFound(shortCode);
        var query = "UPDATE short_urls SET expiresAt = ? WHERE shortcode = ? ";
        await _session.ExecuteAsync(new SimpleStatement(query, newExpirationDate,shortCode));
    }
    
    public async Task DeleteUrlAsync(string shortCode, CancellationToken cancellationToken)
    {
        await ThrowIfRowsNotFound(shortCode);
        var deleteQuery = "DELETE FROM short_urls WHERE shortcode = ? ";
        await _session.ExecuteAsync(new SimpleStatement(deleteQuery, shortCode));
    }

    private async Task ThrowIfRowsNotFound(string shortCode)
    {
        var getQuery = "SELECT * FROM short_urls WHERE shortcode = ?";
        var rowSet = await _session.ExecuteAsync(new SimpleStatement(getQuery, shortCode));
        if (rowSet?.FirstOrDefault() == null)
            throw new UrlNotFoundException($"Url {shortCode} not found");
    }
    
}