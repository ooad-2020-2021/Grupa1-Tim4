﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace UETFA.Data.Migrations
{
    public partial class MigSud : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Sudija",
                table: "Utakmica",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sudija",
                table: "Utakmica");
        }
    }
}
