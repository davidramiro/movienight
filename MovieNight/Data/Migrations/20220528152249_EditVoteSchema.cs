using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieNight.Data.Migrations
{
    public partial class EditVoteSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_AspNetUsers_UserId",
                table: "Movie");

            migrationBuilder.DropForeignKey(
                name: "FK_Suggestion_AspNetUsers_userId",
                table: "Suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_Suggestion_Movie_movieId",
                table: "Suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_AspNetUsers_userId",
                table: "Vote");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Suggestion_suggestionId",
                table: "Vote");

            migrationBuilder.DropIndex(
                name: "IX_Movie_UserId",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Movie");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Vote",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "suggestionId",
                table: "Vote",
                newName: "SuggestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Vote_userId",
                table: "Vote",
                newName: "IX_Vote_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Vote_suggestionId",
                table: "Vote",
                newName: "IX_Vote_SuggestionId");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Suggestion",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "movieId",
                table: "Suggestion",
                newName: "MovieId");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Suggestion",
                newName: "Date");

            migrationBuilder.RenameIndex(
                name: "IX_Suggestion_userId",
                table: "Suggestion",
                newName: "IX_Suggestion_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Suggestion_movieId",
                table: "Suggestion",
                newName: "IX_Suggestion_MovieId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Vote",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestion_AspNetUsers_UserId",
                table: "Suggestion",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestion_Movie_MovieId",
                table: "Suggestion",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_AspNetUsers_UserId",
                table: "Vote",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Suggestion_SuggestionId",
                table: "Vote",
                column: "SuggestionId",
                principalTable: "Suggestion",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Suggestion_AspNetUsers_UserId",
                table: "Suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_Suggestion_Movie_MovieId",
                table: "Suggestion");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_AspNetUsers_UserId",
                table: "Vote");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Suggestion_SuggestionId",
                table: "Vote");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Vote");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Vote",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "SuggestionId",
                table: "Vote",
                newName: "suggestionId");

            migrationBuilder.RenameIndex(
                name: "IX_Vote_UserId",
                table: "Vote",
                newName: "IX_Vote_userId");

            migrationBuilder.RenameIndex(
                name: "IX_Vote_SuggestionId",
                table: "Vote",
                newName: "IX_Vote_suggestionId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Suggestion",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "MovieId",
                table: "Suggestion",
                newName: "movieId");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Suggestion",
                newName: "date");

            migrationBuilder.RenameIndex(
                name: "IX_Suggestion_UserId",
                table: "Suggestion",
                newName: "IX_Suggestion_userId");

            migrationBuilder.RenameIndex(
                name: "IX_Suggestion_MovieId",
                table: "Suggestion",
                newName: "IX_Suggestion_movieId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Movie",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movie_UserId",
                table: "Movie",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_AspNetUsers_UserId",
                table: "Movie",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestion_AspNetUsers_userId",
                table: "Suggestion",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suggestion_Movie_movieId",
                table: "Suggestion",
                column: "movieId",
                principalTable: "Movie",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_AspNetUsers_userId",
                table: "Vote",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Suggestion_suggestionId",
                table: "Vote",
                column: "suggestionId",
                principalTable: "Suggestion",
                principalColumn: "Id");
        }
    }
}
