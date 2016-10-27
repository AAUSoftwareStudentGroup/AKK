using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AKK.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sections", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    ColorOfHolds = table.Column<uint>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Grade = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SectionID = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Routes_Sections_SectionID",
                        column: x => x.SectionID,
                        principalTable: "Sections",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Routes_SectionID",
                table: "Routes",
                column: "SectionID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Sections");
        }
    }
}
