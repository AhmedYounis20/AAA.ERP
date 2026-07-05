using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class update_items_PackingUnitSellingPrices_ChangeColumnsNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPackingUnitSellingPrices_ItemPackingUnits_ItemPackingUnitPriceId",
                table: "ItemPackingUnitSellingPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPackingUnitSellingPrices_SellingPrices_sellingPriceId",
                table: "ItemPackingUnitSellingPrices");

            migrationBuilder.RenameColumn(
                name: "sellingPriceId",
                table: "ItemPackingUnitSellingPrices",
                newName: "SellingPriceId");

            migrationBuilder.RenameColumn(
                name: "ItemPackingUnitPriceId",
                table: "ItemPackingUnitSellingPrices",
                newName: "ItemPackingUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemPackingUnitSellingPrices_sellingPriceId",
                table: "ItemPackingUnitSellingPrices",
                newName: "IX_ItemPackingUnitSellingPrices_SellingPriceId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemPackingUnitSellingPrices_ItemPackingUnitPriceId_sellingPriceId",
                table: "ItemPackingUnitSellingPrices",
                newName: "IX_ItemPackingUnitSellingPrices_ItemPackingUnitId_SellingPriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPackingUnitSellingPrices_ItemPackingUnits_ItemPackingUnitId",
                table: "ItemPackingUnitSellingPrices",
                column: "ItemPackingUnitId",
                principalTable: "ItemPackingUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPackingUnitSellingPrices_SellingPrices_SellingPriceId",
                table: "ItemPackingUnitSellingPrices",
                column: "SellingPriceId",
                principalTable: "SellingPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPackingUnitSellingPrices_ItemPackingUnits_ItemPackingUnitId",
                table: "ItemPackingUnitSellingPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemPackingUnitSellingPrices_SellingPrices_SellingPriceId",
                table: "ItemPackingUnitSellingPrices");

            migrationBuilder.RenameColumn(
                name: "SellingPriceId",
                table: "ItemPackingUnitSellingPrices",
                newName: "sellingPriceId");

            migrationBuilder.RenameColumn(
                name: "ItemPackingUnitId",
                table: "ItemPackingUnitSellingPrices",
                newName: "ItemPackingUnitPriceId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemPackingUnitSellingPrices_SellingPriceId",
                table: "ItemPackingUnitSellingPrices",
                newName: "IX_ItemPackingUnitSellingPrices_sellingPriceId");

            migrationBuilder.RenameIndex(
                name: "IX_ItemPackingUnitSellingPrices_ItemPackingUnitId_SellingPriceId",
                table: "ItemPackingUnitSellingPrices",
                newName: "IX_ItemPackingUnitSellingPrices_ItemPackingUnitPriceId_sellingPriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPackingUnitSellingPrices_ItemPackingUnits_ItemPackingUnitPriceId",
                table: "ItemPackingUnitSellingPrices",
                column: "ItemPackingUnitPriceId",
                principalTable: "ItemPackingUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPackingUnitSellingPrices_SellingPrices_sellingPriceId",
                table: "ItemPackingUnitSellingPrices",
                column: "sellingPriceId",
                principalTable: "SellingPrices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
