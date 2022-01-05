using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialballWebAPI.Migrations
{
    public partial class MatchPlayersMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchEvents_Matches_MatchId",
                table: "MatchEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchEvents_Teams",
                table: "MatchEvents");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchGoals_Players1",
                table: "MatchEvents");

            migrationBuilder.DropIndex(
                name: "IX_MatchEvents_MatchId",
                table: "MatchEvents");

            migrationBuilder.DropIndex(
                name: "IX_MatchEvents_PlayerId",
                table: "MatchEvents");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "MatchEvents");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "MatchEvents");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "MatchEvents",
                newName: "MatchPlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchEvents_TeamId",
                table: "MatchEvents",
                newName: "IX_MatchEvents_MatchPlayerId");

            migrationBuilder.CreateTable(
                name: "MatchPlayers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchPlayers_Matches",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchPlayers_Players",
                        column: x => x.PlayerId,
                        principalTable: "UserDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MatchPlayers_Teams",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayers_MatchId",
                table: "MatchPlayers",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayers_PlayerId",
                table: "MatchPlayers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayers_TeamId",
                table: "MatchPlayers",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchEvents_MatchPlayers",
                table: "MatchEvents",
                column: "MatchPlayerId",
                principalTable: "MatchPlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchEvents_MatchPlayers",
                table: "MatchEvents");

            migrationBuilder.DropTable(
                name: "MatchPlayers");

            migrationBuilder.RenameColumn(
                name: "MatchPlayerId",
                table: "MatchEvents",
                newName: "TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchEvents_MatchPlayerId",
                table: "MatchEvents",
                newName: "IX_MatchEvents_TeamId");

            migrationBuilder.AddColumn<Guid>(
                name: "MatchId",
                table: "MatchEvents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerId",
                table: "MatchEvents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MatchEvents_MatchId",
                table: "MatchEvents",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchEvents_PlayerId",
                table: "MatchEvents",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchEvents_Matches_MatchId",
                table: "MatchEvents",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchEvents_Teams",
                table: "MatchEvents",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchGoals_Players1",
                table: "MatchEvents",
                column: "PlayerId",
                principalTable: "UserDatas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
