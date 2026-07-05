using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class add_items : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemCodes",
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
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemManufacturerCompanies",
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
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemManufacturerCompanies_ManufacturerCompanies_ManufacturerCompanyId",
                        column: x => x.ManufacturerCompanyId,
                        principalTable: "ManufacturerCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemPackingUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackingUnitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartsCount = table.Column<int>(type: "int", nullable: false),
                    IsDefaultSales = table.Column<bool>(type: "bit", nullable: false),
                    IsDefaultPurchases = table.Column<bool>(type: "bit", nullable: false),
                    LastCostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AverageCostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemPackingUnits_PackingUnits_PackingUnitId",
                        column: x => x.PackingUnitId,
                        principalTable: "PackingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemSellingPriceDiscounts",
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
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemSellingPriceDiscounts_SellingPrices_SellingPriceId",
                        column: x => x.SellingPriceId,
                        principalTable: "SellingPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemSuppliers",
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
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemSuppliers_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ItemPackingUnitSellingPrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ItemPackingUnitPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    sellingPriceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        name: "FK_ItemPackingUnitSellingPrices_ItemPackingUnits_ItemPackingUnitPriceId",
                        column: x => x.ItemPackingUnitPriceId,
                        principalTable: "ItemPackingUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemPackingUnitSellingPrices_SellingPrices_sellingPriceId",
                        column: x => x.sellingPriceId,
                        principalTable: "SellingPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemCodes_Code",
                table: "ItemCodes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemCodes_CodeType",
                table: "ItemCodes",
                column: "CodeType");

            migrationBuilder.CreateIndex(
                name: "IX_ItemCodes_ItemId",
                table: "ItemCodes",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemManufacturerCompanies_ItemId",
                table: "ItemManufacturerCompanies",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemManufacturerCompanies_ManufacturerCompanyId",
                table: "ItemManufacturerCompanies",
                column: "ManufacturerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnits_ItemId",
                table: "ItemPackingUnits",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnits_PackingUnitId",
                table: "ItemPackingUnits",
                column: "PackingUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnitSellingPrices_ItemPackingUnitPriceId",
                table: "ItemPackingUnitSellingPrices",
                column: "ItemPackingUnitPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnitSellingPrices_sellingPriceId",
                table: "ItemPackingUnitSellingPrices",
                column: "sellingPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Name",
                table: "Items",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_NameSecondLanguage",
                table: "Items",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ParentId",
                table: "Items",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSellingPriceDiscounts_ItemId",
                table: "ItemSellingPriceDiscounts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSellingPriceDiscounts_SellingPriceId",
                table: "ItemSellingPriceDiscounts",
                column: "SellingPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSuppliers_ItemId",
                table: "ItemSuppliers",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSuppliers_SupplierId",
                table: "ItemSuppliers",
                column: "SupplierId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemCodes");

            migrationBuilder.DropTable(
                name: "ItemManufacturerCompanies");

            migrationBuilder.DropTable(
                name: "ItemPackingUnitSellingPrices");

            migrationBuilder.DropTable(
                name: "ItemSellingPriceDiscounts");

            migrationBuilder.DropTable(
                name: "ItemSuppliers");

            migrationBuilder.DropTable(
                name: "ItemPackingUnits");

            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
