using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieNight.Data.Migrations
{
    public partial class AddTournamentBracket : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tournament",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Running = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Finished = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CurrentRound = table.Column<int>(type: "int", nullable: false),
                    HighestBracket = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MovieBracket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BracketNumber = table.Column<int>(type: "int", nullable: false),
                    TournamentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieBracket", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieBracket_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MoviePair",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TopSuggestionId = table.Column<int>(type: "int", nullable: false),
                    BottomSuggestionId = table.Column<int>(type: "int", nullable: false),
                    winnerFound = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    winnerTop = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MovieBracketId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoviePair", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoviePair_MovieBracket_MovieBracketId",
                        column: x => x.MovieBracketId,
                        principalTable: "MovieBracket",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MoviePair_Suggestion_BottomSuggestionId",
                        column: x => x.BottomSuggestionId,
                        principalTable: "Suggestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoviePair_Suggestion_TopSuggestionId",
                        column: x => x.TopSuggestionId,
                        principalTable: "Suggestion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_MovieBracket_TournamentId",
                table: "MovieBracket",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviePair_BottomSuggestionId",
                table: "MoviePair",
                column: "BottomSuggestionId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviePair_MovieBracketId",
                table: "MoviePair",
                column: "MovieBracketId");

            migrationBuilder.CreateIndex(
                name: "IX_MoviePair_TopSuggestionId",
                table: "MoviePair",
                column: "TopSuggestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoviePair");

            migrationBuilder.DropTable(
                name: "MovieBracket");

            migrationBuilder.DropTable(
                name: "Tournament");
        }
    }
}
