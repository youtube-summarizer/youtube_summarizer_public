using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace YtbSummarizer.Api.Migrations
{
    /// <inheritdoc />
    public partial class Toate_entitatile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Processed1D",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Processed1H",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Processed1W",
                table: "Videos");

            migrationBuilder.DropColumn(
                name: "Processed4H",
                table: "Videos");

            migrationBuilder.AddColumn<string>(
                name: "ChannelId",
                table: "Videos",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChannelId = table.Column<string>(type: "text", nullable: true),
                    ChannelTitle = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInteractedVideos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    VideoId = table.Column<string>(type: "text", nullable: true),
                    Score = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInteractedVideos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserVideos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    VideoId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVideos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "UserInteractedVideos");

            migrationBuilder.DropTable(
                name: "UserVideos");

            migrationBuilder.DropColumn(
                name: "ChannelId",
                table: "Videos");

            migrationBuilder.AddColumn<bool>(
                name: "Processed1D",
                table: "Videos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Processed1H",
                table: "Videos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Processed1W",
                table: "Videos",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Processed4H",
                table: "Videos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
