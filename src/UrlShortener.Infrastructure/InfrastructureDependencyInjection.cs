using Microsoft.Extensions.DependencyInjection;
using UrlShortener.Infrastructure.CassandraBuilder;
using UrlShortener.Infrastructure.CassandraRepository;

namespace UrlShortener.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static void AddInfrastructureDependencies(this IServiceCollection services)
    {
        
        services
            .AddSingleton<SessionFactory>()
            .AddSingleton<IRepository, Repository>();
    }
}