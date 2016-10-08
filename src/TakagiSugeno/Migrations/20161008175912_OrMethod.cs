using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TakagiSugeno.Migrations
{
    public partial class OrMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrMethod",
                table: "Systems",
                nullable: false,
                defaultValue: TakagiSugeno.Model.OrMethod.Max);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrMethod",
                table: "Systems");
        }
    }
}
