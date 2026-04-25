using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RetroAchievCollection.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "configuration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ApiKey = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_configuration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "consoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CodeIntegration = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Company = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_consoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ConsoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CodeIntegration = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Publisher = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Developer = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Genre = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    IsFavorite = table.Column<bool>(type: "INTEGER", nullable: false),
                    PlayCommand = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    ImagePath = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_games_consoles_ConsoleId",
                        column: x => x.ConsoleId,
                        principalTable: "consoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "achievements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CodeIntegration = table.Column<int>(type: "INTEGER", nullable: false),
                    GameId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ImagePath = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_achievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_achievements_games_GameId",
                        column: x => x.GameId,
                        principalTable: "games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_achievements_CodeIntegration",
                table: "achievements",
                column: "CodeIntegration",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_achievements_GameId",
                table: "achievements",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_consoles_CodeIntegration",
                table: "consoles",
                column: "CodeIntegration",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_games_CodeIntegration",
                table: "games",
                column: "CodeIntegration",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_games_ConsoleId",
                table: "games",
                column: "ConsoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "achievements");

            migrationBuilder.DropTable(
                name: "configuration");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "consoles");
        }
    }
}
