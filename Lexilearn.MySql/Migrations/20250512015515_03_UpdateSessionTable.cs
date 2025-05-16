using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lexilearn.MySql.Migrations
{
    /// <inheritdoc />
    public partial class _03_UpdateSessionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PracticeSessions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PracticeSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
