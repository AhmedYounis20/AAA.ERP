using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddProductAndVariantSalesPurchaseTaxes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductPurchaseTaxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPurchaseTaxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPurchaseTaxes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSalesTaxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSalesTaxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSalesTaxes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VariantPurchaseTaxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VariantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariantPurchaseTaxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariantPurchaseTaxes_Variants_VariantId",
                        column: x => x.VariantId,
                        principalTable: "Variants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VariantSalesTaxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VariantId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VariantSalesTaxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VariantSalesTaxes_Variants_VariantId",
                        column: x => x.VariantId,
                        principalTable: "Variants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPurchaseTaxes_CreatedAt",
                table: "ProductPurchaseTaxes",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPurchaseTaxes_ProductId_TaxId",
                table: "ProductPurchaseTaxes",
                columns: new[] { "ProductId", "TaxId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductSalesTaxes_CreatedAt",
                table: "ProductSalesTaxes",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSalesTaxes_ProductId_TaxId",
                table: "ProductSalesTaxes",
                columns: new[] { "ProductId", "TaxId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VariantPurchaseTaxes_CreatedAt",
                table: "VariantPurchaseTaxes",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_VariantPurchaseTaxes_VariantId_TaxId",
                table: "VariantPurchaseTaxes",
                columns: new[] { "VariantId", "TaxId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VariantSalesTaxes_CreatedAt",
                table: "VariantSalesTaxes",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_VariantSalesTaxes_VariantId_TaxId",
                table: "VariantSalesTaxes",
                columns: new[] { "VariantId", "TaxId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPurchaseTaxes");

            migrationBuilder.DropTable(
                name: "ProductSalesTaxes");

            migrationBuilder.DropTable(
                name: "VariantPurchaseTaxes");

            migrationBuilder.DropTable(
                name: "VariantSalesTaxes");
        }
    }
}
