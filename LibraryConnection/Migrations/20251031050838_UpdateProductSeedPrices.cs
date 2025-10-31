using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryConnection.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductSeedPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 1,
                column: "price",
                value: 2000.0);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 2,
                column: "price",
                value: 2500.0);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 3,
                column: "price",
                value: 2200.0);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 4,
                column: "price",
                value: 1800.0);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 5,
                column: "price",
                value: 1500.0);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 6,
                column: "price",
                value: 2300.0);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 7,
                column: "price",
                value: 2000.0);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 8,
                column: "price",
                value: 1600.0);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 9,
                column: "price",
                value: 1500.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 1,
                column: "price",
                value: 1.2);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 2,
                column: "price",
                value: 1.8);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 3,
                column: "price",
                value: 1.5);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 4,
                column: "price",
                value: 1.0);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 5,
                column: "price",
                value: 0.90000000000000002);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 6,
                column: "price",
                value: 1.3);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 7,
                column: "price",
                value: 1.1000000000000001);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 8,
                column: "price",
                value: 0.94999999999999996);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 9,
                column: "price",
                value: 0.84999999999999998);
        }
    }
}
