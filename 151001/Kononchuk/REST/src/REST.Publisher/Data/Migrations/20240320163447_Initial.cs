using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace REST.Publisher.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblEditor",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    login = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    firstname = table.Column<string>(type: "text", nullable: false),
                    lastname = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEditor", x => x.id);
                    table.UniqueConstraint("AK_tblEditor_login", x => x.login);
                    table.CheckConstraint("ValidFirstName", "LENGTH(firstname) BETWEEN 2 AND 64");
                    table.CheckConstraint("ValidLastName", "LENGTH(lastname) BETWEEN 2 AND 64");
                    table.CheckConstraint("ValidLogin", "LENGTH(login) BETWEEN 2 AND 64");
                    table.CheckConstraint("ValidPassword", "LENGTH(password) BETWEEN 8 AND 128");
                });

            migrationBuilder.CreateTable(
                name: "tblTag",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblTag", x => x.id);
                    table.UniqueConstraint("AK_tblTag_name", x => x.name);
                    table.CheckConstraint("ValidName", "LENGTH(name) BETWEEN 2 AND 32");
                });

            migrationBuilder.CreateTable(
                name: "tblIssue",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    editorId = table.Column<long>(type: "bigint", nullable: true),
                    title = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "Now()"),
                    modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "Now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblIssue", x => x.id);
                    table.UniqueConstraint("AK_tblIssue_title", x => x.title);
                    table.CheckConstraint("ValidContent", "LENGTH(content) BETWEEN 4 AND 2048");
                    table.CheckConstraint("ValidTitle", "LENGTH(title) BETWEEN 2 AND 64");
                    table.ForeignKey(
                        name: "FK_tblIssue_tblEditor_editorId",
                        column: x => x.editorId,
                        principalTable: "tblEditor",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "tblIssueTag",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    issueId = table.Column<long>(type: "bigint", nullable: false),
                    tagId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblIssueTag", x => x.id);
                    table.ForeignKey(
                        name: "FK_tblIssueTag_tblIssue_issueId",
                        column: x => x.issueId,
                        principalTable: "tblIssue",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tblIssueTag_tblTag_tagId",
                        column: x => x.tagId,
                        principalTable: "tblTag",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tblNote",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    issueId = table.Column<long>(type: "bigint", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblNote", x => x.id);
                    table.CheckConstraint("ValidContent", "LENGTH(content) BETWEEN 2 AND 2048");
                    table.ForeignKey(
                        name: "FK_tblNote_tblIssue_issueId",
                        column: x => x.issueId,
                        principalTable: "tblIssue",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblIssue_editorId",
                table: "tblIssue",
                column: "editorId");

            migrationBuilder.CreateIndex(
                name: "IX_tblIssueTag_issueId",
                table: "tblIssueTag",
                column: "issueId");

            migrationBuilder.CreateIndex(
                name: "IX_tblIssueTag_tagId",
                table: "tblIssueTag",
                column: "tagId");

            migrationBuilder.CreateIndex(
                name: "IX_tblNote_issueId",
                table: "tblNote",
                column: "issueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblIssueTag");

            migrationBuilder.DropTable(
                name: "tblNote");

            migrationBuilder.DropTable(
                name: "tblTag");

            migrationBuilder.DropTable(
                name: "tblIssue");

            migrationBuilder.DropTable(
                name: "tblEditor");
        }
    }
}
