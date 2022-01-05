using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialballWebAPI.Migrations
{
    public partial class JobAdvertisementsAddedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FromUserJobAdvertisementId",
                table: "Leagues",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobAdvertisements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    JobAdvertisementType = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LowestEarningsPerMonth = table.Column<int>(type: "int", nullable: true),
                    HighestEarningsPerMonth = table.Column<int>(type: "int", nullable: true),
                    TrainingSessionsPerWeek = table.Column<int>(type: "int", nullable: true),
                    Position = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobAdvertisements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FromTeamJobAdvertisements_Teams",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FromUserJobAdvertisements_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "JobAdvertisementAnswers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    JobAdvertisementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobAdvertisementAnswerType = table.Column<int>(type: "int", nullable: false),
                    IsPositive = table.Column<bool>(type: "bit", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobAdvertisementAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobAdvertisementAnswers_JobAdvertisements_JobAdvertisementId",
                        column: x => x.JobAdvertisementId,
                        principalTable: "JobAdvertisements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobAdvertisementTeamAnswers_Teams",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_JobAdvertisementUserAnswers_Users",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_FromUserJobAdvertisementId",
                table: "Leagues",
                column: "FromUserJobAdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisementAnswers_JobAdvertisementId",
                table: "JobAdvertisementAnswers",
                column: "JobAdvertisementId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisementAnswers_TeamId",
                table: "JobAdvertisementAnswers",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisementAnswers_UserId",
                table: "JobAdvertisementAnswers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisements_TeamId",
                table: "JobAdvertisements",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_JobAdvertisements_UserId",
                table: "JobAdvertisements",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leagues_JobAdvertisements_FromUserJobAdvertisementId",
                table: "Leagues",
                column: "FromUserJobAdvertisementId",
                principalTable: "JobAdvertisements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leagues_JobAdvertisements_FromUserJobAdvertisementId",
                table: "Leagues");

            migrationBuilder.DropTable(
                name: "JobAdvertisementAnswers");

            migrationBuilder.DropTable(
                name: "JobAdvertisements");

            migrationBuilder.DropIndex(
                name: "IX_Leagues_FromUserJobAdvertisementId",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "FromUserJobAdvertisementId",
                table: "Leagues");
        }
    }
}
