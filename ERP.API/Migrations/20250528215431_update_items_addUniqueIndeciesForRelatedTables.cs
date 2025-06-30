using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class update_items_addUniqueIndeciesForRelatedTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemSuppliers_ItemId",
                table: "ItemSuppliers");

            migrationBuilder.DropIndex(
                name: "IX_ItemSellingPriceDiscounts_ItemId",
                table: "ItemSellingPriceDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_ItemPackingUnitSellingPrices_ItemPackingUnitPriceId",
                table: "ItemPackingUnitSellingPrices");

            migrationBuilder.DropIndex(
                name: "IX_ItemPackingUnits_ItemId",
                table: "ItemPackingUnits");

            migrationBuilder.DropIndex(
                name: "IX_ItemManufacturerCompanies_ItemId",
                table: "ItemManufacturerCompanies");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSuppliers_ItemId_SupplierId",
                table: "ItemSuppliers",
                columns: new[] { "ItemId", "SupplierId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemSellingPriceDiscounts_ItemId_SellingPriceId",
                table: "ItemSellingPriceDiscounts",
                columns: new[] { "ItemId", "SellingPriceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnitSellingPrices_ItemPackingUnitPriceId_sellingPriceId",
                table: "ItemPackingUnitSellingPrices",
                columns: new[] { "ItemPackingUnitPriceId", "sellingPriceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnits_ItemId_PackingUnitId",
                table: "ItemPackingUnits",
                columns: new[] { "ItemId", "PackingUnitId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemManufacturerCompanies_ItemId_ManufacturerCompanyId",
                table: "ItemManufacturerCompanies",
                columns: new[] { "ItemId", "ManufacturerCompanyId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ItemSuppliers_ItemId_SupplierId",
                table: "ItemSuppliers");

            migrationBuilder.DropIndex(
                name: "IX_ItemSellingPriceDiscounts_ItemId_SellingPriceId",
                table: "ItemSellingPriceDiscounts");

            migrationBuilder.DropIndex(
                name: "IX_ItemPackingUnitSellingPrices_ItemPackingUnitPriceId_sellingPriceId",
                table: "ItemPackingUnitSellingPrices");

            migrationBuilder.DropIndex(
                name: "IX_ItemPackingUnits_ItemId_PackingUnitId",
                table: "ItemPackingUnits");

            migrationBuilder.DropIndex(
                name: "IX_ItemManufacturerCompanies_ItemId_ManufacturerCompanyId",
                table: "ItemManufacturerCompanies");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSuppliers_ItemId",
                table: "ItemSuppliers",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSellingPriceDiscounts_ItemId",
                table: "ItemSellingPriceDiscounts",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnitSellingPrices_ItemPackingUnitPriceId",
                table: "ItemPackingUnitSellingPrices",
                column: "ItemPackingUnitPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPackingUnits_ItemId",
                table: "ItemPackingUnits",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemManufacturerCompanies_ItemId",
                table: "ItemManufacturerCompanies",
                column: "ItemId");
        }
    }
}
