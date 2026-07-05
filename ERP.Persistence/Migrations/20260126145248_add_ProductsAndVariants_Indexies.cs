using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_ProductsAndVariants_Indexies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Variants_ProductId",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_VariantAttachments_VariantId",
                table: "VariantAttachments");

            migrationBuilder.DropIndex(
                name: "IX_Products_ParentId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttachments_ProductId",
                table: "ProductAttachments");

            migrationBuilder.AlterColumn<string>(
                name: "CodeType",
                table: "VariantCodes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProductType",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CodeType",
                table: "ProductCodes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Variants_IsArchived",
                table: "Variants",
                column: "IsArchived");

            migrationBuilder.CreateIndex(
                name: "IX_Variants_IsArchived_ProductId",
                table: "Variants",
                columns: new[] { "IsArchived", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_Variants_IsDefault",
                table: "Variants",
                column: "IsDefault");

            migrationBuilder.CreateIndex(
                name: "IX_Variants_ProductId_IsDefault",
                table: "Variants",
                columns: new[] { "ProductId", "IsDefault" });

            migrationBuilder.CreateIndex(
                name: "IX_VariantCodes_CodeType",
                table: "VariantCodes",
                column: "CodeType");

            migrationBuilder.CreateIndex(
                name: "IX_VariantAttachments_IsPrimary",
                table: "VariantAttachments",
                column: "IsPrimary");

            migrationBuilder.CreateIndex(
                name: "IX_VariantAttachments_VariantId_IsPrimary",
                table: "VariantAttachments",
                columns: new[] { "VariantId", "IsPrimary" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_NodeType",
                table: "Products",
                column: "NodeType");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ParentId_NodeType",
                table: "Products",
                columns: new[] { "ParentId", "NodeType" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductType",
                table: "Products",
                column: "ProductType");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCodes_CodeType",
                table: "ProductCodes",
                column: "CodeType");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttachments_IsPrimary",
                table: "ProductAttachments",
                column: "IsPrimary");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttachments_ProductId_IsPrimary",
                table: "ProductAttachments",
                columns: new[] { "ProductId", "IsPrimary" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Variants_IsArchived",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_Variants_IsArchived_ProductId",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_Variants_IsDefault",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_Variants_ProductId_IsDefault",
                table: "Variants");

            migrationBuilder.DropIndex(
                name: "IX_VariantCodes_CodeType",
                table: "VariantCodes");

            migrationBuilder.DropIndex(
                name: "IX_VariantAttachments_IsPrimary",
                table: "VariantAttachments");

            migrationBuilder.DropIndex(
                name: "IX_VariantAttachments_VariantId_IsPrimary",
                table: "VariantAttachments");

            migrationBuilder.DropIndex(
                name: "IX_Products_NodeType",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ParentId_NodeType",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ProductType",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductCodes_CodeType",
                table: "ProductCodes");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttachments_IsPrimary",
                table: "ProductAttachments");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttachments_ProductId_IsPrimary",
                table: "ProductAttachments");

            migrationBuilder.AlterColumn<string>(
                name: "CodeType",
                table: "VariantCodes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "ProductType",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CodeType",
                table: "ProductCodes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Variants_ProductId",
                table: "Variants",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_VariantAttachments_VariantId",
                table: "VariantAttachments",
                column: "VariantId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ParentId",
                table: "Products",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttachments_ProductId",
                table: "ProductAttachments",
                column: "ProductId");
        }
    }
}
