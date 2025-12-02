using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryConnection.Migrations
{
    /// <inheritdoc />
    public partial class ActualizaModelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dev",
                table: "roles",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1L, "Administrador", "Admin" },
                    { 2L, "Cliente", "Cliente" }
                });

            migrationBuilder.InsertData(
                schema: "dev",
                table: "users",
                columns: new[] { "id", "email", "id_role", "last_name", "name", "password" },
                values: new object[,]
                {
                    { 1L, "admin@snackgol.com", 1L, "SnackGol", "Admin", "Aq8FzdbXqNxd9aINLMuIGg==" },
                    { 2L, "cliente@snackgol.com", 2L, "SnackGol", "Cliente", "dU/QKEZcVX8Ffc08vM8bSg==" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dev",
                table: "users",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                schema: "dev",
                table: "users",
                keyColumn: "id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                schema: "dev",
                table: "roles",
                keyColumn: "id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                schema: "dev",
                table: "roles",
                keyColumn: "id",
                keyValue: 2L);
        }
    }
}
