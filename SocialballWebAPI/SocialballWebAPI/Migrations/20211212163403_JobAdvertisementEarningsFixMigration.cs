using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialballWebAPI.Migrations
{
    public partial class JobAdvertisementEarningsFixMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighestEarningsPerMonth",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "LowestEarningsPerMonth",
                table: "JobAdvertisements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HighestEarningsPerMonth",
                table: "JobAdvertisements",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LowestEarningsPerMonth",
                table: "JobAdvertisements",
                type: "int",
                nullable: true);
        }
    }
}
