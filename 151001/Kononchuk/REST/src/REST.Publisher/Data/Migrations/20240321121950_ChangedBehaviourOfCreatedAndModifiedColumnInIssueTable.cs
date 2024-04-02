using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REST.Publisher.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangedBehaviourOfCreatedAndModifiedColumnInIssueTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "modified",
                table: "tblIssue",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "Now()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "created",
                table: "tblIssue",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "Now()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "modified",
                table: "tblIssue",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "Now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created",
                table: "tblIssue",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "Now()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");
        }
    }
}
