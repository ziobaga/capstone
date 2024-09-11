using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Capstone.Migrations
{
    /// <inheritdoc />
    public partial class addreviewsfield : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FieldsId",
                table: "Reviews",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_FieldsId",
                table: "Reviews",
                column: "FieldsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Fields_FieldsId",
                table: "Reviews",
                column: "FieldsId",
                principalTable: "Fields",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Fields_FieldsId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_FieldsId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "FieldsId",
                table: "Reviews");
        }
    }
}
