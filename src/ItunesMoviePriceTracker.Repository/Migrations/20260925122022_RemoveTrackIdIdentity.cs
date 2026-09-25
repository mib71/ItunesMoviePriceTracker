using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ItunesMoviePriceTracker.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTrackIdIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // SQL Server cannot remove IDENTITY with ALTER COLUMN, so the table is rebuilt.
            // Databases created before EF migrations never had IDENTITY on TrackId,
            // so the rebuild only runs where the column actually is an identity column.
            migrationBuilder.Sql("""
                IF COLUMNPROPERTY(OBJECT_ID(N'dbo.Movies'), N'TrackId', 'IsIdentity') = 1
                BEGIN
                    ALTER TABLE [dbo].[Prices] DROP CONSTRAINT [FK_Prices_Movies_MovieTrackId];

                    CREATE TABLE [dbo].[Movies_New] (
                        [TrackId] bigint NOT NULL,
                        [TrackName] nvarchar(500) NULL,
                        [ReleaseDate] datetime2 NULL,
                        [ArtistName] nvarchar(255) NULL,
                        [LongDescription] nvarchar(2000) NULL,
                        [ArtworkUrl60] nvarchar(500) NULL,
                        [ArtworkUrl400] nvarchar(500) NULL,
                        [TrackHdPrice] decimal(18,2) NOT NULL,
                        [LastChecked] datetime2 NOT NULL,
                        [WatchPrice] decimal(18,2) NULL
                    );

                    INSERT INTO [dbo].[Movies_New]
                        ([TrackId], [TrackName], [ReleaseDate], [ArtistName], [LongDescription],
                         [ArtworkUrl60], [ArtworkUrl400], [TrackHdPrice], [LastChecked], [WatchPrice])
                    SELECT
                        [TrackId], [TrackName], [ReleaseDate], [ArtistName], [LongDescription],
                        [ArtworkUrl60], [ArtworkUrl400], [TrackHdPrice], [LastChecked], [WatchPrice]
                    FROM [dbo].[Movies];

                    DROP TABLE [dbo].[Movies];
                    EXEC sp_rename N'dbo.Movies_New', N'Movies';

                    ALTER TABLE [dbo].[Movies] ADD CONSTRAINT [PK_Movies] PRIMARY KEY ([TrackId]);
                    ALTER TABLE [dbo].[Prices] ADD CONSTRAINT [FK_Prices_Movies_MovieTrackId]
                        FOREIGN KEY ([MovieTrackId]) REFERENCES [dbo].[Movies] ([TrackId]) ON DELETE CASCADE;
                END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Intentionally empty. Re-adding IDENTITY would require another table rebuild
            // and would reintroduce the bug this migration fixes.
        }
    }
}
