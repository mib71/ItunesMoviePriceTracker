using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItunesMoviePriceTracker.Repository.Migrations
{
    /// <inheritdoc />
    public partial class MakeTrackNameRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // No default value: rows without a title must fail the migration
            // rather than silently get an empty title.
            migrationBuilder.AlterColumn<string>(
                name: "TrackName",
                table: "Movies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TrackName",
                table: "Movies",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }
    }
}
