using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryConnection.Migrations
{
    /// <inheritdoc />
    public partial class SyncModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 1,
                column: "image_url",
                value: "https://images.unsplash.com/photo-1548833793-71ad3875f2ac?w=800&q=80");

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 2,
                column: "image_url",
                value: "https://images.unsplash.com/photo-1623446043588-739e8f6daba1?w=800&q=80");

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 3,
                column: "image_url",
                value: "https://images.unsplash.com/photo-1542444459-db63c0d2c979?w=800&q=80");

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 4,
                column: "image_url",
                value: "https://images.unsplash.com/photo-1596461404969-9ae70d18e7d5?w=800&q=80");

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 5,
                column: "image_url",
                value: "https://images.unsplash.com/photo-1601000924815-65f0f41cd3ed?w=800&q=80");

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 6,
                column: "image_url",
                value: "https://images.unsplash.com/photo-1604908554049-69a3a9c7f4ec?w=800&q=80");

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 7,
                column: "image_url",
                value: "https://images.unsplash.com/photo-1548907040-4b7d48268e8b?w=800&q=80");

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 8,
                column: "image_url",
                value: "https://images.unsplash.com/photo-1589712230557-cf5f38f3d001?w=800&q=80");

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 9,
                column: "image_url",
                value: "https://images.unsplash.com/photo-1581323361970-989d7a9b6eb9?w=800&q=80");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 1,
                column: "image_url",
                value: null);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 2,
                column: "image_url",
                value: null);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 3,
                column: "image_url",
                value: null);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 4,
                column: "image_url",
                value: null);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 5,
                column: "image_url",
                value: null);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 6,
                column: "image_url",
                value: null);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 7,
                column: "image_url",
                value: null);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 8,
                column: "image_url",
                value: null);

            migrationBuilder.UpdateData(
                schema: "dev",
                table: "products",
                keyColumn: "id",
                keyValue: 9,
                column: "image_url",
                value: null);
        }
    }
}
