using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crypro.Migrations
{
    /// <inheritdoc />
    public partial class limited4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TradeType",
                table: "TradeLogs",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "TradeType",
                table: "LimitLogs",
                newName: "type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "TradeLogs",
                newName: "TradeType");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "LimitLogs",
                newName: "TradeType");
        }
    }
}
