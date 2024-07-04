using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class AddFixedAssets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bank_Bank_ParentId",
                table: "Bank");

            migrationBuilder.DropForeignKey(
                name: "FK_Bank_ChartOfAccounts_ChartOfAccountId",
                table: "Bank");

            migrationBuilder.DropForeignKey(
                name: "FK_CashInBox_CashInBox_ParentId",
                table: "CashInBox");

            migrationBuilder.DropForeignKey(
                name: "FK_CashInBox_ChartOfAccounts_ChartOfAccountId",
                table: "CashInBox");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_ChartOfAccounts_ChartOfAccountId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Customer_ParentId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Supplier_ChartOfAccounts_ChartOfAccountId",
                table: "Supplier");

            migrationBuilder.DropForeignKey(
                name: "FK_Supplier_Supplier_ParentId",
                table: "Supplier");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Supplier",
                table: "Supplier");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CashInBox",
                table: "CashInBox");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bank",
                table: "Bank");

            migrationBuilder.RenameTable(
                name: "Supplier",
                newName: "Suppliers");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.RenameTable(
                name: "CashInBox",
                newName: "CashInBoxes");

            migrationBuilder.RenameTable(
                name: "Bank",
                newName: "Banks");

            migrationBuilder.RenameIndex(
                name: "IX_Supplier_ParentId",
                table: "Suppliers",
                newName: "IX_Suppliers_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Supplier_NameSecondLanguage",
                table: "Suppliers",
                newName: "IX_Suppliers_NameSecondLanguage");

            migrationBuilder.RenameIndex(
                name: "IX_Supplier_Name",
                table: "Suppliers",
                newName: "IX_Suppliers_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Supplier_ChartOfAccountId",
                table: "Suppliers",
                newName: "IX_Suppliers_ChartOfAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_ParentId",
                table: "Customers",
                newName: "IX_Customers_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_NameSecondLanguage",
                table: "Customers",
                newName: "IX_Customers_NameSecondLanguage");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_Name",
                table: "Customers",
                newName: "IX_Customers_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_ChartOfAccountId",
                table: "Customers",
                newName: "IX_Customers_ChartOfAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_CashInBox_ParentId",
                table: "CashInBoxes",
                newName: "IX_CashInBoxes_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_CashInBox_NameSecondLanguage",
                table: "CashInBoxes",
                newName: "IX_CashInBoxes_NameSecondLanguage");

            migrationBuilder.RenameIndex(
                name: "IX_CashInBox_Name",
                table: "CashInBoxes",
                newName: "IX_CashInBoxes_Name");

            migrationBuilder.RenameIndex(
                name: "IX_CashInBox_ChartOfAccountId",
                table: "CashInBoxes",
                newName: "IX_CashInBoxes_ChartOfAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Bank_ParentId",
                table: "Banks",
                newName: "IX_Banks_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Bank_NameSecondLanguage",
                table: "Banks",
                newName: "IX_Banks_NameSecondLanguage");

            migrationBuilder.RenameIndex(
                name: "IX_Bank_Name",
                table: "Banks",
                newName: "IX_Banks_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Bank_ChartOfAccountId",
                table: "Banks",
                newName: "IX_Banks_ChartOfAccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Suppliers",
                table: "Suppliers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CashInBoxes",
                table: "CashInBoxes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Banks",
                table: "Banks",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FixedAssets",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChartOfAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Serial = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Model = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Version = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ManufactureCompany = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ExpensesAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AccumlatedAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FixedAssetType = table.Column<int>(type: "int", nullable: true),
                    IsDepreciable = table.Column<bool>(type: "bit", nullable: false),
                    AssetLifeSpanByYears = table.Column<int>(type: "int", nullable: false),
                    DepreciationRate = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NodeType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixedAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedAssets_ChartOfAccounts_AccumlatedAccountId",
                        column: x => x.AccumlatedAccountId,
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FixedAssets_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FixedAssets_ChartOfAccounts_ExpensesAccountId",
                        column: x => x.ExpensesAccountId,
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FixedAssets_FixedAssets_ParentId",
                        column: x => x.ParentId,
                        principalTable: "FixedAssets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_AccumlatedAccountId",
                table: "FixedAssets",
                column: "AccumlatedAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_ChartOfAccountId",
                table: "FixedAssets",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_ExpensesAccountId",
                table: "FixedAssets",
                column: "ExpensesAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_Name",
                table: "FixedAssets",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_NameSecondLanguage",
                table: "FixedAssets",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_ParentId",
                table: "FixedAssets",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Banks_Banks_ParentId",
                table: "Banks",
                column: "ParentId",
                principalTable: "Banks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Banks_ChartOfAccounts_ChartOfAccountId",
                table: "Banks",
                column: "ChartOfAccountId",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashInBoxes_CashInBoxes_ParentId",
                table: "CashInBoxes",
                column: "ParentId",
                principalTable: "CashInBoxes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashInBoxes_ChartOfAccounts_ChartOfAccountId",
                table: "CashInBoxes",
                column: "ChartOfAccountId",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_ChartOfAccounts_ChartOfAccountId",
                table: "Customers",
                column: "ChartOfAccountId",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Customers_ParentId",
                table: "Customers",
                column: "ParentId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_ChartOfAccounts_ChartOfAccountId",
                table: "Suppliers",
                column: "ChartOfAccountId",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Suppliers_ParentId",
                table: "Suppliers",
                column: "ParentId",
                principalTable: "Suppliers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Banks_Banks_ParentId",
                table: "Banks");

            migrationBuilder.DropForeignKey(
                name: "FK_Banks_ChartOfAccounts_ChartOfAccountId",
                table: "Banks");

            migrationBuilder.DropForeignKey(
                name: "FK_CashInBoxes_CashInBoxes_ParentId",
                table: "CashInBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_CashInBoxes_ChartOfAccounts_ChartOfAccountId",
                table: "CashInBoxes");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_ChartOfAccounts_ChartOfAccountId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Customers_ParentId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_ChartOfAccounts_ChartOfAccountId",
                table: "Suppliers");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Suppliers_ParentId",
                table: "Suppliers");

            migrationBuilder.DropTable(
                name: "FixedAssets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Suppliers",
                table: "Suppliers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CashInBoxes",
                table: "CashInBoxes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Banks",
                table: "Banks");

            migrationBuilder.RenameTable(
                name: "Suppliers",
                newName: "Supplier");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.RenameTable(
                name: "CashInBoxes",
                newName: "CashInBox");

            migrationBuilder.RenameTable(
                name: "Banks",
                newName: "Bank");

            migrationBuilder.RenameIndex(
                name: "IX_Suppliers_ParentId",
                table: "Supplier",
                newName: "IX_Supplier_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Suppliers_NameSecondLanguage",
                table: "Supplier",
                newName: "IX_Supplier_NameSecondLanguage");

            migrationBuilder.RenameIndex(
                name: "IX_Suppliers_Name",
                table: "Supplier",
                newName: "IX_Supplier_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Suppliers_ChartOfAccountId",
                table: "Supplier",
                newName: "IX_Supplier_ChartOfAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_ParentId",
                table: "Customer",
                newName: "IX_Customer_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_NameSecondLanguage",
                table: "Customer",
                newName: "IX_Customer_NameSecondLanguage");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_Name",
                table: "Customer",
                newName: "IX_Customer_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_ChartOfAccountId",
                table: "Customer",
                newName: "IX_Customer_ChartOfAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_CashInBoxes_ParentId",
                table: "CashInBox",
                newName: "IX_CashInBox_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_CashInBoxes_NameSecondLanguage",
                table: "CashInBox",
                newName: "IX_CashInBox_NameSecondLanguage");

            migrationBuilder.RenameIndex(
                name: "IX_CashInBoxes_Name",
                table: "CashInBox",
                newName: "IX_CashInBox_Name");

            migrationBuilder.RenameIndex(
                name: "IX_CashInBoxes_ChartOfAccountId",
                table: "CashInBox",
                newName: "IX_CashInBox_ChartOfAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Banks_ParentId",
                table: "Bank",
                newName: "IX_Bank_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Banks_NameSecondLanguage",
                table: "Bank",
                newName: "IX_Bank_NameSecondLanguage");

            migrationBuilder.RenameIndex(
                name: "IX_Banks_Name",
                table: "Bank",
                newName: "IX_Bank_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Banks_ChartOfAccountId",
                table: "Bank",
                newName: "IX_Bank_ChartOfAccountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Supplier",
                table: "Supplier",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CashInBox",
                table: "CashInBox",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bank",
                table: "Bank",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bank_Bank_ParentId",
                table: "Bank",
                column: "ParentId",
                principalTable: "Bank",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bank_ChartOfAccounts_ChartOfAccountId",
                table: "Bank",
                column: "ChartOfAccountId",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashInBox_CashInBox_ParentId",
                table: "CashInBox",
                column: "ParentId",
                principalTable: "CashInBox",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CashInBox_ChartOfAccounts_ChartOfAccountId",
                table: "CashInBox",
                column: "ChartOfAccountId",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_ChartOfAccounts_ChartOfAccountId",
                table: "Customer",
                column: "ChartOfAccountId",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Customer_ParentId",
                table: "Customer",
                column: "ParentId",
                principalTable: "Customer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Supplier_ChartOfAccounts_ChartOfAccountId",
                table: "Supplier",
                column: "ChartOfAccountId",
                principalTable: "ChartOfAccounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Supplier_Supplier_ParentId",
                table: "Supplier",
                column: "ParentId",
                principalTable: "Supplier",
                principalColumn: "Id");
        }
    }
}
