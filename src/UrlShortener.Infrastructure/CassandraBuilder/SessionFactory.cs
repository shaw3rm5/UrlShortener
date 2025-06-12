using Cassandra;
using Microsoft.Extensions.Options;

namespace UrlShortener.Infrastructure.CassandraBuilder;

public class SessionFactory
{
    private readonly CassandraConfiguration _configuration;
    private readonly ISession _session;
    
    public SessionFactory(IOptions<CassandraConfiguration> cassanrdaConfigurations)
    {
        _configuration = cassanrdaConfigurations.Value;
        _session = SetSession(
            _configuration.ConnectionString,
            _configuration.KeySpace,
            _configuration.DefaultPort);
    }

    public ISession GetSession() => _session;
    
    private ISession SetSession(string connectionString, string keyspace, int defaultPort)
    {
        var cluster = Cluster
            .Builder()
            .AddContactPoint(connectionString)
            .WithPort(defaultPort)
            .Build();
        
        var session = cluster.Connect();

        session.Execute(new SimpleStatement(
            @"
                CREATE KEYSPACE IF NOT EXISTS ShortUrl
                WITH replication = {
                  'class': 'SimpleStrategy',
                  'replication_factor': 1
                };"));
        
        session.Execute("USE  " + keyspace);
        
        session.Execute(new SimpleStatement(
            @"
        CREATE TABLE IF NOT EXISTS short_urls (
            shortCode text PRIMARY KEY,
            originalUrl text,
            createdAt timestamp,
            expiresAt timestamp,
            isActive boolean,
            clicks int,
            alias text
        );"));
        
        session.Execute(new SimpleStatement(
            @"
        CREATE TABLE IF NOT EXISTS url_clicks (
            shortcode text,
            clickedat timestamp,
            ipaddress text,
            useragent text,
            PRIMARY KEY (shortcode, clickedat)
        ) WITH CLUSTERING ORDER BY (clickedat DESC);")
        );
        
        return session;
    }
}