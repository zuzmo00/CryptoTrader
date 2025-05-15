using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crypro.Migrations
{
    /// <inheritdoc />
    public partial class limited7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "LimitLogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "LimitedTransactions",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "LimitLogs");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "LimitedTransactions");
        }
    }
}
