using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class update_financialTransaction_DeleteEntryId1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialTransactions_Entries_EntryId1",
                table: "FinancialTransactions");

            migrationBuilder.DropIndex(
                name: "IX_FinancialTransactions_EntryId1",
                table: "FinancialTransactions");

            migrationBuilder.DropColumn(
                name: "EntryId1",
                table: "FinancialTransactions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EntryId1",
                table: "FinancialTransactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactions_EntryId1",
                table: "FinancialTransactions",
                column: "EntryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialTransactions_Entries_EntryId1",
                table: "FinancialTransactions",
                column: "EntryId1",
                principalTable: "Entries",
                principalColumn: "Id");
        }
    }
}
