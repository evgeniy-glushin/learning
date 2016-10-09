using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class AlterWarTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Field",
                table: "Wars",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "WhoShotId",
                table: "Wars",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Wars_WhoShotId",
                table: "Wars",
                column: "WhoShotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wars_AspNetUsers_WhoShotId",
                table: "Wars",
                column: "WhoShotId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wars_AspNetUsers_WhoShotId",
                table: "Wars");

            migrationBuilder.DropIndex(
                name: "IX_Wars_WhoShotId",
                table: "Wars");

            migrationBuilder.DropColumn(
                name: "Field",
                table: "Wars");

            migrationBuilder.DropColumn(
                name: "WhoShotId",
                table: "Wars");
        }
    }
}
