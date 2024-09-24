using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capstone.Migrations
{
    /// <inheritdoc />
    public partial class NoListImgField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImmaginiCampo",
                table: "Fields");

            migrationBuilder.AddColumn<string>(
                name: "ImmagineCampo",
                table: "Fields",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImmagineCampo",
                table: "Fields");

            migrationBuilder.AddColumn<string>(
                name: "ImmaginiCampo",
                table: "Fields",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
