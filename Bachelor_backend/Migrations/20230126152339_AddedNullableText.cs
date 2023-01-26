using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bachelorbackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedNullableText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audiofiles_Texts_TextId",
                table: "Audiofiles");

            migrationBuilder.AlterColumn<int>(
                name: "TextId",
                table: "Audiofiles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Audiofiles_Texts_TextId",
                table: "Audiofiles",
                column: "TextId",
                principalTable: "Texts",
                principalColumn: "TextId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Audiofiles_Texts_TextId",
                table: "Audiofiles");

            migrationBuilder.AlterColumn<int>(
                name: "TextId",
                table: "Audiofiles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Audiofiles_Texts_TextId",
                table: "Audiofiles",
                column: "TextId",
                principalTable: "Texts",
                principalColumn: "TextId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
