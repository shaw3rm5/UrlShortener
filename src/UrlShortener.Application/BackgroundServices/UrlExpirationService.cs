using Cassandra;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UrlShortener.Infrastructure;
using UrlShortener.Infrastructure.CassandraBuilder;
using UrlShortener.Infrastructure.CassandraRepository;

namespace UrlShortener.Application.BackgroundServices;

public class UrlExpirationService : BackgroundService
{
    
    private readonly ISession _session;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(30);
    
    public UrlExpirationService(SessionFactory factory)
    {
        _session = factory.GetSession();
    }
    
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        { 
            await CheckIfExpiredAsync(cancellationToken);
            await Task.Delay(_checkInterval, cancellationToken);
        }
    }

    private async Task CheckIfExpiredAsync(CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        var query = "SELECT shortCode, expiresAt FROM short_urls WHERE isActive = true ALLOW FILTERING";
        var result = await _session.ExecuteAsync(new SimpleStatement(query));

        foreach (var r in result)
        {
            var expiredAtDate = r.GetValue<DateTimeOffset?>("expiresat");
            if (expiredAtDate < now)
                await DeactivateExpiredUrlAsync(r.GetValue<string>("shortcode"), cancellationToken);
        }
        
    }
    
    private async Task DeactivateExpiredUrlAsync(string shortCode, CancellationToken cancellation)
    {
        var query = "UPDATE short_urls SET isactive = false WHERE shortcode = ? ";
        await _session.ExecuteAsync(new SimpleStatement(query, shortCode));
    }
}   