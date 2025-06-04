using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crypro.Migrations
{
    /// <inheritdoc />
    public partial class test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FeeLogs_UserId",
                table: "FeeLogs");

            migrationBuilder.CreateIndex(
                name: "IX_FeeLogs_UserId",
                table: "FeeLogs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FeeLogs_UserId",
                table: "FeeLogs");

            migrationBuilder.CreateIndex(
                name: "IX_FeeLogs_UserId",
                table: "FeeLogs",
                column: "UserId",
                unique: true);
        }
    }
}
