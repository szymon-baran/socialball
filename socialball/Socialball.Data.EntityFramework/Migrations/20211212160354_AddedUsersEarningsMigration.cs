using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialballWebAPI.Migrations
{
    public partial class AddedUsersEarningsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Earnings",
                table: "UserDatas",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlayerEarnings",
                table: "PlayerTransferOffers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Earnings",
                table: "JobAdvertisements",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Earnings",
                table: "UserDatas");

            migrationBuilder.DropColumn(
                name: "PlayerEarnings",
                table: "PlayerTransferOffers");

            migrationBuilder.DropColumn(
                name: "Earnings",
                table: "JobAdvertisements");
        }
    }
}
