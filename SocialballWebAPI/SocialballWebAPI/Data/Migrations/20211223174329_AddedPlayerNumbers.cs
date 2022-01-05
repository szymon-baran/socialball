using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialballWebAPI.Migrations
{
    public partial class AddedPlayerNumbers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "MatchPlayers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                table: "MatchPlayers");
        }
    }
}
