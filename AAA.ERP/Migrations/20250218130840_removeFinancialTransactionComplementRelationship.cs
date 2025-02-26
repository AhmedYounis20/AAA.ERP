using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class removeFinancialTransactionComplementRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinancialTransactions_FinancialTransactions_ComplementTransactionId",
                table: "FinancialTransactions");

            migrationBuilder.DropIndex(
                name: "IX_FinancialTransactions_ComplementTransactionId",
                table: "FinancialTransactions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactions_ComplementTransactionId",
                table: "FinancialTransactions",
                column: "ComplementTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_FinancialTransactions_FinancialTransactions_ComplementTransactionId",
                table: "FinancialTransactions",
                column: "ComplementTransactionId",
                principalTable: "FinancialTransactions",
                principalColumn: "Id");
        }
    }
}
