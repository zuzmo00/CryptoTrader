using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crypro.Migrations
{
    /// <inheritdoc />
    public partial class limited8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "LimitLogs",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "LimitedTransactions",
                newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "LimitLogs",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "LimitedTransactions",
                newName: "type");
        }
    }
}
