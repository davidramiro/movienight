using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieNight.Data.Migrations
{
    public partial class AddVotesAndSuggestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Suggestion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    userId = table.Column<string>(type: "TEXT", nullable: true),
                    movieId = table.Column<int>(type: "INTEGER", nullable: true),
                    date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suggestion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suggestion_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Suggestion_Movie_movieId",
                        column: x => x.movieId,
                        principalTable: "Movie",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Vote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    userId = table.Column<string>(type: "TEXT", nullable: true),
                    suggestionId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vote_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vote_Suggestion_suggestionId",
                        column: x => x.suggestionId,
                        principalTable: "Suggestion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Suggestion_movieId",
                table: "Suggestion",
                column: "movieId");

            migrationBuilder.CreateIndex(
                name: "IX_Suggestion_userId",
                table: "Suggestion",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_suggestionId",
                table: "Vote",
                column: "suggestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_userId",
                table: "Vote",
                column: "userId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vote");

            migrationBuilder.DropTable(
                name: "Suggestion");
        }
    }
}
