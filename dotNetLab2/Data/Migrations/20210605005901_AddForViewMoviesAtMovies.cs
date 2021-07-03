using Microsoft.EntityFrameworkCore.Migrations;

namespace dotNetLab2.Migrations
{
    public partial class AddForViewMoviesAtMovies : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_ForViewMovies_ForViewMovieId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_ForViewMovieId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "ForViewMovieId",
                table: "Movies");

            migrationBuilder.CreateTable(
                name: "ForViewMovieMovie",
                columns: table => new
                {
                    ForViewMoviesId = table.Column<int>(type: "int", nullable: false),
                    MoviesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForViewMovieMovie", x => new { x.ForViewMoviesId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_ForViewMovieMovie_ForViewMovies_ForViewMoviesId",
                        column: x => x.ForViewMoviesId,
                        principalTable: "ForViewMovies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ForViewMovieMovie_Movies_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForViewMovieMovie_MoviesId",
                table: "ForViewMovieMovie",
                column: "MoviesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForViewMovieMovie");

            migrationBuilder.AddColumn<int>(
                name: "ForViewMovieId",
                table: "Movies",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_ForViewMovieId",
                table: "Movies",
                column: "ForViewMovieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_ForViewMovies_ForViewMovieId",
                table: "Movies",
                column: "ForViewMovieId",
                principalTable: "ForViewMovies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
