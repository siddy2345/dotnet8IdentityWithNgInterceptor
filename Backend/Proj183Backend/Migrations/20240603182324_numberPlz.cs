using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proj183Backend.Migrations
{
    /// <inheritdoc />
    public partial class numberPlz : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Number",
                table: "Address",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PLZ",
                table: "Address",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "PLZ",
                table: "Address");
        }
    }
}
