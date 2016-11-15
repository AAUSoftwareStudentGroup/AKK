using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AKK.Migrations
{
    public partial class NameToGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Grades",
                columns: table => new
                {
                    GradeId = table.Column<Guid>(nullable: false),
                    ColorDB = table.Column<uint>(nullable: true),
                    Difficulty = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grades", x => x.GradeId);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    SectionId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.SectionId);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    RouteId = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    ColorOfHoldsDB = table.Column<uint>(nullable: true),
                    ColorOfTapeDB = table.Column<uint>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    GradeId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PendingDeletion = table.Column<bool>(nullable: false),
                    SectionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.RouteId);
                    table.ForeignKey(
                        name: "FK_Routes_Grades_GradeId",
                        column: x => x.GradeId,
                        principalTable: "Grades",
                        principalColumn: "GradeId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Routes_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "SectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_GradeId",
                table: "Routes",
                column: "GradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_SectionId",
                table: "Routes",
                column: "SectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Grades");

            migrationBuilder.DropTable(
                name: "Sections");
        }
    }
}
