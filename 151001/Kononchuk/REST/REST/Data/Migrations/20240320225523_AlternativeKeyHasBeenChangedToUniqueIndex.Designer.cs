﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using REST.Data;

#nullable disable

namespace REST.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240320225523_AlternativeKeyHasBeenChangedToUniqueIndex")]
    partial class AlternativeKeyHasBeenChangedToUniqueIndex
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("REST.Models.Entities.Editor", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("firstname");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("lastname");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("login");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("tblEditor", null, t =>
                        {
                            t.HasCheckConstraint("ValidFirstName", "LENGTH(firstname) BETWEEN 2 AND 64");

                            t.HasCheckConstraint("ValidLastName", "LENGTH(lastname) BETWEEN 2 AND 64");

                            t.HasCheckConstraint("ValidLogin", "LENGTH(login) BETWEEN 2 AND 64");

                            t.HasCheckConstraint("ValidPassword", "LENGTH(password) BETWEEN 8 AND 128");
                        });
                });

            modelBuilder.Entity("REST.Models.Entities.Issue", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<DateTime?>("Created")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created")
                        .HasDefaultValueSql("Now()");

                    b.Property<long?>("EditorId")
                        .HasColumnType("bigint")
                        .HasColumnName("editorId");

                    b.Property<DateTime?>("Modified")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modified")
                        .HasDefaultValueSql("Now()");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.HasIndex("EditorId");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("tblIssue", null, t =>
                        {
                            t.HasCheckConstraint("ValidContent", "LENGTH(content) BETWEEN 4 AND 2048");

                            t.HasCheckConstraint("ValidTitle", "LENGTH(title) BETWEEN 2 AND 64");
                        });
                });

            modelBuilder.Entity("REST.Models.Entities.IssueTag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<long>("IssueId")
                        .HasColumnType("bigint")
                        .HasColumnName("issueId");

                    b.Property<long>("TagId")
                        .HasColumnType("bigint")
                        .HasColumnName("tagId");

                    b.HasKey("Id");

                    b.HasIndex("IssueId");

                    b.HasIndex("TagId");

                    b.ToTable("tblIssueTag", (string)null);
                });

            modelBuilder.Entity("REST.Models.Entities.Note", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<long>("IssueId")
                        .HasColumnType("bigint")
                        .HasColumnName("issueId");

                    b.HasKey("Id");

                    b.HasIndex("IssueId");

                    b.ToTable("tblNote", null, t =>
                        {
                            t.HasCheckConstraint("ValidContent", "LENGTH(content) BETWEEN 2 AND 2048");
                        });
                });

            modelBuilder.Entity("REST.Models.Entities.Tag", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("tblTag", null, t =>
                        {
                            t.HasCheckConstraint("ValidName", "LENGTH(name) BETWEEN 2 AND 32");
                        });
                });

            modelBuilder.Entity("REST.Models.Entities.Issue", b =>
                {
                    b.HasOne("REST.Models.Entities.Editor", "Editor")
                        .WithMany("Issues")
                        .HasForeignKey("EditorId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Editor");
                });

            modelBuilder.Entity("REST.Models.Entities.IssueTag", b =>
                {
                    b.HasOne("REST.Models.Entities.Issue", "Issue")
                        .WithMany("IssueTags")
                        .HasForeignKey("IssueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("REST.Models.Entities.Tag", "Tag")
                        .WithMany("IssueTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Issue");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("REST.Models.Entities.Note", b =>
                {
                    b.HasOne("REST.Models.Entities.Issue", "Issue")
                        .WithMany("Notes")
                        .HasForeignKey("IssueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Issue");
                });

            modelBuilder.Entity("REST.Models.Entities.Editor", b =>
                {
                    b.Navigation("Issues");
                });

            modelBuilder.Entity("REST.Models.Entities.Issue", b =>
                {
                    b.Navigation("IssueTags");

                    b.Navigation("Notes");
                });

            modelBuilder.Entity("REST.Models.Entities.Tag", b =>
                {
                    b.Navigation("IssueTags");
                });
#pragma warning restore 612, 618
        }
    }
}
