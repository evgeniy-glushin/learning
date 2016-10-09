using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class UnapplyTheLastMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Wins",
                table: "AspNetUsers",
                nullable: false);

            migrationBuilder.AlterColumn<int>(
                name: "Loses",
                table: "AspNetUsers",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Wins",
                table: "AspNetUsers",
                nullable: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "Loses",
                table: "AspNetUsers",
                nullable: false);
        }
    }
}
