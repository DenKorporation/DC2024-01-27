using Cassandra;
using Cassandra.Mapping;
using REST.Discussion.Data.Configurations;
using ISession = Cassandra.ISession;

namespace REST.Discussion.Data;

public class CassandraContext
{
    private readonly ISession _session;

    public ISession Session => _session;

    static CassandraContext()
    {
        MappingConfiguration.Global.Define<NoteMappings>();   
    }
    
    public CassandraContext(string? connectionString, string? keyspace)
    {
        ArgumentNullException.ThrowIfNull(connectionString);
        ArgumentNullException.ThrowIfNull(keyspace);
        var cluster = Cluster.Builder()
            .AddContactPoint(connectionString)
            .Build();

        _session = cluster.Connect(keyspace);
    }
}