using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class addAccountGuideV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b23a5fb-a2ed-4860-9863-8cb5f3322996");

            migrationBuilder.DeleteData(
                table: "GLSettings",
                keyColumn: "Id",
                keyValue: new Guid("9ae5291c-e983-49c4-b72a-8524ea10a2bb"));

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
                values: new object[] { "f3db95f3-9297-4fab-b700-62dc754ef2f6", "5a8d17d3-50bc-410d-ad95-5a1e5e8817e5", "ADMIN", "ADMIN" });

            migrationBuilder.InsertData(
                table: "GLSettings",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DecimalDigitsNumber", "DepreciationApplication", "IsAllowingDeleteVoucher", "IsAllowingEditVoucher", "IsAllowingNegativeBalances", "ModifiedAt", "ModifiedBy", "MonthDays", "Notes" },
                values: new object[] { new Guid("7e43efdf-df7e-4a9b-afc0-402f02f0ceed"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)0, "WithYearClosed", false, false, false, null, null, (byte)0, null });

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

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3db95f3-9297-4fab-b700-62dc754ef2f6");

            migrationBuilder.DeleteData(
                table: "GLSettings",
                keyColumn: "Id",
                keyValue: new Guid("7e43efdf-df7e-4a9b-afc0-402f02f0ceed"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8b23a5fb-a2ed-4860-9863-8cb5f3322996", "f38f5af4-364b-4a85-8c6d-207b935d4263", "ADMIN", "ADMIN" });

            migrationBuilder.InsertData(
                table: "GLSettings",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DecimalDigitsNumber", "DepreciationApplication", "IsAllowingDeleteVoucher", "IsAllowingEditVoucher", "IsAllowingNegativeBalances", "ModifiedAt", "ModifiedBy", "MonthDays", "Notes" },
                values: new object[] { new Guid("9ae5291c-e983-49c4-b72a-8524ea10a2bb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)0, "WithYearClosed", false, false, false, null, null, (byte)0, null });
        }
    }
}
