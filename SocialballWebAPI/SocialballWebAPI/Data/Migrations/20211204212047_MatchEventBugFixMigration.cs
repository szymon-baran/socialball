using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialballWebAPI.Migrations
{
    public partial class MatchEventBugFixMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TeamId",
                table: "MatchEvents",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MatchEvents_TeamId",
                table: "MatchEvents",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchEvents_Teams",
                table: "MatchEvents",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchEvents_Teams",
                table: "MatchEvents");

            migrationBuilder.DropIndex(
                name: "IX_MatchEvents_TeamId",
                table: "MatchEvents");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "MatchEvents");
        }
    }
}
