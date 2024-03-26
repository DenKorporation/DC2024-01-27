using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using REST.Models.Entities;

namespace REST.Data.Configuration;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tblTag").HasKey(t => t.Id);
        builder.Property(t => t.Id).HasColumnName("id");
        builder.HasIndex(t => t.Name).IsUnique();

        builder.Property(t => t.Name).HasColumnName("name");
        builder.ToTable(t => t.HasCheckConstraint("ValidName", "LENGTH(name) BETWEEN 2 AND 32"));

        builder
            .HasMany(t => t.Issues)
            .WithMany(i => i.Tags)
            .UsingEntity<IssueTag>(l => l.HasOne(it => it.Issue).WithMany(i => i.IssueTags),
                r => r.HasOne(it => it.Tag).WithMany(t => t.IssueTags),
                j =>
                {
                    j.ToTable("tblIssueTag").HasKey(it => it.Id);
                    j.Property(it => it.Id).HasColumnName("id");

                    j.Property(it => it.IssueId).HasColumnName("issueId");
                    j.Property(it => it.TagId).HasColumnName("tagId");
                });
    }
}