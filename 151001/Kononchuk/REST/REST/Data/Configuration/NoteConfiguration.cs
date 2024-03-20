using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using REST.Models.Entities;

namespace REST.Data.Configuration;

public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.ToTable("tblNote").HasKey(n => n.Id);
        builder.Property(n => n.Id).HasColumnName("id");

        builder.Property(n => n.IssueId).HasColumnName("issueId");
        builder.HasOne(n => n.Issue)
            .WithMany(i => i.Notes)
            .HasForeignKey(n => n.IssueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(n => n.Content).HasColumnName("content").IsRequired();
        builder.ToTable(n => n.HasCheckConstraint("ValidContent", "LENGTH(content) BETWEEN 2 AND 2048"));
    }
}