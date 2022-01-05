using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialballWebAPI.Migrations
{
    public partial class MessageRebuildMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessages_Users",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_TeamMessages_Teams",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ToTeamId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ToUserId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ToTeamId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ToUserId",
                table: "Messages");

            migrationBuilder.CreateTable(
                name: "UserMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    ToUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMessages_Messages",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMessages_Users",
                        column: x => x.ToUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_MessageId",
                table: "UserMessages",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessages_ToUserId",
                table: "UserMessages",
                column: "ToUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMessages");

            migrationBuilder.AddColumn<Guid>(
                name: "ToTeamId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ToUserId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ToTeamId",
                table: "Messages",
                column: "ToTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ToUserId",
                table: "Messages",
                column: "ToUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessages_Users",
                table: "Messages",
                column: "ToUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMessages_Teams",
                table: "Messages",
                column: "ToTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
