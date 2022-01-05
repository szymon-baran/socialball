using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialballWebAPI.Migrations
{
    public partial class GoalsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwayGoals",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "HomeGoals",
                table: "Matches");

            migrationBuilder.AddColumn<string>(
                name: "Stadium",
                table: "Matches",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ScorerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssistPlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Minute = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goals_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Goals_Players",
                        column: x => x.AssistPlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Goals_Players1",
                        column: x => x.ScorerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Goals_AssistPlayerId",
                table: "Goals",
                column: "AssistPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_MatchId",
                table: "Goals",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_ScorerId",
                table: "Goals",
                column: "ScorerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropColumn(
                name: "Stadium",
                table: "Matches");

            migrationBuilder.AddColumn<int>(
                name: "AwayGoals",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HomeGoals",
                table: "Matches",
                type: "int",
                nullable: true);
        }
    }
}
