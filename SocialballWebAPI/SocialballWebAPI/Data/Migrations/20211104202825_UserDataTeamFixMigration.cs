using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialballWebAPI.Migrations
{
    public partial class UserDataTeamFixMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDatas_Teams",
                table: "UserDatas");

            migrationBuilder.CreateIndex(
                name: "IX_UserDatas_TeamId",
                table: "UserDatas",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDatas_Teams",
                table: "UserDatas",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserDatas_Teams",
                table: "UserDatas");

            migrationBuilder.DropIndex(
                name: "IX_UserDatas_TeamId",
                table: "UserDatas");

            migrationBuilder.AddForeignKey(
                name: "FK_UserDatas_Teams",
                table: "UserDatas",
                column: "UserId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
