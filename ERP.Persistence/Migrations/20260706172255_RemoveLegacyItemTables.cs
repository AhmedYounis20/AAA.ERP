using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveLegacyItemTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemCodes",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "ItemManufacturerCompanies",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "ItemPackingUnitSellingPrices",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "ItemSellingPriceDiscounts",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "ItemSuppliers",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "ItemPackingUnits",
                schema: "inventory");

            migrationBuilder.DropTable(
                name: "Items",
                schema: "inventory");

            migrationBuilder.CreateIndex(
                name: "IX_VariantSuppliers_SupplierId",
                schema: "inventory",
                table: "VariantSuppliers",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_VariantSuppliers_Suppliers_SupplierId",
                schema: "inventory",
                table: "VariantSuppliers",
                column: "SupplierId",
                principalSchema: "accounting",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VariantSuppliers_Suppliers_SupplierId",
                schema: "inventory",
                table: "VariantSuppliers");

            migrationBuilder.DropIndex(
                name: "IX_VariantSuppliers_SupplierId",
                schema: "inventory",
                table: "VariantSuppliers");

            migrationBuilder.CreateTable(
                name: "Items",
                schema: "inventory",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApplyDomainChanges = table.Column<bool>(type: "bit", nullable: false),
                    ColorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConditionalDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CountryOfOrigin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DefaultDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DefaultDiscountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDiscountBasedOnSellingPrice = table.Column<bool>(type: "bit", nullable: false),
                    ItemType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Model = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NodeType = table.Column<int>(type: "int", nullable: false),
                    SizeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Items_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "inventory",
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemCodes",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CodeType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemCodes_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemManufacturerCompanies",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ManufacturerCompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemManufacturerCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemManufacturerCompanies_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemManufacturerCompanies_ManufacturerCompanies_ManufacturerCompanyId",
                        column: x => x.ManufacturerCompanyId,
                        principalSchema: "inventory",
                        principalTable: "ManufacturerCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemPackingUnits",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackingUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AverageCostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDefaultPackingUnit = table.Column<bool>(type: "bit", nullable: false),
                    IsDefaultPurchases = table.Column<bool>(type: "bit", nullable: false),
                    IsDefaultSales = table.Column<bool>(type: "bit", nullable: false),
                    LastCostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    PartsCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPackingUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemPackingUnits_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemPackingUnits_PackingUnits_PackingUnitId",
                        column: x => x.PackingUnitId,
                        principalSchema: "inventory",
                        principalTable: "PackingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemSellingPriceDiscounts",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellingPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSellingPriceDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemSellingPriceDiscounts_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemSellingPriceDiscounts_SellingPrices_SellingPriceId",
                        column: x => x.SellingPriceId,
                        principalSchema: "inventory",
                        principalTable: "SellingPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemSuppliers",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSuppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemSuppliers_Items_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "inventory",
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemSuppliers_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "accounting",
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemPackingUnitSellingPrices",
                schema: "inventory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemPackingUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SellingPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPackingUnitSellingPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemPackingUnitSellingPrices_ItemPackingUnits_ItemPackingUnitId",
                        column: x => x.ItemPackingUnitId,
                        principalSchema: "inventory",
                        principalTable: "ItemPackingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemPackingUnitSellingPrices_SellingPrices_SellingPriceId",
                        column: x => x.SellingPriceId,
                        principalSchema: "inventory",
                        principalTable: "SellingPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemCodes_Code",
                schema: "inventory",
                table: "ItemCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemCodes_CodeType",
                schema: "inventory",
                table: "ItemCodes",
                column: "CodeType");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCodes_CreatedAt",
                schema: "inventory",
                table: "ItemCodes",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCodes_ItemId",
                schema: "inventory",
                table: "ItemCodes",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemManufacturerCompanies_CreatedAt",
                schema: "inventory",
                table: "ItemManufacturerCompanies",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ItemManufacturerCompanies_ItemId_ManufacturerCompanyId",
                schema: "inventory",
                table: "ItemManufacturerCompanies",
                columns: new[] { "ItemId", "ManufacturerCompanyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemManufacturerCompanies_ManufacturerCompanyId",
                schema: "inventory",
                table: "ItemManufacturerCompanies",
                column: "ManufacturerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnits_CreatedAt",
                schema: "inventory",
                table: "ItemPackingUnits",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnits_ItemId_PackingUnitId",
                schema: "inventory",
                table: "ItemPackingUnits",
                columns: new[] { "ItemId", "PackingUnitId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnits_PackingUnitId",
                schema: "inventory",
                table: "ItemPackingUnits",
                column: "PackingUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnitSellingPrices_CreatedAt",
                schema: "inventory",
                table: "ItemPackingUnitSellingPrices",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnitSellingPrices_ItemPackingUnitId_SellingPriceId",
                schema: "inventory",
                table: "ItemPackingUnitSellingPrices",
                columns: new[] { "ItemPackingUnitId", "SellingPriceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnitSellingPrices_SellingPriceId",
                schema: "inventory",
                table: "ItemPackingUnitSellingPrices",
                column: "SellingPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_CreatedAt",
                schema: "inventory",
                table: "Items",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Name",
                schema: "inventory",
                table: "Items",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_NameSecondLanguage",
                schema: "inventory",
                table: "Items",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_NodeType",
                schema: "inventory",
                table: "Items",
                column: "NodeType");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ParentId",
                schema: "inventory",
                table: "Items",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ParentId_NodeType",
                schema: "inventory",
                table: "Items",
                columns: new[] { "ParentId", "NodeType" });

            migrationBuilder.CreateIndex(
                name: "IX_ItemSellingPriceDiscounts_CreatedAt",
                schema: "inventory",
                table: "ItemSellingPriceDiscounts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSellingPriceDiscounts_ItemId_SellingPriceId",
                schema: "inventory",
                table: "ItemSellingPriceDiscounts",
                columns: new[] { "ItemId", "SellingPriceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemSellingPriceDiscounts_SellingPriceId",
                schema: "inventory",
                table: "ItemSellingPriceDiscounts",
                column: "SellingPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSuppliers_CreatedAt",
                schema: "inventory",
                table: "ItemSuppliers",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSuppliers_ItemId_SupplierId",
                schema: "inventory",
                table: "ItemSuppliers",
                columns: new[] { "ItemId", "SupplierId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemSuppliers_SupplierId",
                schema: "inventory",
                table: "ItemSuppliers",
                column: "SupplierId");
        }
    }
}
