using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryConnection.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dev");

            migrationBuilder.CreateTable(
                name: "carts",
                schema: "dev",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: true),
                    session_token = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_carts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "dev",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "clients",
                schema: "dev",
                columns: table => new
                {
                    document = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    emailAddress = table.Column<string>(type: "text", nullable: true),
                    docType = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.document);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "dev",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                schema: "dev",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    price = table.Column<double>(type: "double precision", nullable: false),
                    stock = table.Column<int>(type: "integer", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_products_categories_category_id",
                        column: x => x.category_id,
                        principalSchema: "dev",
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "dev",
                columns: table => new
                {
                    order_id = table.Column<string>(type: "text", nullable: false),
                    customer_id = table.Column<string>(type: "text", nullable: false),
                    order_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<string>(type: "text", nullable: true),
                    total_gross_amount = table.Column<double>(type: "double precision", nullable: false),
                    total_net_price = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.order_id);
                    table.ForeignKey(
                        name: "FK_orders_clients_customer_id",
                        column: x => x.customer_id,
                        principalSchema: "dev",
                        principalTable: "clients",
                        principalColumn: "document",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "dev",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    id_role = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_roles_id_role",
                        column: x => x.id_role,
                        principalSchema: "dev",
                        principalTable: "roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cart_items",
                schema: "dev",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cart_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price = table.Column<double>(type: "double precision", nullable: false),
                    subtotal = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cart_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_cart_items_carts_cart_id",
                        column: x => x.cart_id,
                        principalSchema: "dev",
                        principalTable: "carts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_cart_items_products_product_id",
                        column: x => x.product_id,
                        principalSchema: "dev",
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "orderLines",
                schema: "dev",
                columns: table => new
                {
                    lineNum = table.Column<int>(type: "integer", nullable: false),
                    orderId = table.Column<string>(type: "text", nullable: false),
                    item = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: true),
                    description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    gross_amount = table.Column<double>(type: "double precision", nullable: true),
                    net_price = table.Column<double>(type: "double precision", nullable: true),
                    tax_amount = table.Column<double>(type: "double precision", nullable: false),
                    quantity = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orderLines", x => new { x.orderId, x.lineNum });
                    table.ForeignKey(
                        name: "FK_orderLines_orders_orderId",
                        column: x => x.orderId,
                        principalSchema: "dev",
                        principalTable: "orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "dev",
                table: "categories",
                columns: new[] { "id", "is_active", "name" },
                values: new object[,]
                {
                    { 1, true, "Bebidas" },
                    { 2, true, "Snacks" },
                    { 3, true, "Dulces" }
                });

            migrationBuilder.InsertData(
                schema: "dev",
                table: "products",
                columns: new[] { "id", "category_id", "description", "image_url", "is_active", "name", "price", "stock" },
                values: new object[,]
                {
                    { 1, 1, "Botella de agua", null, true, "Agua mineral 600ml", 1.2, 100 },
                    { 2, 1, "Bebida gaseosa", null, true, "Gaseosa cola 500ml", 1.8, 80 },
                    { 3, 1, "Sabor naranja", null, true, "Jugo natural 350ml", 1.5, 60 },
                    { 4, 2, "Snack salado", null, true, "Papitas clásicas 45g", 1.0, 120 },
                    { 5, 2, "Bolsa de maní", null, true, "Maní salado 50g", 0.90000000000000002, 90 },
                    { 6, 2, "Con queso", null, true, "Nachos 70g", 1.3, 70 },
                    { 7, 3, "70% cacao", null, true, "Chocolate barra 40g", 1.1000000000000001, 50 },
                    { 8, 3, "Frutales", null, true, "Gomitas 90g", 0.94999999999999996, 65 },
                    { 9, 3, "Mix sabores", null, true, "Caramelos surtidos 100g", 0.84999999999999998, 150 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_cart_id_product_id",
                schema: "dev",
                table: "cart_items",
                columns: new[] { "cart_id", "product_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cart_items_product_id",
                schema: "dev",
                table: "cart_items",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_carts_session_token",
                schema: "dev",
                table: "carts",
                column: "session_token");

            migrationBuilder.CreateIndex(
                name: "IX_carts_user_id",
                schema: "dev",
                table: "carts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_customer_id",
                schema: "dev",
                table: "orders",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_category_id",
                schema: "dev",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_is_active",
                schema: "dev",
                table: "products",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "IX_users_id_role",
                schema: "dev",
                table: "users",
                column: "id_role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cart_items",
                schema: "dev");

            migrationBuilder.DropTable(
                name: "orderLines",
                schema: "dev");

            migrationBuilder.DropTable(
                name: "users",
                schema: "dev");

            migrationBuilder.DropTable(
                name: "carts",
                schema: "dev");

            migrationBuilder.DropTable(
                name: "products",
                schema: "dev");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "dev");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "dev");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "dev");

            migrationBuilder.DropTable(
                name: "clients",
                schema: "dev");
        }
    }
}
