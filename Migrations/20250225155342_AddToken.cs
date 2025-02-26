using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanAoo.Migrations
{
    /// <inheritdoc />
    public partial class AddToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Employees",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Employees",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "Customers",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "Customers",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "Customers");
        }
    }
}
