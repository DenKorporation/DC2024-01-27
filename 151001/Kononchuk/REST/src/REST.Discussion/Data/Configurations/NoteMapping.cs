using Cassandra.Mapping;
using REST.Discussion.Models.Entities;

namespace REST.Discussion.Data.Configurations;

public class NoteMappings : Mappings
{
    public NoteMappings()
    {
        For<Note>().TableName("tblNote")
            .PartitionKey(n => n.Country);
    }
}