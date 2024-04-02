using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace REST.Publisher.Data.Migrations
{
    /// <inheritdoc />
    public partial class MoveNoteEntityToDiscussionModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblNote");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "IX_tblNote_issueId",
                table: "tblNote",
                column: "issueId");
        }
    }
}
