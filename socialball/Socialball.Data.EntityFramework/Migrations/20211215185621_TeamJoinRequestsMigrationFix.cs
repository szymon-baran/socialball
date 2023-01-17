using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialballWebAPI.Migrations
{
    public partial class TeamJoinRequestsMigrationFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobAdvertisementTeamAnswers_Teams",
                table: "JobAdvertisementAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_JobAdvertisementAnswers_Teams",
                table: "JobAdvertisementAnswers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobAdvertisementAnswers_Teams",
                table: "JobAdvertisementAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK_JobAdvertisementTeamAnswers_Teams",
                table: "JobAdvertisementAnswers",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
