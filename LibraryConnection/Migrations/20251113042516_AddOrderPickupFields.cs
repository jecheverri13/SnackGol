using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryConnection.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderPickupFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "pickup_code",
                schema: "dev",
                table: "orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "pickup_generated_at",
                schema: "dev",
                table: "orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pickup_payload_base64",
                schema: "dev",
                table: "orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pickup_qr_base64",
                schema: "dev",
                table: "orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "pickup_redeemed_at",
                schema: "dev",
                table: "orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pickup_token_hash",
                schema: "dev",
                table: "orders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pickup_verified_by",
                schema: "dev",
                table: "orders",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pickup_code",
                schema: "dev",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "pickup_generated_at",
                schema: "dev",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "pickup_payload_base64",
                schema: "dev",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "pickup_qr_base64",
                schema: "dev",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "pickup_redeemed_at",
                schema: "dev",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "pickup_token_hash",
                schema: "dev",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "pickup_verified_by",
                schema: "dev",
                table: "orders");
        }
    }
}
