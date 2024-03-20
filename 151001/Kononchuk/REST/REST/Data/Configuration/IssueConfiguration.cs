using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using REST.Models.Entities;

namespace REST.Data.Configuration;

public class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.ToTable("tblIssue").HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnName("id");
        builder.HasAlternateKey(i => i.Title);

        builder.Property(i => i.EditorId).HasColumnName("editorId");
        builder.HasOne(i => i.Editor)
            .WithMany(e => e.Issues)
            .HasForeignKey(i => i.EditorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(i => i.Title).HasColumnName("title");
        builder.ToTable(i => i.HasCheckConstraint("ValidTitle", "LENGTH(title) BETWEEN 2 AND 64"));

        builder.Property(i => i.Content).HasColumnName("content").IsRequired();
        builder.ToTable(i => i.HasCheckConstraint("ValidContent", "LENGTH(content) BETWEEN 4 AND 2048"));

        builder.Property(i => i.Created).HasColumnName("created").IsRequired().HasDefaultValueSql("Now()");
        builder.Property(i => i.Modified).HasColumnName("modified").IsRequired().HasDefaultValueSql("Now()");
    }
}