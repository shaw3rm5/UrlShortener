namespace UrlShortener.Infrastructure.CassandraBuilder;

public class CassandraConfiguration
{
    public string ConnectionString { get; set; }
    public string KeySpace { get; set; }
    public int DefaultPort { get; set; }
}