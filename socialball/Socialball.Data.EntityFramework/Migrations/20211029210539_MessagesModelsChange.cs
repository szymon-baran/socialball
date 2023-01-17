using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialballWebAPI.Migrations
{
    public partial class MessagesModelsChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamMessages_Users",
                table: "TeamMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamMessages",
                table: "TeamMessages");

            migrationBuilder.RenameTable(
                name: "TeamMessages",
                newName: "Messages");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Messages",
                newName: "ToUserId");

            migrationBuilder.RenameColumn(
                name: "TeamId",
                table: "Messages",
                newName: "ToTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamMessages_UserId",
                table: "Messages",
                newName: "IX_Messages_ToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_TeamMessages_TeamId",
                table: "Messages",
                newName: "IX_Messages_ToTeamId");

            migrationBuilder.AddColumn<Guid>(
                name: "FromUserId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MessageType",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FromUserId",
                table: "Messages",
                column: "FromUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users",
                table: "Messages",
                column: "FromUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateMessages_Users",
                table: "Messages",
                column: "ToUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateMessages_Users",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_FromUserId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "FromUserId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MessageType",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "TeamMessages");

            migrationBuilder.RenameColumn(
                name: "ToUserId",
                table: "TeamMessages",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ToTeamId",
                table: "TeamMessages",
                newName: "TeamId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ToUserId",
                table: "TeamMessages",
                newName: "IX_TeamMessages_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ToTeamId",
                table: "TeamMessages",
                newName: "IX_TeamMessages_TeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamMessages",
                table: "TeamMessages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamMessages_Users",
                table: "TeamMessages",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
