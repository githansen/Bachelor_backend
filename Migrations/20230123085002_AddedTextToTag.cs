using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bachelorbackend.Migrations
{
    /// <inheritdoc />
    public partial class AddedTextToTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "text",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "text",
                table: "Tags");
        }
    }
}
