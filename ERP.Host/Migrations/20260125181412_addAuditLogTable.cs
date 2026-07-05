using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class addAuditLogTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountNature",
                table: "FinancialTransactions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "EntryNumber",
                table: "Entries",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangedProperties = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IpAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    AdditionalInfo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CreatedAt",
                table: "Suppliers",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_StockBalances_CreatedAt",
                table: "StockBalances",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_CreatedAt",
                table: "Sizes",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SellingPrices_CreatedAt",
                table: "SellingPrices",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedAt",
                table: "Roles",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PackingUnits_CreatedAt",
                table: "PackingUnits",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturerCompanies_CreatedAt",
                table: "ManufacturerCompanies",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSuppliers_CreatedAt",
                table: "ItemSuppliers",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSellingPriceDiscounts_CreatedAt",
                table: "ItemSellingPriceDiscounts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CreatedAt",
                table: "Items",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Items_NodeType",
                table: "Items",
                column: "NodeType");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ParentId_NodeType",
                table: "Items",
                columns: new[] { "ParentId", "NodeType" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnitSellingPrices_CreatedAt",
                table: "ItemPackingUnitSellingPrices",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnits_CreatedAt",
                table: "ItemPackingUnits",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ItemManufacturerCompanies_CreatedAt",
                table: "ItemManufacturerCompanies",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCodes_CreatedAt",
                table: "ItemCodes",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactions_CreatedAt",
                table: "InventoryTransactions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactionItems_CreatedAt",
                table: "InventoryTransactionItems",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_GLSettings_CreatedAt",
                table: "GLSettings",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_CreatedAt",
                table: "FixedAssets",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactions_AccountNature",
                table: "FinancialTransactions",
                column: "AccountNature");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactions_CreatedAt",
                table: "FinancialTransactions",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialPeriods_CreatedAt",
                table: "FinancialPeriods",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_EntryCostCenters_CreatedAt",
                table: "EntryCostCenters",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_EntryAttachments_CreatedAt",
                table: "EntryAttachments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_CreatedAt",
                table: "Entries",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_EntryDate",
                table: "Entries",
                column: "EntryDate");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_EntryNumber",
                table: "Entries",
                column: "EntryNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_FinancialPeriodId_EntryNumber",
                table: "Entries",
                columns: new[] { "FinancialPeriodId", "EntryNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CreatedAt",
                table: "Customers",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_CreatedAt",
                table: "Currencies",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_CreatedAt",
                table: "CostCenters",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenterChartOfAccounts_CreatedAt",
                table: "CostCenterChartOfAccounts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Colors_CreatedAt",
                table: "Colors",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionBooks_CreatedAt",
                table: "CollectionBooks",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ChartOfAccounts_CreatedAt",
                table: "ChartOfAccounts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CashInBoxes_CreatedAt",
                table: "CashInBoxes",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CreatedAt",
                table: "Branches",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_CreatedAt",
                table: "Banks",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_CreatedAt",
                table: "Attachments",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AccountGuides_CreatedAt",
                table: "AccountGuides",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Action",
                table: "AuditLogs",
                column: "Action");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityId",
                table: "AuditLogs",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityType",
                table: "AuditLogs",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_EntityType_EntityId",
                table: "AuditLogs",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_Timestamp",
                table: "AuditLogs",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_AuditLogs_UserId",
                table: "AuditLogs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropIndex(
                name: "IX_Suppliers_CreatedAt",
                table: "Suppliers");

            migrationBuilder.DropIndex(
                name: "IX_StockBalances_CreatedAt",
                table: "StockBalances");

            migrationBuilder.DropIndex(
                name: "IX_Sizes_CreatedAt",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_SellingPrices_CreatedAt",
                table: "SellingPrices");

            migrationBuilder.DropIndex(
                name: "IX_Roles_CreatedAt",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_PackingUnits_CreatedAt",
                table: "PackingUnits");

            migrationBuilder.DropIndex(
                name: "IX_ManufacturerCompanies_CreatedAt",
                table: "ManufacturerCompanies");

            migrationBuilder.DropIndex(
                name: "IX_ItemSuppliers_CreatedAt",
                table: "ItemSuppliers");

            migrationBuilder.DropIndex(
                name: "IX_ItemSellingPriceDiscounts_CreatedAt",
                table: "ItemSellingPriceDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_Items_CreatedAt",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_NodeType",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ParentId_NodeType",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_ItemPackingUnitSellingPrices_CreatedAt",
                table: "ItemPackingUnitSellingPrices");

            migrationBuilder.DropIndex(
                name: "IX_ItemPackingUnits_CreatedAt",
                table: "ItemPackingUnits");

            migrationBuilder.DropIndex(
                name: "IX_ItemManufacturerCompanies_CreatedAt",
                table: "ItemManufacturerCompanies");

            migrationBuilder.DropIndex(
                name: "IX_ItemCodes_CreatedAt",
                table: "ItemCodes");

            migrationBuilder.DropIndex(
                name: "IX_InventoryTransactions_CreatedAt",
                table: "InventoryTransactions");

            migrationBuilder.DropIndex(
                name: "IX_InventoryTransactionItems_CreatedAt",
                table: "InventoryTransactionItems");

            migrationBuilder.DropIndex(
                name: "IX_GLSettings_CreatedAt",
                table: "GLSettings");

            migrationBuilder.DropIndex(
                name: "IX_FixedAssets_CreatedAt",
                table: "FixedAssets");

            migrationBuilder.DropIndex(
                name: "IX_FinancialTransactions_AccountNature",
                table: "FinancialTransactions");

            migrationBuilder.DropIndex(
                name: "IX_FinancialTransactions_CreatedAt",
                table: "FinancialTransactions");

            migrationBuilder.DropIndex(
                name: "IX_FinancialPeriods_CreatedAt",
                table: "FinancialPeriods");

            migrationBuilder.DropIndex(
                name: "IX_EntryCostCenters_CreatedAt",
                table: "EntryCostCenters");

            migrationBuilder.DropIndex(
                name: "IX_EntryAttachments_CreatedAt",
                table: "EntryAttachments");

            migrationBuilder.DropIndex(
                name: "IX_Entries_CreatedAt",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_EntryDate",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_EntryNumber",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Entries_FinancialPeriodId_EntryNumber",
                table: "Entries");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CreatedAt",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Currencies_CreatedAt",
                table: "Currencies");

            migrationBuilder.DropIndex(
                name: "IX_CostCenters_CreatedAt",
                table: "CostCenters");

            migrationBuilder.DropIndex(
                name: "IX_CostCenterChartOfAccounts_CreatedAt",
                table: "CostCenterChartOfAccounts");

            migrationBuilder.DropIndex(
                name: "IX_Colors_CreatedAt",
                table: "Colors");

            migrationBuilder.DropIndex(
                name: "IX_CollectionBooks_CreatedAt",
                table: "CollectionBooks");

            migrationBuilder.DropIndex(
                name: "IX_ChartOfAccounts_CreatedAt",
                table: "ChartOfAccounts");

            migrationBuilder.DropIndex(
                name: "IX_CashInBoxes_CreatedAt",
                table: "CashInBoxes");

            migrationBuilder.DropIndex(
                name: "IX_Branches_CreatedAt",
                table: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Banks_CreatedAt",
                table: "Banks");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_CreatedAt",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_AccountGuides_CreatedAt",
                table: "AccountGuides");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNature",
                table: "FinancialTransactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "EntryNumber",
                table: "Entries",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
