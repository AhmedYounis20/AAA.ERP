using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class update_entryExchangeRateName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_FinancialPeriods_FinancialPeriodId1",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_FinancialPeriodId1",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "FinancialPeriodId1",
                table: "Entries");

            migrationBuilder.RenameColumn(
                name: "ExchageRate",
                table: "Entries",
                newName: "ExchangeRate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExchangeRate",
                table: "Entries",
                newName: "ExchageRate");

            migrationBuilder.AddColumn<Guid>(
                name: "FinancialPeriodId1",
                table: "Entries",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Entries_FinancialPeriodId1",
                table: "Entries",
                column: "FinancialPeriodId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_FinancialPeriods_FinancialPeriodId1",
                table: "Entries",
                column: "FinancialPeriodId1",
                principalTable: "FinancialPeriods",
                principalColumn: "Id");
        }
    }
}
