using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    /// <inheritdoc />
    public partial class AddEntrySourceDocumentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactionItems_Items_ItemId",
                table: "InventoryTransactionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransferItems_Items_ItemId",
                table: "InventoryTransferItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockBalances_Items_ItemId",
                table: "StockBalances");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "StockBalances",
                newName: "VariantId");

            migrationBuilder.RenameIndex(
                name: "IX_StockBalances_ItemId_PackingUnitId_BranchId",
                table: "StockBalances",
                newName: "IX_StockBalances_VariantId_PackingUnitId_BranchId");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "InventoryTransferItems",
                newName: "VariantId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryTransferItems_ItemId",
                table: "InventoryTransferItems",
                newName: "IX_InventoryTransferItems_VariantId");

            migrationBuilder.RenameColumn(
                name: "ItemId",
                table: "InventoryTransactionItems",
                newName: "VariantId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryTransactionItems_ItemId",
                table: "InventoryTransactionItems",
                newName: "IX_InventoryTransactionItems_VariantId");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverName",
                table: "Entries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 11)
                .OldAnnotation("Relational:ColumnOrder", 9);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Entries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 12)
                .OldAnnotation("Relational:ColumnOrder", 10);

            migrationBuilder.AlterColumn<Guid>(
                name: "FinancialPeriodId",
                table: "Entries",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 7)
                .OldAnnotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeRate",
                table: "Entries",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)")
                .Annotation("Relational:ColumnOrder", 9)
                .OldAnnotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrencyId",
                table: "Entries",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 8)
                .OldAnnotation("Relational:ColumnOrder", 6);

            migrationBuilder.AlterColumn<Guid>(
                name: "BranchId",
                table: "Entries",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 10)
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AddColumn<Guid>(
                name: "SourceDocumentId",
                table: "Entries",
                type: "uniqueidentifier",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 5);

            migrationBuilder.AddColumn<string>(
                name: "SourceDocumentType",
                table: "Entries",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true)
                .Annotation("Relational:ColumnOrder", 6);

            migrationBuilder.CreateIndex(
                name: "IX_Entries_SourceDocumentType_SourceDocumentId",
                table: "Entries",
                columns: new[] { "SourceDocumentType", "SourceDocumentId" },
                unique: true,
                filter: "[SourceDocumentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactionItems_Variants_VariantId",
                table: "InventoryTransactionItems",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransferItems_Variants_VariantId",
                table: "InventoryTransferItems",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockBalances_Variants_VariantId",
                table: "StockBalances",
                column: "VariantId",
                principalTable: "Variants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransactionItems_Variants_VariantId",
                table: "InventoryTransactionItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryTransferItems_Variants_VariantId",
                table: "InventoryTransferItems");

            migrationBuilder.DropForeignKey(
                name: "FK_StockBalances_Variants_VariantId",
                table: "StockBalances");

            migrationBuilder.DropIndex(
                name: "IX_Entries_SourceDocumentType_SourceDocumentId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "SourceDocumentId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "SourceDocumentType",
                table: "Entries");

            migrationBuilder.RenameColumn(
                name: "VariantId",
                table: "StockBalances",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_StockBalances_VariantId_PackingUnitId_BranchId",
                table: "StockBalances",
                newName: "IX_StockBalances_ItemId_PackingUnitId_BranchId");

            migrationBuilder.RenameColumn(
                name: "VariantId",
                table: "InventoryTransferItems",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryTransferItems_VariantId",
                table: "InventoryTransferItems",
                newName: "IX_InventoryTransferItems_ItemId");

            migrationBuilder.RenameColumn(
                name: "VariantId",
                table: "InventoryTransactionItems",
                newName: "ItemId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryTransactionItems_VariantId",
                table: "InventoryTransactionItems",
                newName: "IX_InventoryTransactionItems_ItemId");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverName",
                table: "Entries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 9)
                .OldAnnotation("Relational:ColumnOrder", 11);

            migrationBuilder.AlterColumn<string>(
                name: "Notes",
                table: "Entries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 10)
                .OldAnnotation("Relational:ColumnOrder", 12);

            migrationBuilder.AlterColumn<Guid>(
                name: "FinancialPeriodId",
                table: "Entries",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 5)
                .OldAnnotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<decimal>(
                name: "ExchangeRate",
                table: "Entries",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)")
                .Annotation("Relational:ColumnOrder", 7)
                .OldAnnotation("Relational:ColumnOrder", 9);

            migrationBuilder.AlterColumn<Guid>(
                name: "CurrencyId",
                table: "Entries",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 6)
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<Guid>(
                name: "BranchId",
                table: "Entries",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 8)
                .OldAnnotation("Relational:ColumnOrder", 10);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransactionItems_Items_ItemId",
                table: "InventoryTransactionItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryTransferItems_Items_ItemId",
                table: "InventoryTransferItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockBalances_Items_ItemId",
                table: "StockBalances",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
