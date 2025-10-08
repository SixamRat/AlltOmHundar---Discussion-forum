using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlltOmHundar.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateForAzure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Allmänt hundprat");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Allmänt hunprat");
        }
    }
}
