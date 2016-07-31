using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TakagiSugeno.Migrations
{
    public partial class Rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RuleElements_InputsOutputs_TSInputOutputId",
                table: "RuleElements");

            migrationBuilder.DropForeignKey(
                name: "FK_RuleElements_Rules_TSRuleId",
                table: "RuleElements");

            migrationBuilder.DropForeignKey(
                name: "FK_RuleElements_Variables_TSVariableId",
                table: "RuleElements");

            migrationBuilder.DropForeignKey(
                name: "FK_Variables_InputsOutputs_TSInputOutputId",
                table: "Variables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Variables",
                table: "Variables");

            migrationBuilder.DropIndex(
                name: "IX_Variables_TSInputOutputId",
                table: "Variables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RuleElements",
                table: "RuleElements");

            migrationBuilder.DropIndex(
                name: "IX_RuleElements_TSInputOutputId",
                table: "RuleElements");

            migrationBuilder.DropIndex(
                name: "IX_RuleElements_TSRuleId",
                table: "RuleElements");

            migrationBuilder.DropIndex(
                name: "IX_RuleElements_TSVariableId",
                table: "RuleElements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rules",
                table: "Rules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InputsOutputs",
                table: "InputsOutputs");

            migrationBuilder.DropColumn(
                name: "TSVariableId",
                table: "Variables");

            migrationBuilder.DropColumn(
                name: "TSInputOutputId",
                table: "Variables");

            migrationBuilder.DropColumn(
                name: "TSRuleElementId",
                table: "RuleElements");

            migrationBuilder.DropColumn(
                name: "TSInputOutputId",
                table: "RuleElements");

            migrationBuilder.DropColumn(
                name: "TSRuleId",
                table: "RuleElements");

            migrationBuilder.DropColumn(
                name: "TSVariableId",
                table: "RuleElements");

            migrationBuilder.DropColumn(
                name: "TSRuleId",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "TSInputOutputId",
                table: "InputsOutputs");

            migrationBuilder.AddColumn<int>(
                name: "VariableId",
                table: "Variables",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "InputOutputId",
                table: "Variables",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RuleElementId",
                table: "RuleElements",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "InputOutputId",
                table: "RuleElements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RuleId",
                table: "RuleElements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VariableId",
                table: "RuleElements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RuleId",
                table: "Rules",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "InputOutputId",
                table: "InputsOutputs",
                nullable: false)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Variables",
                table: "Variables",
                column: "VariableId");

            migrationBuilder.CreateIndex(
                name: "IX_Variables_InputOutputId",
                table: "Variables",
                column: "InputOutputId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RuleElements",
                table: "RuleElements",
                column: "RuleElementId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleElements_InputOutputId",
                table: "RuleElements",
                column: "InputOutputId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleElements_RuleId",
                table: "RuleElements",
                column: "RuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleElements_VariableId",
                table: "RuleElements",
                column: "VariableId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rules",
                table: "Rules",
                column: "RuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InputsOutputs",
                table: "InputsOutputs",
                column: "InputOutputId");

            migrationBuilder.AddForeignKey(
                name: "FK_RuleElements_InputsOutputs_InputOutputId",
                table: "RuleElements",
                column: "InputOutputId",
                principalTable: "InputsOutputs",
                principalColumn: "InputOutputId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_RuleElements_Rules_RuleId",
                table: "RuleElements",
                column: "RuleId",
                principalTable: "Rules",
                principalColumn: "RuleId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_RuleElements_Variables_VariableId",
                table: "RuleElements",
                column: "VariableId",
                principalTable: "Variables",
                principalColumn: "VariableId",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Variables_InputsOutputs_InputOutputId",
                table: "Variables",
                column: "InputOutputId",
                principalTable: "InputsOutputs",
                principalColumn: "InputOutputId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RuleElements_InputsOutputs_InputOutputId",
                table: "RuleElements");

            migrationBuilder.DropForeignKey(
                name: "FK_RuleElements_Rules_RuleId",
                table: "RuleElements");

            migrationBuilder.DropForeignKey(
                name: "FK_RuleElements_Variables_VariableId",
                table: "RuleElements");

            migrationBuilder.DropForeignKey(
                name: "FK_Variables_InputsOutputs_InputOutputId",
                table: "Variables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Variables",
                table: "Variables");

            migrationBuilder.DropIndex(
                name: "IX_Variables_InputOutputId",
                table: "Variables");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RuleElements",
                table: "RuleElements");

            migrationBuilder.DropIndex(
                name: "IX_RuleElements_InputOutputId",
                table: "RuleElements");

            migrationBuilder.DropIndex(
                name: "IX_RuleElements_RuleId",
                table: "RuleElements");

            migrationBuilder.DropIndex(
                name: "IX_RuleElements_VariableId",
                table: "RuleElements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rules",
                table: "Rules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InputsOutputs",
                table: "InputsOutputs");

            migrationBuilder.DropColumn(
                name: "VariableId",
                table: "Variables");

            migrationBuilder.DropColumn(
                name: "InputOutputId",
                table: "Variables");

            migrationBuilder.DropColumn(
                name: "RuleElementId",
                table: "RuleElements");

            migrationBuilder.DropColumn(
                name: "InputOutputId",
                table: "RuleElements");

            migrationBuilder.DropColumn(
                name: "RuleId",
                table: "RuleElements");

            migrationBuilder.DropColumn(
                name: "VariableId",
                table: "RuleElements");

            migrationBuilder.DropColumn(
                name: "RuleId",
                table: "Rules");

            migrationBuilder.DropColumn(
                name: "InputOutputId",
                table: "InputsOutputs");

            migrationBuilder.AddColumn<int>(
                name: "TSVariableId",
                table: "Variables",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "TSInputOutputId",
                table: "Variables",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TSRuleElementId",
                table: "RuleElements",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "TSInputOutputId",
                table: "RuleElements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TSRuleId",
                table: "RuleElements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TSVariableId",
                table: "RuleElements",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TSRuleId",
                table: "Rules",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "TSInputOutputId",
                table: "InputsOutputs",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Variables",
                table: "Variables",
                column: "TSVariableId");

            migrationBuilder.CreateIndex(
                name: "IX_Variables_TSInputOutputId",
                table: "Variables",
                column: "TSInputOutputId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RuleElements",
                table: "RuleElements",
                column: "TSRuleElementId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleElements_TSInputOutputId",
                table: "RuleElements",
                column: "TSInputOutputId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleElements_TSRuleId",
                table: "RuleElements",
                column: "TSRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleElements_TSVariableId",
                table: "RuleElements",
                column: "TSVariableId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rules",
                table: "Rules",
                column: "TSRuleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InputsOutputs",
                table: "InputsOutputs",
                column: "TSInputOutputId");

            migrationBuilder.AddForeignKey(
                name: "FK_RuleElements_InputsOutputs_TSInputOutputId",
                table: "RuleElements",
                column: "TSInputOutputId",
                principalTable: "InputsOutputs",
                principalColumn: "TSInputOutputId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RuleElements_Rules_TSRuleId",
                table: "RuleElements",
                column: "TSRuleId",
                principalTable: "Rules",
                principalColumn: "TSRuleId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RuleElements_Variables_TSVariableId",
                table: "RuleElements",
                column: "TSVariableId",
                principalTable: "Variables",
                principalColumn: "TSVariableId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Variables_InputsOutputs_TSInputOutputId",
                table: "Variables",
                column: "TSInputOutputId",
                principalTable: "InputsOutputs",
                principalColumn: "TSInputOutputId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
