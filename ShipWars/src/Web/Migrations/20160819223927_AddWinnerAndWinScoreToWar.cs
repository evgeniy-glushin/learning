using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class AddWinnerAndWinScoreToWar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WinScore",
                table: "Wars",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WinnerId",
                table: "Wars",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wars_WinnerId",
                table: "Wars",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wars_AspNetUsers_WinnerId",
                table: "Wars",
                column: "WinnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wars_AspNetUsers_WinnerId",
                table: "Wars");

            migrationBuilder.DropIndex(
                name: "IX_Wars_WinnerId",
                table: "Wars");

            migrationBuilder.DropColumn(
                name: "WinScore",
                table: "Wars");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "Wars");
        }
    }
}
