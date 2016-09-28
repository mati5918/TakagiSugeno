using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using TakagiSugeno.Model;

namespace TakagiSugeno.Migrations
{
    public partial class RulesUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RuleElements_Variables_VariableId",
                table: "RuleElements");

            migrationBuilder.AddColumn<bool>(
                name: "IsNegation",
                table: "RuleElements",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NextOpartion",
                table: "RuleElements",
                nullable: false,
                defaultValue: RuleNextOperation.And);

            migrationBuilder.AlterColumn<int>(
                name: "VariableId",
                table: "RuleElements",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RuleElements_Variables_VariableId",
                table: "RuleElements",
                column: "VariableId",
                principalTable: "Variables",
                principalColumn: "VariableId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RuleElements_Variables_VariableId",
                table: "RuleElements");

            migrationBuilder.DropColumn(
                name: "IsNegation",
                table: "RuleElements");

            migrationBuilder.DropColumn(
                name: "NextOpartion",
                table: "RuleElements");

            migrationBuilder.AlterColumn<int>(
                name: "VariableId",
                table: "RuleElements",
                nullable: false);

            migrationBuilder.AddForeignKey(
                name: "FK_RuleElements_Variables_VariableId",
                table: "RuleElements",
                column: "VariableId",
                principalTable: "Variables",
                principalColumn: "VariableId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
