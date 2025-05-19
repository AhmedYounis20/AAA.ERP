using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class add_packingUnits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PackingUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingUnits", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackingUnits_Name",
                table: "PackingUnits",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PackingUnits_NameSecondLanguage",
                table: "PackingUnits",
                column: "NameSecondLanguage",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackingUnits");
        }
    }
}
