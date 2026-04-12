using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BettingSystem.Migrations
{
    /// <inheritdoc />
    public partial class SeedGames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GameInformations",
                columns: new[] { "GameID", "GameDescription", "GameName" },
                values: new object[,]
                {
                    { 1, "A game of blackjack...", "Blackjack" },
                    { 2, "Roll the dice...", "Dices" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameInformations",
                keyColumn: "GameID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GameInformations",
                keyColumn: "GameID",
                keyValue: 2);
        }
    }
}
