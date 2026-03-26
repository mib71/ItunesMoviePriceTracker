using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItunesMoviePriceTracker.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    TrackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackName = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ArtistName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    LongDescription = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ArtworkUrl60 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ArtworkUrl400 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrackHdPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastChecked = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WatchPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.TrackId);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MovieTrackId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_Movies_MovieTrackId",
                        column: x => x.MovieTrackId,
                        principalTable: "Movies",
                        principalColumn: "TrackId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Prices_MovieTrackId",
                table: "Prices",
                column: "MovieTrackId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
