using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialballWebAPI.Migrations
{
    public partial class PlayerTransferOfferAddedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "IsInjuredUntil",
                table: "UserDatas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlayerTransferOffers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FromTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ToTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransferFee = table.Column<int>(type: "int", nullable: false),
                    IsAcceptedByPlayer = table.Column<bool>(type: "bit", nullable: false),
                    IsAcceptedByOtherTeam = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerTransferOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FromTeamPlayerTransferOffers_Teams",
                        column: x => x.FromTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerTransferOffers_Players",
                        column: x => x.PlayerId,
                        principalTable: "UserDatas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ToTeamPlayerTransferOffers_Teams",
                        column: x => x.ToTeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTransferOffers_FromTeamId",
                table: "PlayerTransferOffers",
                column: "FromTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTransferOffers_PlayerId",
                table: "PlayerTransferOffers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerTransferOffers_ToTeamId",
                table: "PlayerTransferOffers",
                column: "ToTeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerTransferOffers");

            migrationBuilder.DropColumn(
                name: "IsInjuredUntil",
                table: "UserDatas");
        }
    }
}
