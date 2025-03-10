using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanAoo.Migrations
{
    /// <inheritdoc />
    public partial class AddForgotPass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordToken",
                table: "Employees",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordTokenExpiry",
                table: "Employees",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordToken",
                table: "Customers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetPasswordTokenExpiry",
                table: "Customers",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordToken",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ResetPasswordTokenExpiry",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ResetPasswordToken",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "ResetPasswordTokenExpiry",
                table: "Customers");
        }
    }
}
