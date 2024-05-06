using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class AddChartOfAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3db95f3-9297-4fab-b700-62dc754ef2f6");

            migrationBuilder.DeleteData(
                table: "GLSettings",
                keyColumn: "Id",
                keyValue: new Guid("7e43efdf-df7e-4a9b-afc0-402f02f0ceed"));

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "GLSettings");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "FinancialPeriods");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Currencies");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "AccountGuides");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8b23a5fb-a2ed-4860-9863-8cb5f3322996", "b10313b0-adb5-4526-9f82-f4022eab184f", "ADMIN", "ADMIN" });

            migrationBuilder.InsertData(
                table: "GLSettings",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DecimalDigitsNumber", "DepreciationApplication", "IsAllowingDeleteVoucher", "IsAllowingEditVoucher", "IsAllowingNegativeBalances", "ModifiedAt", "ModifiedBy", "MonthDays" },
                values: new object[] { new Guid("9ae5291c-e983-49c4-b72a-8524ea10a2bb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)0, "WithYearClosed", false, false, false, null, null, (byte)0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b23a5fb-a2ed-4860-9863-8cb5f3322996");

            migrationBuilder.DeleteData(
                table: "GLSettings",
                keyColumn: "Id",
                keyValue: new Guid("9ae5291c-e983-49c4-b72a-8524ea10a2bb"));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "GLSettings",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true)
                .Annotation("Relational:ColumnOrder", 7);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "FinancialPeriods",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true)
                .Annotation("Relational:ColumnOrder", 5);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Currencies",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true)
                .Annotation("Relational:ColumnOrder", 7);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "AccountGuides",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true)
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f3db95f3-9297-4fab-b700-62dc754ef2f6", "5a8d17d3-50bc-410d-ad95-5a1e5e8817e5", "ADMIN", "ADMIN" });

            migrationBuilder.InsertData(
                table: "GLSettings",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DecimalDigitsNumber", "DepreciationApplication", "IsAllowingDeleteVoucher", "IsAllowingEditVoucher", "IsAllowingNegativeBalances", "ModifiedAt", "ModifiedBy", "MonthDays", "Notes" },
                values: new object[] { new Guid("7e43efdf-df7e-4a9b-afc0-402f02f0ceed"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)0, "WithYearClosed", false, false, false, null, null, (byte)0, null });
        }
    }
}
