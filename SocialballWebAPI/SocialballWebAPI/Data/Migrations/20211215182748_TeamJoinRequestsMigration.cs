using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialballWebAPI.Migrations
{
    public partial class TeamJoinRequestsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobAdvertisementAnswers_JobAdvertisements_JobAdvertisementId",
                table: "JobAdvertisementAnswers");

            migrationBuilder.AlterColumn<Guid>(
                name: "JobAdvertisementId",
                table: "JobAdvertisementAnswers",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_JobAdvertisementAnswers_JobAdvertisements_JobAdvertisementId",
                table: "JobAdvertisementAnswers",
                column: "JobAdvertisementId",
                principalTable: "JobAdvertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobAdvertisementAnswers_JobAdvertisements_JobAdvertisementId",
                table: "JobAdvertisementAnswers");

            migrationBuilder.AlterColumn<Guid>(
                name: "JobAdvertisementId",
                table: "JobAdvertisementAnswers",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_JobAdvertisementAnswers_JobAdvertisements_JobAdvertisementId",
                table: "JobAdvertisementAnswers",
                column: "JobAdvertisementId",
                principalTable: "JobAdvertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
