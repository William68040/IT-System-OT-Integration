using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Shared.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "IsActive", "MachineType", "Name", "ProductionTime" },
                values: new object[,]
                {
                    { 1, "925 sterling silver necklace", true, 0, "Silver Necklace", 10 },
                    { 2, "925 sterling silver ring", true, 2, "Silver Ring", 12 },
                    { 3, "925 sterling silver bracelet", true, 1, "Silver Bracelet", 7 },
                    { 4, "925 sterling silver earrings", true, 3, "Silver Earrings", 5 },
                    { 5, "24k gold necklace", true, 0, "Gold Necklace", 12 },
                    { 6, "24k gold ring", true, 2, "Gold Ring", 15 },
                    { 7, "18k gold bracelet", true, 1, "Gold Bracelet", 8 },
                    { 8, "24k gold earrings", true, 3, "Gold Earrings", 6 },
                    { 9, "Gold necklace with diamond details", true, 0, "Diamond Necklace", 18 },
                    { 10, "Gold ring with diamond details", true, 2, "Diamond Ring", 20 },
                    { 11, "Gold bracelet with diamond details", true, 1, "Diamond Bracelet", 14 },
                    { 12, "Gold earrings with diamond details", true, 3, "Diamond Earrings", 10 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);
        }
    }
}
