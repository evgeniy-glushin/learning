using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class FixWarTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wars_AspNetUsers_Player1Id1",
                table: "Wars");

            migrationBuilder.DropForeignKey(
                name: "FK_Wars_AspNetUsers_Player2Id1",
                table: "Wars");

            migrationBuilder.DropIndex(
                name: "IX_Wars_Player1Id1",
                table: "Wars");

            migrationBuilder.DropIndex(
                name: "IX_Wars_Player2Id1",
                table: "Wars");

            migrationBuilder.DropColumn(
                name: "Player1Id1",
                table: "Wars");

            migrationBuilder.DropColumn(
                name: "Player2Id1",
                table: "Wars");

            migrationBuilder.AlterColumn<string>(
                name: "Player2Id",
                table: "Wars",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Player1Id",
                table: "Wars",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Wars_Player1Id",
                table: "Wars",
                column: "Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Wars_Player2Id",
                table: "Wars",
                column: "Player2Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wars_AspNetUsers_Player1Id",
                table: "Wars",
                column: "Player1Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wars_AspNetUsers_Player2Id",
                table: "Wars",
                column: "Player2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wars_AspNetUsers_Player1Id",
                table: "Wars");

            migrationBuilder.DropForeignKey(
                name: "FK_Wars_AspNetUsers_Player2Id",
                table: "Wars");

            migrationBuilder.DropIndex(
                name: "IX_Wars_Player1Id",
                table: "Wars");

            migrationBuilder.DropIndex(
                name: "IX_Wars_Player2Id",
                table: "Wars");

            migrationBuilder.AddColumn<string>(
                name: "Player1Id1",
                table: "Wars",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Player2Id1",
                table: "Wars",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Player2Id",
                table: "Wars",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Player1Id",
                table: "Wars",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Wars_Player1Id1",
                table: "Wars",
                column: "Player1Id1");

            migrationBuilder.CreateIndex(
                name: "IX_Wars_Player2Id1",
                table: "Wars",
                column: "Player2Id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Wars_AspNetUsers_Player1Id1",
                table: "Wars",
                column: "Player1Id1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wars_AspNetUsers_Player2Id1",
                table: "Wars",
                column: "Player2Id1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
