using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryThresholds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InventoryThresholdBranchId",
                table: "Variants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InventoryThresholdScope",
                table: "Variants",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InventoryThresholdBranchId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InventoryThresholdScope",
                table: "Products",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InventoryThresholds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerType = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VariantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Level = table.Column<int>(type: "int", nullable: false),
                    MinQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    MaxQuantity = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Scope = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EnableNotification = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryThresholds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryThresholds_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryThresholds_Variants_VariantId",
                        column: x => x.VariantId,
                        principalTable: "Variants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryThresholds_CreatedAt",
                table: "InventoryThresholds",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryThresholds_ProductId_Scope_BranchId",
                table: "InventoryThresholds",
                columns: new[] { "ProductId", "Scope", "BranchId" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryThresholds_VariantId_Scope_BranchId",
                table: "InventoryThresholds",
                columns: new[] { "VariantId", "Scope", "BranchId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryThresholds");

            migrationBuilder.DropColumn(
                name: "InventoryThresholdBranchId",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "InventoryThresholdScope",
                table: "Variants");

            migrationBuilder.DropColumn(
                name: "InventoryThresholdBranchId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "InventoryThresholdScope",
                table: "Products");
        }
    }
}
