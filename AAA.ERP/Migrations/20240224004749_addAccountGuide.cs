using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class addAccountGuide : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Currency",
                table: "Currency");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1cc9213-3267-44b3-b475-c23dde69a7d7");

            migrationBuilder.RenameTable(
                name: "Currency",
                newName: "Currencies");

            migrationBuilder.RenameIndex(
                name: "IX_Currency_Symbol",
                table: "Currencies",
                newName: "IX_Currencies_Symbol");

            migrationBuilder.RenameIndex(
                name: "IX_Currency_NameSecondLanguage",
                table: "Currencies",
                newName: "IX_Currencies_NameSecondLanguage");

            migrationBuilder.RenameIndex(
                name: "IX_Currency_Name",
                table: "Currencies",
                newName: "IX_Currencies_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AccountGuides",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGuides", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "aacdf2b5-0f9a-4977-a086-ffee465c238f", "59b9e8f1-8461-4808-aa03-19d75f1bbcbb", "ADMIN", "ADMIN" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountGuides_Name",
                table: "AccountGuides",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountGuides_NameSecondLanguage",
                table: "AccountGuides",
                column: "NameSecondLanguage",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountGuides");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Currencies",
                table: "Currencies");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aacdf2b5-0f9a-4977-a086-ffee465c238f");

            migrationBuilder.RenameTable(
                name: "Currencies",
                newName: "Currency");

            migrationBuilder.RenameIndex(
                name: "IX_Currencies_Symbol",
                table: "Currency",
                newName: "IX_Currency_Symbol");

            migrationBuilder.RenameIndex(
                name: "IX_Currencies_NameSecondLanguage",
                table: "Currency",
                newName: "IX_Currency_NameSecondLanguage");

            migrationBuilder.RenameIndex(
                name: "IX_Currencies_Name",
                table: "Currency",
                newName: "IX_Currency_Name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Currency",
                table: "Currency",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a1cc9213-3267-44b3-b475-c23dde69a7d7", "244424ed-d6ea-41ca-83ae-d2b468cdfdc0", "ADMIN", "ADMIN" });
        }
    }
}
