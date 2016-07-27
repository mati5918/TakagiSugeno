using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TakagiSugeno.Migrations
{
    public partial class DbInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Systems",
                columns: table => new
                {
                    TSSystemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AndMethod = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems", x => x.TSSystemId);
                });

            migrationBuilder.CreateTable(
                name: "InputsOutputs",
                columns: table => new
                {
                    TSInputOutputId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<int>(nullable: false),
                    TSSystemId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputsOutputs", x => x.TSInputOutputId);
                    table.ForeignKey(
                        name: "FK_InputsOutputs_Systems_TSSystemId",
                        column: x => x.TSSystemId,
                        principalTable: "Systems",
                        principalColumn: "TSSystemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rules",
                columns: table => new
                {
                    TSRuleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TSSystemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rules", x => x.TSRuleId);
                    table.ForeignKey(
                        name: "FK_Rules_Systems_TSSystemId",
                        column: x => x.TSSystemId,
                        principalTable: "Systems",
                        principalColumn: "TSSystemId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Variables",
                columns: table => new
                {
                    TSVariableId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    TSInputOutputId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Variables", x => x.TSVariableId);
                    table.ForeignKey(
                        name: "FK_Variables_InputsOutputs_TSInputOutputId",
                        column: x => x.TSInputOutputId,
                        principalTable: "InputsOutputs",
                        principalColumn: "TSInputOutputId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RuleElements",
                columns: table => new
                {
                    TSRuleElementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TSInputOutputId = table.Column<int>(nullable: false),
                    TSRuleId = table.Column<int>(nullable: false),
                    TSVariableId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleElements", x => x.TSRuleElementId);
                    table.ForeignKey(
                        name: "FK_RuleElements_InputsOutputs_TSInputOutputId",
                        column: x => x.TSInputOutputId,
                        principalTable: "InputsOutputs",
                        principalColumn: "TSInputOutputId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RuleElements_Rules_TSRuleId",
                        column: x => x.TSRuleId,
                        principalTable: "Rules",
                        principalColumn: "TSRuleId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RuleElements_Variables_TSVariableId",
                        column: x => x.TSVariableId,
                        principalTable: "Variables",
                        principalColumn: "TSVariableId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InputsOutputs_TSSystemId",
                table: "InputsOutputs",
                column: "TSSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Rules_TSSystemId",
                table: "Rules",
                column: "TSSystemId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Variables_TSInputOutputId",
                table: "Variables",
                column: "TSInputOutputId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RuleElements");

            migrationBuilder.DropTable(
                name: "Rules");

            migrationBuilder.DropTable(
                name: "Variables");

            migrationBuilder.DropTable(
                name: "InputsOutputs");

            migrationBuilder.DropTable(
                name: "Systems");
        }
    }
}
