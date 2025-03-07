using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanAoo.Migrations
{
    /// <inheritdoc />
    public partial class FixEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Employees",
                newName: "City");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "City",
                table: "Employees",
                newName: "Country");
        }
    }
}
