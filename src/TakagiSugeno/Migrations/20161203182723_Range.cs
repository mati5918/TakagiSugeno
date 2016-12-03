using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TakagiSugeno.Migrations
{
    public partial class Range : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "RangeEnd",
                table: "InputsOutputs",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RangeStart",
                table: "InputsOutputs",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RangeEnd",
                table: "InputsOutputs");

            migrationBuilder.DropColumn(
                name: "RangeStart",
                table: "InputsOutputs");
        }
    }
}
