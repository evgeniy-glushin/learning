using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class FixInvitationTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_AspNetUsers_InvitedId1",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_AspNetUsers_InviterId1",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_InvitedId1",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_InviterId1",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "InvitedId1",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "InviterId1",
                table: "Invitations");

            migrationBuilder.AlterColumn<string>(
                name: "InviterId",
                table: "Invitations",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "InvitedId",
                table: "Invitations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_InvitedId",
                table: "Invitations",
                column: "InvitedId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_InviterId",
                table: "Invitations",
                column: "InviterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_AspNetUsers_InvitedId",
                table: "Invitations",
                column: "InvitedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_AspNetUsers_InviterId",
                table: "Invitations",
                column: "InviterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_AspNetUsers_InvitedId",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Invitations_AspNetUsers_InviterId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_InvitedId",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_InviterId",
                table: "Invitations");

            migrationBuilder.AddColumn<string>(
                name: "InvitedId1",
                table: "Invitations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InviterId1",
                table: "Invitations",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InviterId",
                table: "Invitations",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "InvitedId",
                table: "Invitations",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_InvitedId1",
                table: "Invitations",
                column: "InvitedId1");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_InviterId1",
                table: "Invitations",
                column: "InviterId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_AspNetUsers_InvitedId1",
                table: "Invitations",
                column: "InvitedId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invitations_AspNetUsers_InviterId1",
                table: "Invitations",
                column: "InviterId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
