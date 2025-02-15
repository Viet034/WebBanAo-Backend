using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBanAoo.Migrations
{
    /// <inheritdoc />
    public partial class SizeCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SizeCode",
                table: "Sizes",
                type: "int",
                maxLength: 100,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SizeCode",
                table: "Sizes");
        }
    }
}
