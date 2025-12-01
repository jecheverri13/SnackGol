using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryConnection.Migrations
{
    /// <inheritdoc />
    public partial class ActualizarModelo : Migration
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
                    total_net_price = table.Column<double>(type: "double precision", nullable: false),
                    pickup_code = table.Column<string>(type: "text", nullable: true),
                    pickup_token_hash = table.Column<string>(type: "text", nullable: true),
                    pickup_payload_base64 = table.Column<string>(type: "text", nullable: true),
                    pickup_qr_base64 = table.Column<string>(type: "text", nullable: true),
                    pickup_generated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    pickup_redeemed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    pickup_verified_by = table.Column<string>(type: "text", nullable: true)
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
                    { 1, 1, "Botella de agua", "https://images.unsplash.com/photo-1637774139107-b83aae618551?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=735", true, "Agua mineral 600ml", 2000.0, 100 },
                    { 2, 1, "Bebida gaseosa", "https://images.unsplash.com/photo-1622708862830-a026e3ef60bd?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=682", true, "Gaseosa cola 500ml", 2500.0, 80 },
                    { 3, 1, "Sabor naranja", "https://images.unsplash.com/photo-1621506289894-c3a62d6be8f3?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=687", true, "Jugo natural 350ml", 2200.0, 60 },
                    { 4, 2, "Snack salado", "https://images.unsplash.com/photo-1741520150134-0d60d82dfac9?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D&auto=format&fit=crop&q=80&w=1406", true, "Papitas clásicas 45g", 1800.0, 120 },
                    { 5, 2, "Bolsa de maní", "https://media.istockphoto.com/id/2149777376/photo/salted-roasted-peanuts-in-bowl-on-kitchen-table.jpg?s=612x612&w=0&k=20&c=Xb0yc6TXrYSqtGX4mtnQxhGORsmG0aXWtekhAgS85Ks=", true, "Maní salado 50g", 1500.0, 90 },
                    { 6, 2, "Con queso", "https://stordfkenticomedia.blob.core.windows.net/df-us/rms/media/recipemediafiles/recipe%20images%20and%20files/k12/desktop%20(900x600)/2024/apr/2024_k12_ultimate-jalapeno-nachos_900x600.jpg?ext=.jpg", true, "Nachos 70g", 2300.0, 70 },
                    { 7, 3, "70% cacao", "https://www.ferrerorocher.com/us/sites/ferrerorocher20_us/files/2025-08/250035-t1-rocher-dark-frontale-eng.png?t=1756883333", true, "Chocolate barra 40g", 2000.0, 50 },
                    { 8, 3, "Frutales", "http://sweetasfudge.com/cdn/shop/products/Assorted-Fruit-Slices-on-White.jpg?v=1681352307", true, "Gomitas 90g", 1600.0, 65 },
                    { 9, 3, "Bandeja de cupcakes", "https://plus.unsplash.com/premium_photo-1681506436855-db38ac7cf9e6?ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1yZWxhdGVkfDR8fHxlbnwwfHx8fHw%3D&auto=format&fit=crop&q=80&w=800", true, "Cupcakes surtidos", 1800.0, 40 }
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
