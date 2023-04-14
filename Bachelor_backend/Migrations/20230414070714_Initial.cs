using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bachelorbackend.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagId);
                });

            migrationBuilder.CreateTable(
                name: "TargetGroups",
                columns: table => new
                {
                    Targetid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Genders = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Languages = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dialects = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgeGroups = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetGroups", x => x.Targetid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NativeLanguage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgeGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dialect = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Texts",
                columns: table => new
                {
                    TextId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetGroupTargetid = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Texts", x => x.TextId);
                    table.ForeignKey(
                        name: "FK_Texts_TargetGroups_TargetGroupTargetid",
                        column: x => x.TargetGroupTargetid,
                        principalTable: "TargetGroups",
                        principalColumn: "Targetid");
                });

            migrationBuilder.CreateTable(
                name: "Audiofiles",
                columns: table => new
                {
                    UUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TextId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audiofiles", x => x.UUID);
                    table.ForeignKey(
                        name: "FK_Audiofiles_Texts_TextId",
                        column: x => x.TextId,
                        principalTable: "Texts",
                        principalColumn: "TextId");
                    table.ForeignKey(
                        name: "FK_Audiofiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "TagsForTexts",
                columns: table => new
                {
                    TagsTagId = table.Column<int>(type: "int", nullable: false),
                    TextsTextId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagsForTexts", x => new { x.TagsTagId, x.TextsTextId });
                    table.ForeignKey(
                        name: "FK_TagsForTexts_Tags_TagsTagId",
                        column: x => x.TagsTagId,
                        principalTable: "Tags",
                        principalColumn: "TagId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagsForTexts_Texts_TextsTextId",
                        column: x => x.TextsTextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Audiofiles_TextId",
                table: "Audiofiles",
                column: "TextId");

            migrationBuilder.CreateIndex(
                name: "IX_Audiofiles_UserId",
                table: "Audiofiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TagsForTexts_TextsTextId",
                table: "TagsForTexts",
                column: "TextsTextId");

            migrationBuilder.CreateIndex(
                name: "IX_Texts_TargetGroupTargetid",
                table: "Texts",
                column: "TargetGroupTargetid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Audiofiles");

            migrationBuilder.DropTable(
                name: "TagsForTexts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Texts");

            migrationBuilder.DropTable(
                name: "TargetGroups");
        }
    }
}
