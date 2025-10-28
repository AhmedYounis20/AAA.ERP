using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Inventory");

            migrationBuilder.CreateTable(
                name: "Attachments",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttributeDefinitions",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ColorValue = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransactions",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TransactionNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TransactionPartyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FinancialPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransfers",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SourceBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinationBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TransferType = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransfers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                schema: "Inventory",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplyDomainChanges = table.Column<bool>(type: "bit", nullable: false),
                    ColorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SizeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ItemType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConditionalDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DefaultDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DefaultDiscountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDiscountBasedOnSellingPrice = table.Column<bool>(type: "bit", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryOfOrigin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NodeType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Items_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Inventory",
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ManufacturerCompanies",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManufacturerCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PackingUnits",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackingUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SellingPrices",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellingPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sizes",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sizes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttributeValues",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeDefinitionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttributeValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttributeValues_AttributeDefinitions_AttributeDefinitionId",
                        column: x => x.AttributeDefinitionId,
                        principalSchema: "Inventory",
                        principalTable: "AttributeDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemCodes",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CodeType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemCodes_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemSuppliers",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSuppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemSuppliers_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemManufacturerCompanies",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ManufacturerCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemManufacturerCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemManufacturerCompanies_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemManufacturerCompanies_ManufacturerCompanies_ManufacturerCompanyId",
                        column: x => x.ManufacturerCompanyId,
                        principalSchema: "Inventory",
                        principalTable: "ManufacturerCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransactionItems",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackingUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransactionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransactionItems_InventoryTransactions_InventoryTransactionId",
                        column: x => x.InventoryTransactionId,
                        principalSchema: "Inventory",
                        principalTable: "InventoryTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryTransactionItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryTransactionItems_PackingUnits_PackingUnitId",
                        column: x => x.PackingUnitId,
                        principalSchema: "Inventory",
                        principalTable: "PackingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryTransferItems",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryTransferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackingUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryTransferItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryTransferItems_InventoryTransfers_InventoryTransferId",
                        column: x => x.InventoryTransferId,
                        principalSchema: "Inventory",
                        principalTable: "InventoryTransfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryTransferItems_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryTransferItems_PackingUnits_PackingUnitId",
                        column: x => x.PackingUnitId,
                        principalSchema: "Inventory",
                        principalTable: "PackingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemPackingUnits",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackingUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartsCount = table.Column<int>(type: "int", nullable: false),
                    IsDefaultPackingUnit = table.Column<bool>(type: "bit", nullable: false),
                    IsDefaultSales = table.Column<bool>(type: "bit", nullable: false),
                    IsDefaultPurchases = table.Column<bool>(type: "bit", nullable: false),
                    LastCostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AverageCostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPackingUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemPackingUnits_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemPackingUnits_PackingUnits_PackingUnitId",
                        column: x => x.PackingUnitId,
                        principalSchema: "Inventory",
                        principalTable: "PackingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockBalances",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackingUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrentBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MinimumBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaximumBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockBalances_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockBalances_PackingUnits_PackingUnitId",
                        column: x => x.PackingUnitId,
                        principalSchema: "Inventory",
                        principalTable: "PackingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemSellingPriceDiscounts",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellingPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSellingPriceDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemSellingPriceDiscounts_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "Inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemSellingPriceDiscounts_SellingPrices_SellingPriceId",
                        column: x => x.SellingPriceId,
                        principalSchema: "Inventory",
                        principalTable: "SellingPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemPackingUnitSellingPrices",
                schema: "Inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemPackingUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellingPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPackingUnitSellingPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemPackingUnitSellingPrices_ItemPackingUnits_ItemPackingUnitId",
                        column: x => x.ItemPackingUnitId,
                        principalSchema: "Inventory",
                        principalTable: "ItemPackingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemPackingUnitSellingPrices_SellingPrices_SellingPriceId",
                        column: x => x.SellingPriceId,
                        principalSchema: "Inventory",
                        principalTable: "SellingPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttributeDefinitions_Name",
                schema: "Inventory",
                table: "AttributeDefinitions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttributeDefinitions_NameSecondLanguage",
                schema: "Inventory",
                table: "AttributeDefinitions",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttributeValues_AttributeDefinitionId_Name",
                schema: "Inventory",
                table: "AttributeValues",
                columns: new[] { "AttributeDefinitionId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_Code",
                schema: "Inventory",
                table: "Colors",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_ColorValue",
                schema: "Inventory",
                table: "Colors",
                column: "ColorValue",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_Name",
                schema: "Inventory",
                table: "Colors",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_NameSecondLanguage",
                schema: "Inventory",
                table: "Colors",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactionItems_InventoryTransactionId",
                schema: "Inventory",
                table: "InventoryTransactionItems",
                column: "InventoryTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactionItems_ItemId",
                schema: "Inventory",
                table: "InventoryTransactionItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransactionItems_PackingUnitId",
                schema: "Inventory",
                table: "InventoryTransactionItems",
                column: "PackingUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferItems_InventoryTransferId",
                schema: "Inventory",
                table: "InventoryTransferItems",
                column: "InventoryTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferItems_ItemId",
                schema: "Inventory",
                table: "InventoryTransferItems",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryTransferItems_PackingUnitId",
                schema: "Inventory",
                table: "InventoryTransferItems",
                column: "PackingUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCodes_Code",
                schema: "Inventory",
                table: "ItemCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemCodes_CodeType",
                schema: "Inventory",
                table: "ItemCodes",
                column: "CodeType");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCodes_ItemId",
                schema: "Inventory",
                table: "ItemCodes",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemManufacturerCompanies_ItemId_ManufacturerCompanyId",
                schema: "Inventory",
                table: "ItemManufacturerCompanies",
                columns: new[] { "ItemId", "ManufacturerCompanyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemManufacturerCompanies_ManufacturerCompanyId",
                schema: "Inventory",
                table: "ItemManufacturerCompanies",
                column: "ManufacturerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnits_ItemId_PackingUnitId",
                schema: "Inventory",
                table: "ItemPackingUnits",
                columns: new[] { "ItemId", "PackingUnitId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnits_PackingUnitId",
                schema: "Inventory",
                table: "ItemPackingUnits",
                column: "PackingUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnitSellingPrices_ItemPackingUnitId_SellingPriceId",
                schema: "Inventory",
                table: "ItemPackingUnitSellingPrices",
                columns: new[] { "ItemPackingUnitId", "SellingPriceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnitSellingPrices_SellingPriceId",
                schema: "Inventory",
                table: "ItemPackingUnitSellingPrices",
                column: "SellingPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Name",
                schema: "Inventory",
                table: "Items",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_NameSecondLanguage",
                schema: "Inventory",
                table: "Items",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ParentId",
                schema: "Inventory",
                table: "Items",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSellingPriceDiscounts_ItemId_SellingPriceId",
                schema: "Inventory",
                table: "ItemSellingPriceDiscounts",
                columns: new[] { "ItemId", "SellingPriceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemSellingPriceDiscounts_SellingPriceId",
                schema: "Inventory",
                table: "ItemSellingPriceDiscounts",
                column: "SellingPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSuppliers_ItemId_SupplierId",
                schema: "Inventory",
                table: "ItemSuppliers",
                columns: new[] { "ItemId", "SupplierId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturerCompanies_Name",
                schema: "Inventory",
                table: "ManufacturerCompanies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ManufacturerCompanies_NameSecondLanguage",
                schema: "Inventory",
                table: "ManufacturerCompanies",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PackingUnits_Name",
                schema: "Inventory",
                table: "PackingUnits",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PackingUnits_NameSecondLanguage",
                schema: "Inventory",
                table: "PackingUnits",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellingPrices_Name",
                schema: "Inventory",
                table: "SellingPrices",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellingPrices_NameSecondLanguage",
                schema: "Inventory",
                table: "SellingPrices",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_Code",
                schema: "Inventory",
                table: "Sizes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_Name",
                schema: "Inventory",
                table: "Sizes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_NameSecondLanguage",
                schema: "Inventory",
                table: "Sizes",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockBalances_ItemId_PackingUnitId_BranchId",
                schema: "Inventory",
                table: "StockBalances",
                columns: new[] { "ItemId", "PackingUnitId", "BranchId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockBalances_PackingUnitId",
                schema: "Inventory",
                table: "StockBalances",
                column: "PackingUnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attachments",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "AttributeValues",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "Colors",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "InventoryTransactionItems",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "InventoryTransferItems",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "ItemCodes",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "ItemManufacturerCompanies",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "ItemPackingUnitSellingPrices",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "ItemSellingPriceDiscounts",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "ItemSuppliers",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "Sizes",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "StockBalances",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "AttributeDefinitions",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "InventoryTransactions",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "InventoryTransfers",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "ManufacturerCompanies",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "ItemPackingUnits",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "SellingPrices",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "Items",
                schema: "Inventory");

            migrationBuilder.DropTable(
                name: "PackingUnits",
                schema: "Inventory");
        }
    }
}
