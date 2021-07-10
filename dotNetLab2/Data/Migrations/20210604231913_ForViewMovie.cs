using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dotNetLab2.Migrations
{
    public partial class ForViewMovie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ForViewMovieId",
                table: "Movies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ForViewMovies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WatchDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForViewMovies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForViewMovies_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ForViewMovieId",
                table: "Movies",
                column: "ForViewMovieId");

            migrationBuilder.CreateIndex(
                name: "IX_ForViewMovies_ApplicationUserId",
                table: "ForViewMovies",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_ForViewMovies_ForViewMovieId",
                table: "Movies",
                column: "ForViewMovieId",
                principalTable: "ForViewMovies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_ForViewMovies_ForViewMovieId",
                table: "Movies");

            migrationBuilder.DropTable(
                name: "ForViewMovies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_ForViewMovieId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ForViewMovieId",
                table: "Movies");
        }
    }
}
