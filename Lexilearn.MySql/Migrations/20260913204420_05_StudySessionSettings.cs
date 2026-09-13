using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexilearn.MySql.Migrations
{
    /// <inheritdoc />
    public partial class _05_StudySessionSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudySessionSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DeckId = table.Column<int>(type: "int", nullable: false),
                    SessionSize = table.Column<int>(type: "int", nullable: false, defaultValue: 20),
                    NewCardsPercentage = table.Column<int>(type: "int", nullable: false, defaultValue: 20),
                    HardCardsPercentage = table.Column<int>(type: "int", nullable: false, defaultValue: 30),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudySessionSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudySessionSettings_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_StudySessionSettings_DeckId",
                table: "StudySessionSettings",
                column: "DeckId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudySessionSettings");
        }
    }
}
