using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class AddGLSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aacdf2b5-0f9a-4977-a086-ffee465c238f");

            migrationBuilder.CreateTable(
                name: "GLSettings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAllowingEditVoucher = table.Column<bool>(type: "bit", nullable: false),
                    IsAllowingDeleteVoucher = table.Column<bool>(type: "bit", nullable: false),
                    IsAllowingNegativeBalances = table.Column<bool>(type: "bit", nullable: false),
                    DecimalDigitsNumber = table.Column<byte>(type: "tinyint", nullable: false),
                    DepreciationApplication = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MonthDays = table.Column<byte>(type: "tinyint", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d296e9a8-5a17-4b28-bdfb-825391908970", "95394177-9cbf-4d23-bd6a-365478add137", "ADMIN", "ADMIN" });

            migrationBuilder.InsertData(
                table: "GLSettings",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "DecimalDigitsNumber", "DepreciationApplication", "IsAllowingDeleteVoucher", "IsAllowingEditVoucher", "IsAllowingNegativeBalances", "ModifiedAt", "ModifiedBy", "MonthDays", "Notes" },
                values: new object[] { new Guid("2e6ab5eb-3ffd-4835-909e-6adf3e783909"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, (byte)0, "WithYearClosed", false, false, false, null, null, (byte)0, null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GLSettings");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d296e9a8-5a17-4b28-bdfb-825391908970");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "aacdf2b5-0f9a-4977-a086-ffee465c238f", "59b9e8f1-8461-4808-aa03-19d75f1bbcbb", "ADMIN", "ADMIN" });
        }
    }
}
