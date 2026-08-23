using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexilearn.MySql.Migrations
{
    /// <inheritdoc />
    public partial class _04_AnkiCompatibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename PracticeSessionCards -> CardReviews in place instead of drop+recreate,
            // so existing review history survives the migration.
            migrationBuilder.RenameTable(
                name: "PracticeSessionCards",
                newName: "CardReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_PracticeSessionCards_Cards_CardId",
                table: "CardReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_PracticeSessionCards_PracticeSessions_SessionId",
                table: "CardReviews");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "CardReviews",
                newName: "Rating");

            migrationBuilder.RenameIndex(
                name: "IX_PracticeSessionCards_CardId",
                table: "CardReviews",
                newName: "IX_CardReviews_CardId");

            migrationBuilder.RenameIndex(
                name: "IX_PracticeSessionCards_SessionId",
                table: "CardReviews",
                newName: "IX_CardReviews_SessionId");

            migrationBuilder.AddColumn<DateTime>(
                name: "ReviewedAt",
                table: "CardReviews",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP(6)");

            migrationBuilder.AddColumn<int>(
                name: "PreviousStatus",
                table: "CardReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NextStatus",
                table: "CardReviews",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "PreviousReviewAt",
                table: "CardReviews",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NextReviewAt",
                table: "CardReviews",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResponseTimeMilliseconds",
                table: "CardReviews",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CardReviews_Cards_CardId",
                table: "CardReviews",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CardReviews_PracticeSessions_SessionId",
                table: "CardReviews",
                column: "SessionId",
                principalTable: "PracticeSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddColumn<string>(
                name: "Example",
                table: "Cards",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ExternalId",
                table: "Cards",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ImportSource",
                table: "Cards",
                type: "varchar(30)",
                maxLength: 30,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_ImportSource_ExternalId",
                table: "Cards",
                columns: new[] { "ImportSource", "ExternalId" },
                unique: true);

            migrationBuilder.CreateTable(
                name: "CardSchedulingStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CardId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    NextReviewAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastReviewedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ReviewCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LapseCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    IntervalDays = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardSchedulingStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CardSchedulingStates_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CardSchedulingStates_CardId",
                table: "CardSchedulingStates",
                column: "CardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardSchedulingStates_Status_NextReviewAt",
                table: "CardSchedulingStates",
                columns: new[] { "Status", "NextReviewAt" });

            // Backfill a scheduling state for every card that already exists, so the
            // Card -> CardSchedulingState relationship is never null for pre-existing rows.
            migrationBuilder.Sql(
                "INSERT INTO CardSchedulingStates (CardId, Status, ReviewCount, LapseCount, IntervalDays) " +
                "SELECT c.Id, 0, 0, 0, 0 FROM Cards c " +
                "WHERE NOT EXISTS (SELECT 1 FROM CardSchedulingStates s WHERE s.CardId = c.Id);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardSchedulingStates");

            migrationBuilder.DropIndex(
                name: "IX_Cards_ImportSource_ExternalId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "Example",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ExternalId",
                table: "Cards");

            migrationBuilder.DropColumn(
                name: "ImportSource",
                table: "Cards");

            migrationBuilder.DropForeignKey(
                name: "FK_CardReviews_Cards_CardId",
                table: "CardReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_CardReviews_PracticeSessions_SessionId",
                table: "CardReviews");

            migrationBuilder.DropColumn(
                name: "ReviewedAt",
                table: "CardReviews");

            migrationBuilder.DropColumn(
                name: "PreviousStatus",
                table: "CardReviews");

            migrationBuilder.DropColumn(
                name: "NextStatus",
                table: "CardReviews");

            migrationBuilder.DropColumn(
                name: "PreviousReviewAt",
                table: "CardReviews");

            migrationBuilder.DropColumn(
                name: "NextReviewAt",
                table: "CardReviews");

            migrationBuilder.DropColumn(
                name: "ResponseTimeMilliseconds",
                table: "CardReviews");

            migrationBuilder.RenameIndex(
                name: "IX_CardReviews_CardId",
                table: "CardReviews",
                newName: "IX_PracticeSessionCards_CardId");

            migrationBuilder.RenameIndex(
                name: "IX_CardReviews_SessionId",
                table: "CardReviews",
                newName: "IX_PracticeSessionCards_SessionId");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "CardReviews",
                newName: "Status");

            migrationBuilder.RenameTable(
                name: "CardReviews",
                newName: "PracticeSessionCards");

            migrationBuilder.AddForeignKey(
                name: "FK_PracticeSessionCards_Cards_CardId",
                table: "PracticeSessionCards",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PracticeSessionCards_PracticeSessions_SessionId",
                table: "PracticeSessionCards",
                column: "SessionId",
                principalTable: "PracticeSessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
