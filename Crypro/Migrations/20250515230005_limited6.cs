using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crypro.Migrations
{
    /// <inheritdoc />
    public partial class limited6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "TradeLogs");

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "LimitedTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "LimitedTransactions");

            migrationBuilder.AddColumn<int>(
                name: "type",
                table: "TradeLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
