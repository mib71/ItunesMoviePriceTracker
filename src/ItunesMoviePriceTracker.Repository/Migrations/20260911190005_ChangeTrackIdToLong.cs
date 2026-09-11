using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItunesMoviePriceTracker.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTrackIdToLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key constraint first
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Movies_MovieTrackId",
                table: "Prices");

            // Drop the primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_Movies",
                table: "Movies");

            // Alter the TrackId column in Movies table first
            migrationBuilder.AlterColumn<long>(
                name: "TrackId",
                table: "Movies",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            // Then alter the MovieTrackId column in Prices table
            migrationBuilder.AlterColumn<long>(
                name: "MovieTrackId",
                table: "Prices",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            // Recreate the primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_Movies",
                table: "Movies",
                column: "TrackId");

            // Recreate the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Movies_MovieTrackId",
                table: "Prices",
                column: "MovieTrackId",
                principalTable: "Movies",
                principalColumn: "TrackId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key constraint first
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Movies_MovieTrackId",
                table: "Prices");

            // Drop the primary key
            migrationBuilder.DropPrimaryKey(
                name: "PK_Movies",
                table: "Movies");

            // Alter the TrackId column in Movies table first
            migrationBuilder.AlterColumn<int>(
                name: "TrackId",
                table: "Movies",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            // Then alter the MovieTrackId column in Prices table
            migrationBuilder.AlterColumn<int>(
                name: "MovieTrackId",
                table: "Prices",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            // Recreate the primary key
            migrationBuilder.AddPrimaryKey(
                name: "PK_Movies",
                table: "Movies",
                column: "TrackId");

            // Recreate the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Movies_MovieTrackId",
                table: "Prices",
                column: "MovieTrackId",
                principalTable: "Movies",
                principalColumn: "TrackId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
