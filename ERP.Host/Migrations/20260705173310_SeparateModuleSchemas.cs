using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    /// <inheritdoc />
    public partial class SeparateModuleSchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "accounting");

            migrationBuilder.EnsureSchema(
                name: "identity");

            migrationBuilder.EnsureSchema(
                name: "inventory");

            migrationBuilder.EnsureSchema(
                name: "platform");

            migrationBuilder.RenameTable(
                name: "VariantSuppliers",
                newName: "VariantSuppliers",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "VariantSellingPriceDiscounts",
                newName: "VariantSellingPriceDiscounts",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "VariantSalesTaxes",
                newName: "VariantSalesTaxes",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "Variants",
                newName: "Variants",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "VariantPurchaseTaxes",
                newName: "VariantPurchaseTaxes",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "VariantPackingUnitSellingPrices",
                newName: "VariantPackingUnitSellingPrices",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "VariantPackingUnits",
                newName: "VariantPackingUnits",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "VariantManufacturerCompanies",
                newName: "VariantManufacturerCompanies",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "VariantCostCenters",
                newName: "VariantCostCenters",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "VariantCodes",
                newName: "VariantCodes",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "VariantAttributeValues",
                newName: "VariantAttributeValues",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "VariantAttachments",
                newName: "VariantAttachments",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "Taxes",
                newName: "Taxes",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "Suppliers",
                newName: "Suppliers",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "StockBalances",
                newName: "StockBalances",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "Sizes",
                newName: "Sizes",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "SellingPrices",
                newName: "SellingPrices",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Roles",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "ProductSuppliers",
                newName: "ProductSuppliers",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ProductSellingPriceDiscounts",
                newName: "ProductSellingPriceDiscounts",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ProductSalesTaxes",
                newName: "ProductSalesTaxes",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Products",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ProductPurchaseTaxes",
                newName: "ProductPurchaseTaxes",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ProductPackingUnitSellingPrices",
                newName: "ProductPackingUnitSellingPrices",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ProductPackingUnits",
                newName: "ProductPackingUnits",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ProductManufacturerCompanies",
                newName: "ProductManufacturerCompanies",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ProductCostCenters",
                newName: "ProductCostCenters",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ProductCodes",
                newName: "ProductCodes",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ProductAttributeDefinitions",
                newName: "ProductAttributeDefinitions",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ProductAttachments",
                newName: "ProductAttachments",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "PackingUnits",
                newName: "PackingUnits",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ManufacturerCompanies",
                newName: "ManufacturerCompanies",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ItemSuppliers",
                newName: "ItemSuppliers",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ItemSellingPriceDiscounts",
                newName: "ItemSellingPriceDiscounts",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "Items",
                newName: "Items",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ItemPackingUnitSellingPrices",
                newName: "ItemPackingUnitSellingPrices",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ItemPackingUnits",
                newName: "ItemPackingUnits",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ItemManufacturerCompanies",
                newName: "ItemManufacturerCompanies",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "ItemCodes",
                newName: "ItemCodes",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "InventoryTransfers",
                newName: "InventoryTransfers",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "InventoryTransferItems",
                newName: "InventoryTransferItems",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "InventoryTransactions",
                newName: "InventoryTransactions",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "InventoryTransactionItems",
                newName: "InventoryTransactionItems",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "InventoryThresholds",
                newName: "InventoryThresholds",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "GLSettings",
                newName: "GLSettings",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "FixedAssets",
                newName: "FixedAssets",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "FinancialTransactions",
                newName: "FinancialTransactions",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "FinancialPeriods",
                newName: "FinancialPeriods",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "ExpiryLevels",
                newName: "ExpiryLevels",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "EntryCostCenters",
                newName: "EntryCostCenters",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "EntryAttachments",
                newName: "EntryAttachments",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "Entries",
                newName: "Entries",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customers",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "Currencies",
                newName: "Currencies",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "CostCenters",
                newName: "CostCenters",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "CostCenterChartOfAccounts",
                newName: "CostCenterChartOfAccounts",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "Colors",
                newName: "Colors",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "CollectionBooks",
                newName: "CollectionBooks",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "ChartOfAccounts",
                newName: "ChartOfAccounts",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "CashInBoxes",
                newName: "CashInBoxes",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "Branches",
                newName: "Branches",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "Banks",
                newName: "Banks",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "AuditLogs",
                newName: "AuditLogs",
                newSchema: "platform");

            migrationBuilder.RenameTable(
                name: "AttributeValues",
                newName: "AttributeValues",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "AttributeDefinitions",
                newName: "AttributeDefinitions",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "Attachments",
                newName: "Attachments",
                newSchema: "accounting");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AspNetUsers",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "identity");

            migrationBuilder.RenameTable(
                name: "AccountGuides",
                newName: "AccountGuides",
                newSchema: "accounting");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "VariantSuppliers",
                schema: "inventory",
                newName: "VariantSuppliers");

            migrationBuilder.RenameTable(
                name: "VariantSellingPriceDiscounts",
                schema: "inventory",
                newName: "VariantSellingPriceDiscounts");

            migrationBuilder.RenameTable(
                name: "VariantSalesTaxes",
                schema: "inventory",
                newName: "VariantSalesTaxes");

            migrationBuilder.RenameTable(
                name: "Variants",
                schema: "inventory",
                newName: "Variants");

            migrationBuilder.RenameTable(
                name: "VariantPurchaseTaxes",
                schema: "inventory",
                newName: "VariantPurchaseTaxes");

            migrationBuilder.RenameTable(
                name: "VariantPackingUnitSellingPrices",
                schema: "inventory",
                newName: "VariantPackingUnitSellingPrices");

            migrationBuilder.RenameTable(
                name: "VariantPackingUnits",
                schema: "inventory",
                newName: "VariantPackingUnits");

            migrationBuilder.RenameTable(
                name: "VariantManufacturerCompanies",
                schema: "inventory",
                newName: "VariantManufacturerCompanies");

            migrationBuilder.RenameTable(
                name: "VariantCostCenters",
                schema: "inventory",
                newName: "VariantCostCenters");

            migrationBuilder.RenameTable(
                name: "VariantCodes",
                schema: "inventory",
                newName: "VariantCodes");

            migrationBuilder.RenameTable(
                name: "VariantAttributeValues",
                schema: "inventory",
                newName: "VariantAttributeValues");

            migrationBuilder.RenameTable(
                name: "VariantAttachments",
                schema: "inventory",
                newName: "VariantAttachments");

            migrationBuilder.RenameTable(
                name: "Taxes",
                schema: "accounting",
                newName: "Taxes");

            migrationBuilder.RenameTable(
                name: "Suppliers",
                schema: "accounting",
                newName: "Suppliers");

            migrationBuilder.RenameTable(
                name: "StockBalances",
                schema: "inventory",
                newName: "StockBalances");

            migrationBuilder.RenameTable(
                name: "Sizes",
                schema: "inventory",
                newName: "Sizes");

            migrationBuilder.RenameTable(
                name: "SellingPrices",
                schema: "inventory",
                newName: "SellingPrices");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "accounting",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "ProductSuppliers",
                schema: "inventory",
                newName: "ProductSuppliers");

            migrationBuilder.RenameTable(
                name: "ProductSellingPriceDiscounts",
                schema: "inventory",
                newName: "ProductSellingPriceDiscounts");

            migrationBuilder.RenameTable(
                name: "ProductSalesTaxes",
                schema: "inventory",
                newName: "ProductSalesTaxes");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "inventory",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "ProductPurchaseTaxes",
                schema: "inventory",
                newName: "ProductPurchaseTaxes");

            migrationBuilder.RenameTable(
                name: "ProductPackingUnitSellingPrices",
                schema: "inventory",
                newName: "ProductPackingUnitSellingPrices");

            migrationBuilder.RenameTable(
                name: "ProductPackingUnits",
                schema: "inventory",
                newName: "ProductPackingUnits");

            migrationBuilder.RenameTable(
                name: "ProductManufacturerCompanies",
                schema: "inventory",
                newName: "ProductManufacturerCompanies");

            migrationBuilder.RenameTable(
                name: "ProductCostCenters",
                schema: "inventory",
                newName: "ProductCostCenters");

            migrationBuilder.RenameTable(
                name: "ProductCodes",
                schema: "inventory",
                newName: "ProductCodes");

            migrationBuilder.RenameTable(
                name: "ProductAttributeDefinitions",
                schema: "inventory",
                newName: "ProductAttributeDefinitions");

            migrationBuilder.RenameTable(
                name: "ProductAttachments",
                schema: "inventory",
                newName: "ProductAttachments");

            migrationBuilder.RenameTable(
                name: "PackingUnits",
                schema: "inventory",
                newName: "PackingUnits");

            migrationBuilder.RenameTable(
                name: "ManufacturerCompanies",
                schema: "inventory",
                newName: "ManufacturerCompanies");

            migrationBuilder.RenameTable(
                name: "ItemSuppliers",
                schema: "inventory",
                newName: "ItemSuppliers");

            migrationBuilder.RenameTable(
                name: "ItemSellingPriceDiscounts",
                schema: "inventory",
                newName: "ItemSellingPriceDiscounts");

            migrationBuilder.RenameTable(
                name: "Items",
                schema: "inventory",
                newName: "Items");

            migrationBuilder.RenameTable(
                name: "ItemPackingUnitSellingPrices",
                schema: "inventory",
                newName: "ItemPackingUnitSellingPrices");

            migrationBuilder.RenameTable(
                name: "ItemPackingUnits",
                schema: "inventory",
                newName: "ItemPackingUnits");

            migrationBuilder.RenameTable(
                name: "ItemManufacturerCompanies",
                schema: "inventory",
                newName: "ItemManufacturerCompanies");

            migrationBuilder.RenameTable(
                name: "ItemCodes",
                schema: "inventory",
                newName: "ItemCodes");

            migrationBuilder.RenameTable(
                name: "InventoryTransfers",
                schema: "inventory",
                newName: "InventoryTransfers");

            migrationBuilder.RenameTable(
                name: "InventoryTransferItems",
                schema: "inventory",
                newName: "InventoryTransferItems");

            migrationBuilder.RenameTable(
                name: "InventoryTransactions",
                schema: "inventory",
                newName: "InventoryTransactions");

            migrationBuilder.RenameTable(
                name: "InventoryTransactionItems",
                schema: "inventory",
                newName: "InventoryTransactionItems");

            migrationBuilder.RenameTable(
                name: "InventoryThresholds",
                schema: "inventory",
                newName: "InventoryThresholds");

            migrationBuilder.RenameTable(
                name: "GLSettings",
                schema: "accounting",
                newName: "GLSettings");

            migrationBuilder.RenameTable(
                name: "FixedAssets",
                schema: "accounting",
                newName: "FixedAssets");

            migrationBuilder.RenameTable(
                name: "FinancialTransactions",
                schema: "accounting",
                newName: "FinancialTransactions");

            migrationBuilder.RenameTable(
                name: "FinancialPeriods",
                schema: "accounting",
                newName: "FinancialPeriods");

            migrationBuilder.RenameTable(
                name: "ExpiryLevels",
                schema: "inventory",
                newName: "ExpiryLevels");

            migrationBuilder.RenameTable(
                name: "EntryCostCenters",
                schema: "accounting",
                newName: "EntryCostCenters");

            migrationBuilder.RenameTable(
                name: "EntryAttachments",
                schema: "accounting",
                newName: "EntryAttachments");

            migrationBuilder.RenameTable(
                name: "Entries",
                schema: "accounting",
                newName: "Entries");

            migrationBuilder.RenameTable(
                name: "Customers",
                schema: "accounting",
                newName: "Customers");

            migrationBuilder.RenameTable(
                name: "Currencies",
                schema: "accounting",
                newName: "Currencies");

            migrationBuilder.RenameTable(
                name: "CostCenters",
                schema: "accounting",
                newName: "CostCenters");

            migrationBuilder.RenameTable(
                name: "CostCenterChartOfAccounts",
                schema: "accounting",
                newName: "CostCenterChartOfAccounts");

            migrationBuilder.RenameTable(
                name: "Colors",
                schema: "inventory",
                newName: "Colors");

            migrationBuilder.RenameTable(
                name: "CollectionBooks",
                schema: "accounting",
                newName: "CollectionBooks");

            migrationBuilder.RenameTable(
                name: "ChartOfAccounts",
                schema: "accounting",
                newName: "ChartOfAccounts");

            migrationBuilder.RenameTable(
                name: "CashInBoxes",
                schema: "accounting",
                newName: "CashInBoxes");

            migrationBuilder.RenameTable(
                name: "Branches",
                schema: "accounting",
                newName: "Branches");

            migrationBuilder.RenameTable(
                name: "Banks",
                schema: "accounting",
                newName: "Banks");

            migrationBuilder.RenameTable(
                name: "AuditLogs",
                schema: "platform",
                newName: "AuditLogs");

            migrationBuilder.RenameTable(
                name: "AttributeValues",
                schema: "inventory",
                newName: "AttributeValues");

            migrationBuilder.RenameTable(
                name: "AttributeDefinitions",
                schema: "inventory",
                newName: "AttributeDefinitions");

            migrationBuilder.RenameTable(
                name: "Attachments",
                schema: "accounting",
                newName: "Attachments");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "identity",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "identity",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "identity",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "identity",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "identity",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "identity",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "identity",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameTable(
                name: "AccountGuides",
                schema: "accounting",
                newName: "AccountGuides");
        }
    }
}
