using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using REST.Models.Entities;

namespace REST.Data.Configuration;

public class EditorConfiguration : IEntityTypeConfiguration<Editor>
{
    public void Configure(EntityTypeBuilder<Editor> builder)
    {
        builder.ToTable("tblEditor").HasKey(e => e.Id);
        builder.Property(e => e.Id).HasColumnName("id");

        builder.HasIndex(e => e.Login).IsUnique();
        builder.Property(e => e.Login).HasColumnName("login");
        builder.ToTable(e => e.HasCheckConstraint("ValidLogin", "LENGTH(login) BETWEEN 2 AND 64"));

        builder.Property(e => e.Password).HasColumnName("password");
        builder.ToTable(e => e.HasCheckConstraint("ValidPassword", "LENGTH(password) BETWEEN 8 AND 128"));
        
        builder.Property(e => e.FirstName).HasColumnName("firstname");
        builder.ToTable(e => e.HasCheckConstraint("ValidFirstName", "LENGTH(firstname) BETWEEN 2 AND 64"));
        
        builder.Property(e => e.LastName).HasColumnName("lastname");
        builder.ToTable(e => e.HasCheckConstraint("ValidLastName", "LENGTH(lastname) BETWEEN 2 AND 64"));
    }
}