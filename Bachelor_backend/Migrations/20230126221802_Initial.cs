using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bachelorbackend.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Texts",
                columns: table => new
                {
                    TextId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TextText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Texts", x => x.TextId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NativeLanguage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AgeGroup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dialect = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
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

            migrationBuilder.CreateTable(
                name: "Audiofiles",
                columns: table => new
                {
                    UUID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    TextId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audiofiles", x => x.UUID);
                    table.ForeignKey(
                        name: "FK_Audiofiles_Texts_TextId",
                        column: x => x.TextId,
                        principalTable: "Texts",
                        principalColumn: "TextId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Audiofiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "TagId", "TagText" },
                values: new object[,]
                {
                    { 1, "Narkotika" },
                    { 2, "Kniv" }
                });

            migrationBuilder.InsertData(
                table: "Texts",
                columns: new[] { "TextId", "TextText" },
                values: new object[,]
                {
                    { 1, "Jeg dør for mine bros og de dør for meg\r\nFakk alle hater jeg sverger jeg er lei\r\nBitches prøvde å teste de lo men nå\r\nLer jeg\r\nNgrn switcha opp det ække mitt problem det var din feil\r\nJeg sa jeg har det bra\r\nIkke ødeleg ditt liv det var det mamma sa\r\nMen jeg er oppvokst i et helvete det er der jeg er fra\r\nStarta ung som rapper og jeg vet hvor jeg skal\r\nJeg skaffa gode læg med min trapphone\r\nMine niggas vi er fly for jeg satser nå\r\nMamma please don't cry, vi skal nå opp til topp\r\nKommer ikke hjem med hull i klærne lenger nå\r\nHva hadde skjedd om jeg var hvit (mamma)\r\nIkke politisaker om jeg var hvit (mamma)\r\nTil og med skole hadde gått fint (mamma)\r\nDet er derfor de ser fuck politi (fuck dem Haha/mamma)\r\nHaterne snakker luft ja de snakker shit\r\n16 år og allerede opplevd nok i mitt liv\r\nKaren var 10 år første gang karen løp fra politi\r\nMamma gråter i et avhør hun vet ingenting\r\nFuck politi ja fuck politi\r\nDe vet ikke hvem du er men de fakker opp din liv\r\nHud fargen har tydeligvis mye å si\r\nBryr seg 0 om din family 0 om din familly\r\nFikk ikke jobb så jeg valgte å trappe\r\nFakk alle disse haterne jeg gjør mitt beste\r\nJeg shower off en bil og kommentar feltet fullt av den ække din men fuck dem neger for dem hakke vært i mine værdager\r\nJeg hadde flere planer men mange av dem gikk galt\r\nArtist planen er den eneste som går som den skal\r\nJeg mista min bestemor, unkel og min bestefar\r\nAlle tårene gjorde det vanskelig å sove på natta\r\nAlt jeg har gjort jeg har gjort det på\r\nEgenhånd\r\nEneste jeg kunne lene meg på var min unkel bro\r\nPoliti venter i buskene men jeg løpet som bolt\r\nIngen kan ta min familly det beste jg har fått\r\nJeg er flink til å late som ingenting har skjedd\r\nDet er bare mine close ones som jeg snakker med\r\nJeg ække en morder men ikke test meg neger\r\nJeg ække en morder men ikke test meg neger\r\nFuck politi ja fuck politi\r\nDe vet ikke hvem du er men de fakker opp din liv\r\nHud fargen har tydeligvis mye å si\r\nBryr seg 0 om din family 0 om din familly" },
                    { 2, "Jeg har Bakke kontakt jeg lar dem fly\r\nDu hakke sett en shit lil boy hold tyst\r\nBle født i et mørkt sted men jeg fant lys\r\nJeg har bitches overalt nå boy i alle byer\r\nFor her i hooden det går fast\r\nVarer går fram og tilbake neger vi gjør task\r\nLeker du big man bitch du får slapp\r\nFakk hva de sier rambow starta fra scratch (hæ)\r\nFor jeg har gjort tusenvis av feil\r\nMen jeg har lært av dem alle jeg holder meg til samme spor\r\nHaterne for snakke for jeg blikke lei\r\nRambow on the track det er fortsatt fire in the booth\r\nJeg vil bare catche green green (green green)\r\nIngen av oss hadde læg tro meg vi levde tungt\r\nJeg vil bare catche ti på ti på ti\r\nMine brødre i gata hakke jobb de catcher floos\r\nOps ved vår blokk de fåkke bli\r\nJeg følger ikke andres sti jeg lager min\r\nFuck haters nigga vi gjør vår ting\r\nDu sier du er best lil boy stop å lyv\r\nDu leker g til du møter på veggen\r\nVi lekte politi og tyv det ække lek lenger\r\nHadde mange før nå har jeg 3 venner\r\nFordi fake ble til real og real ble til fake neger\r\nPappa lærte meg trust får deg drept\r\nVanskelig å lære men tok step by step\r\nFikk fame nå er alle sammen glad i meg igjen\r\nVi ække brødre eller day ones bitch ass hold kjeft\r\nNå vil alle bli bros for real\r\nHold deg unna bitch stop tråkke på min tå\r\nNå har jeg hoes og freaky bitches\r\nJeg ække g men jeg trykker om jeg må\r\nJeg vil bare catche green green (green green)\r\nIngen av oss hadde læg tro meg vi levde tungt\r\nJeg vil bare catche ti på ti på ti\r\nMine brødre i gata hakke jobb de catcher floos" }
                });
            migrationBuilder.InsertData(
                table: "TagsForTexts",
                columns: new[] {"TagsTagId", "TextsTextId"},
                values: new object[,]
                {
                    { 1,1}, {1,2}, {2,1}, {2,2 } }
                ); 
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
