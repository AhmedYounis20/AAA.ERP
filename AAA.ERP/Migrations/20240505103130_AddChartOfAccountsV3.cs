using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class AddChartOfAccountsV3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountGuidId",
                table: "ChartOfAccounts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                .Annotation("Relational:ColumnOrder", 5);

            migrationBuilder.AddColumn<int>(
                name: "AccountNature",
                table: "ChartOfAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "ChartOfAccounts",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 7);

            migrationBuilder.AddColumn<bool>(
                name: "IsDepreciable",
                table: "ChartOfAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPostedAccount",
                table: "ChartOfAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsStopActive",
                table: "ChartOfAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsStopDealing",
                table: "ChartOfAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b23a5fb-a2ed-4860-9863-8cb5f3322996",
                column: "ConcurrencyStamp",
                value: "ce19946d-6286-4237-923d-e37c3997362a");

            migrationBuilder.CreateIndex(
                name: "IX_ChartOfAccounts_AccountGuidId",
                table: "ChartOfAccounts",
                column: "AccountGuidId");

            migrationBuilder.CreateIndex(
                name: "IX_ChartOfAccounts_Code",
                table: "ChartOfAccounts",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ChartOfAccounts_AccountGuides_AccountGuidId",
                table: "ChartOfAccounts",
                column: "AccountGuidId",
                principalTable: "AccountGuides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChartOfAccounts_AccountGuides_AccountGuidId",
                table: "ChartOfAccounts");

            migrationBuilder.DropIndex(
                name: "IX_ChartOfAccounts_AccountGuidId",
                table: "ChartOfAccounts");

            migrationBuilder.DropIndex(
                name: "IX_ChartOfAccounts_Code",
                table: "ChartOfAccounts");

            migrationBuilder.DropColumn(
                name: "AccountGuidId",
                table: "ChartOfAccounts");

            migrationBuilder.DropColumn(
                name: "AccountNature",
                table: "ChartOfAccounts");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "ChartOfAccounts");

            migrationBuilder.DropColumn(
                name: "IsDepreciable",
                table: "ChartOfAccounts");

            migrationBuilder.DropColumn(
                name: "IsPostedAccount",
                table: "ChartOfAccounts");

            migrationBuilder.DropColumn(
                name: "IsStopActive",
                table: "ChartOfAccounts");

            migrationBuilder.DropColumn(
                name: "IsStopDealing",
                table: "ChartOfAccounts");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8b23a5fb-a2ed-4860-9863-8cb5f3322996",
                column: "ConcurrencyStamp",
                value: "ae1ebb8a-cf7e-413b-a57f-c861c304c2f4");
        }
    }
}
