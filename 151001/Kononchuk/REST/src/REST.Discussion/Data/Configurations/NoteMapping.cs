using Cassandra.Mapping;
using REST.Discussion.Models.Entities;
using Cassandra.Mapping.Attributes;

namespace REST.Discussion.Data.Configurations;

public class NoteMappings : Mappings
{
    public NoteMappings()
    {
        For<Note>()
            .TableName("tblNote")
            .PartitionKey(n => n.Country)
            .ClusteringKey(n => n.IssueId)
            .ClusteringKey(n => n.Id)
            .Column(n => n.Country, col => col.WithName("country"))
            .Column(n => n.IssueId, col => col.WithName("issueId"))
            .Column(n => n.Id, col => col.WithName("id"))
            .Column(n => n.Content, col => col.WithName("content"));
    }
}