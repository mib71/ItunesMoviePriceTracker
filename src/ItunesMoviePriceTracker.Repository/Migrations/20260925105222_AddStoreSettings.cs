using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItunesMoviePriceTracker.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddStoreSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    CurrencyCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Culture = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    SelectedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreSettings", x => x.Id);
                    table.CheckConstraint("CK_StoreSettings_SingleRow", "[Id] = 1");
                });

            // Add as nullable, backfill, then make NOT NULL. No permanent default is left behind,
            // so code that forgets to set CountryCode fails loudly instead of silently writing 'se'.
            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Prices",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: true);

            // EXEC defers compilation, so the new column is visible when the migration
            // is run as a single-batch SQL script (e.g. in SSMS), not just via MigrateAsync().
            migrationBuilder.Sql("EXEC(N'UPDATE Prices SET CountryCode = ''se'' WHERE CountryCode IS NULL;');");

            migrationBuilder.AlterColumn<string>(
                name: "CountryCode",
                table: "Prices",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(2)",
                oldMaxLength: 2,
                oldNullable: true);

            // Existing installations keep running against the Swedish store without going through setup.
            // A fresh database has no movies, gets no row, and will be sent to setup.
            migrationBuilder.Sql("""
                INSERT INTO StoreSettings (Id, CountryCode, CurrencyCode, Culture, SelectedAt)
                SELECT 1, 'se', 'SEK', 'sv-SE', SYSUTCDATETIME()
                WHERE EXISTS (SELECT 1 FROM Movies);
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Prices");

            migrationBuilder.DropTable(
                name: "StoreSettings");
        }
    }
}