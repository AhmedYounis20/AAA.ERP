using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class AddFinancialPeriodIdToInventoryTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "InventoryTransactions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 8)
                .OldAnnotation("Relational:ColumnOrder", 7);

            migrationBuilder.AddColumn<Guid>(
                name: "FinancialPeriodId",
                table: "InventoryTransactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                .Annotation("Relational:ColumnOrder", 7);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_FinancialPeriodId",
                table: "InventoryTransactions",
                column: "FinancialPeriodId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactions_FinancialPeriods_FinancialPeriodId",
                table: "InventoryTransactions",
                column: "FinancialPeriodId",
                principalTable: "FinancialPeriods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactions_FinancialPeriods_FinancialPeriodId",
                table: "InventoryTransactions");

            migrationBuilder.DropIndex(
                name: "IX_InventoryTransactions_FinancialPeriodId",
                table: "InventoryTransactions");

            migrationBuilder.DropColumn(
                name: "FinancialPeriodId",
                table: "InventoryTransactions");

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "InventoryTransactions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 7)
                .OldAnnotation("Relational:ColumnOrder", 8);
        }
    }
}
