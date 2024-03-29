using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REST.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlternativeKeyHasBeenChangedToUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_tblTag_name",
                table: "tblTag");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_tblIssue_title",
                table: "tblIssue");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_tblEditor_login",
                table: "tblEditor");

            migrationBuilder.CreateIndex(
                name: "IX_tblTag_name",
                table: "tblTag",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblIssue_title",
                table: "tblIssue",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblEditor_login",
                table: "tblEditor",
                column: "login",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tblTag_name",
                table: "tblTag");

            migrationBuilder.DropIndex(
                name: "IX_tblIssue_title",
                table: "tblIssue");

            migrationBuilder.DropIndex(
                name: "IX_tblEditor_login",
                table: "tblEditor");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_tblTag_name",
                table: "tblTag",
                column: "name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_tblIssue_title",
                table: "tblIssue",
                column: "title");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_tblEditor_login",
                table: "tblEditor",
                column: "login");
        }
    }
}
