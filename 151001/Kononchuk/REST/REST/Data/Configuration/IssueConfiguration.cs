using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using REST.Models.Entities;

namespace REST.Data.Configuration;

public class IssueConfiguration : IEntityTypeConfiguration<Issue>
{
    //TODO Properly handle created and modified fields
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.ToTable("tblIssue").HasKey(i => i.Id);
        builder.Property(i => i.Id).HasColumnName("id");
        builder.HasIndex(i => i.Title).IsUnique();

        builder.Property(i => i.EditorId).HasColumnName("editorId");
        builder.HasOne(i => i.Editor)
            .WithMany(e => e.Issues)
            .HasForeignKey(i => i.EditorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Property(i => i.Title).HasColumnName("title");
        builder.ToTable(i => i.HasCheckConstraint("ValidTitle", "LENGTH(title) BETWEEN 2 AND 64"));

        builder.Property(i => i.Content).HasColumnName("content").IsRequired();
        builder.ToTable(i => i.HasCheckConstraint("ValidContent", "LENGTH(content) BETWEEN 4 AND 2048"));

        builder.Property(i => i.Created).HasColumnName("created").IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        builder.Property(i => i.Modified).HasColumnName("modified").IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP").ValueGeneratedOnAddOrUpdate();
    }
}