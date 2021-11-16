using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialballWebAPI.Migrations
{
    public partial class JobAdvertisementsV2Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leagues_JobAdvertisements_FromUserJobAdvertisementId",
                table: "Leagues");

            migrationBuilder.DropIndex(
                name: "IX_Leagues_FromUserJobAdvertisementId",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "FromUserJobAdvertisementId",
                table: "Leagues");

            migrationBuilder.RenameColumn(
                name: "IsPositive",
                table: "JobAdvertisementAnswers",
                newName: "IsResponsePositive");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "JobAdvertisements",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ResponseContent",
                table: "JobAdvertisementAnswers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "JobAdvertisements");

            migrationBuilder.DropColumn(
                name: "ResponseContent",
                table: "JobAdvertisementAnswers");

            migrationBuilder.RenameColumn(
                name: "IsResponsePositive",
                table: "JobAdvertisementAnswers",
                newName: "IsPositive");

            migrationBuilder.AddColumn<Guid>(
                name: "FromUserJobAdvertisementId",
                table: "Leagues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_FromUserJobAdvertisementId",
                table: "Leagues",
                column: "FromUserJobAdvertisementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leagues_JobAdvertisements_FromUserJobAdvertisementId",
                table: "Leagues",
                column: "FromUserJobAdvertisementId",
                principalTable: "JobAdvertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
