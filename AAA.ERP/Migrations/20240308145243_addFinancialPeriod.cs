using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class addFinancialPeriod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountGuides");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d296e9a8-5a17-4b28-bdfb-825391908970");

            migrationBuilder.DeleteData(
                table: "GLSettings",
                keyColumn: "Id",
                keyValue: new Guid("2e6ab5eb-3ffd-4835-909e-6adf3e783909"));

            migrationBuilder.CreateTable(
                name: "FinancialPeriods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    YearNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PeriodTypeByMonth = table.Column<byte>(type: "tinyint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialPeriods", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8b23a5fb-a2ed-4860-9863-8cb5f3322996", "f38f5af4-364b-4a85-8c6d-207b935d4263", "ADMIN", "ADMIN" });

            migrationBuilder.InsertData(
                table: "GLSettings",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DecimalDigitsNumber", "DepreciationApplication", "IsAllowingDeleteVoucher", "IsAllowingEditVoucher", "IsAllowingNegativeBalances", "ModifiedAt", "ModifiedBy", "MonthDays", "Notes" },
                values: new object[] { new Guid("9ae5291c-e983-49c4-b72a-8524ea10a2bb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)0, "WithYearClosed", false, false, false, null, null, (byte)0, null });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialPeriods_YearNumber",
                table: "FinancialPeriods",
                column: "YearNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialPeriods");

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
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGuides", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d296e9a8-5a17-4b28-bdfb-825391908970", "95394177-9cbf-4d23-bd6a-365478add137", "ADMIN", "ADMIN" });

            migrationBuilder.InsertData(
                table: "GLSettings",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DecimalDigitsNumber", "DepreciationApplication", "IsAllowingDeleteVoucher", "IsAllowingEditVoucher", "IsAllowingNegativeBalances", "ModifiedAt", "ModifiedBy", "MonthDays", "Notes" },
                values: new object[] { new Guid("2e6ab5eb-3ffd-4835-909e-6adf3e783909"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)0, "WithYearClosed", false, false, false, null, null, (byte)0, null });

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
    }
}
