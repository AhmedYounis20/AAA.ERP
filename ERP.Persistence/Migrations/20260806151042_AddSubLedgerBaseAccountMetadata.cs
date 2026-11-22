using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSubLedgerBaseAccountMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SubLeadgerType",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentId",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 16)
                .OldAnnotation("Relational:ColumnOrder", 14);

            migrationBuilder.AlterColumn<string>(
                name: "NameSecondLanguage",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 14)
                .OldAnnotation("Relational:ColumnOrder", 12);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 13)
                .OldAnnotation("Relational:ColumnOrder", 11);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 15)
                .OldAnnotation("Relational:ColumnOrder", 13);

            migrationBuilder.AddColumn<string>(
                name: "FixedAssetType",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "nvarchar(450)",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 11);

            migrationBuilder.AddColumn<string>(
                name: "SubLedgerAccountRole",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "nvarchar(450)",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 12);

            migrationBuilder.CreateIndex(
                name: "IX_ChartOfAccounts_IsSubLeadgerBaseAccount_SubLeadgerType_FixedAssetType_SubLedgerAccountRole",
                schema: "accounting",
                table: "ChartOfAccounts",
                columns: new[] { "IsSubLeadgerBaseAccount", "SubLeadgerType", "FixedAssetType", "SubLedgerAccountRole" },
                unique: true,
                filter: "[IsSubLeadgerBaseAccount] = 1");

            migrationBuilder.Sql("""
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'Bank', SubLedgerAccountRole = 'Default', IsSubLeadgerBaseAccount = 1 WHERE Id = '0aecd366-95fd-47b8-8516-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'Customer', SubLedgerAccountRole = 'Default', IsSubLeadgerBaseAccount = 1 WHERE Id = 'a8819d15-bae1-44ab-850d-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'CashInBox', SubLedgerAccountRole = 'Default', IsSubLeadgerBaseAccount = 1 WHERE Id = 'bc7492d9-f338-4b46-8515-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'Supplier', SubLedgerAccountRole = 'Default', IsSubLeadgerBaseAccount = 1 WHERE Id = '68cf0b75-21a6-4f8a-8554-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'Branch', SubLedgerAccountRole = 'Default', IsSubLeadgerBaseAccount = 1 WHERE Id = '0e9722ff-df3d-4937-8510-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'Branch', SubLedgerAccountRole = 'StockUnderTransfer', IsSubLeadgerBaseAccount = 1 WHERE Id = '6776b6a3-019c-480b-855b-08dc3dfcb069';

                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'Lands', SubLedgerAccountRole = 'Asset', IsSubLeadgerBaseAccount = 1 WHERE Id = '2448c162-2329-418f-8522-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'Buildings', SubLedgerAccountRole = 'Asset', IsSubLeadgerBaseAccount = 1 WHERE Id = '66d631a5-021d-4cb5-8523-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'Equipment', SubLedgerAccountRole = 'Asset', IsSubLeadgerBaseAccount = 1 WHERE Id = '8faa0f12-3b3e-4e26-8524-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'ElectronicDevices', SubLedgerAccountRole = 'Asset', IsSubLeadgerBaseAccount = 1 WHERE Id = 'c8b5cf24-98ab-414d-8525-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'OfficeSupplies', SubLedgerAccountRole = 'Asset', IsSubLeadgerBaseAccount = 1 WHERE Id = '2b70f4c6-3851-4967-8526-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'Furniture', SubLedgerAccountRole = 'Asset', IsSubLeadgerBaseAccount = 1 WHERE Id = 'cb9fd3bd-720e-45f0-8527-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'VehiclesAndTransportationItems', SubLedgerAccountRole = 'Asset', IsSubLeadgerBaseAccount = 1 WHERE Id = '8f2a2956-a327-4ec0-8528-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'Tools', SubLedgerAccountRole = 'Asset', IsSubLeadgerBaseAccount = 1 WHERE Id = '97a25e9d-dff3-4412-8529-08dc3dfcb068';

                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'Buildings', SubLedgerAccountRole = 'AccumulatedDepreciation', IsSubLeadgerBaseAccount = 1 WHERE Id = '86c0ab17-14f7-45cc-852b-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'Equipment', SubLedgerAccountRole = 'AccumulatedDepreciation', IsSubLeadgerBaseAccount = 1 WHERE Id = '11756e11-9861-4158-852c-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'ElectronicDevices', SubLedgerAccountRole = 'AccumulatedDepreciation', IsSubLeadgerBaseAccount = 1 WHERE Id = '6d116f0e-1d2a-4924-852d-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'OfficeSupplies', SubLedgerAccountRole = 'AccumulatedDepreciation', IsSubLeadgerBaseAccount = 1 WHERE Id = 'a29e405f-8a8a-42ff-852e-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'Furniture', SubLedgerAccountRole = 'AccumulatedDepreciation', IsSubLeadgerBaseAccount = 1 WHERE Id = 'aff2891b-f9e0-4824-852f-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'VehiclesAndTransportationItems', SubLedgerAccountRole = 'AccumulatedDepreciation', IsSubLeadgerBaseAccount = 1 WHERE Id = 'de0d3b5a-f655-429a-8530-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'Tools', SubLedgerAccountRole = 'AccumulatedDepreciation', IsSubLeadgerBaseAccount = 1 WHERE Id = '70b76b30-8307-481b-8531-08dc3dfcb068';

                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'Buildings', SubLedgerAccountRole = 'DepreciationExpense', IsSubLeadgerBaseAccount = 1 WHERE Id = 'd45672f5-5469-4349-8549-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'Equipment', SubLedgerAccountRole = 'DepreciationExpense', IsSubLeadgerBaseAccount = 1 WHERE Id = '42155321-78a3-4933-854a-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'ElectronicDevices', SubLedgerAccountRole = 'DepreciationExpense', IsSubLeadgerBaseAccount = 1 WHERE Id = 'f6e76628-2471-4a2b-854b-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'OfficeSupplies', SubLedgerAccountRole = 'DepreciationExpense', IsSubLeadgerBaseAccount = 1 WHERE Id = '5440be91-6ce8-4812-854c-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'Furniture', SubLedgerAccountRole = 'DepreciationExpense', IsSubLeadgerBaseAccount = 1 WHERE Id = '4cd75337-041f-4e76-854d-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'VehiclesAndTransportationItems', SubLedgerAccountRole = 'DepreciationExpense', IsSubLeadgerBaseAccount = 1 WHERE Id = 'b16bff28-ee42-45cb-854e-08dc3dfcb068';
                UPDATE accounting.ChartOfAccounts SET SubLeadgerType = 'FixedAsset', FixedAssetType = 'Tools', SubLedgerAccountRole = 'DepreciationExpense', IsSubLeadgerBaseAccount = 1 WHERE Id = '52b23a54-38df-4d2f-854f-08dc3dfcb068';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChartOfAccounts_IsSubLeadgerBaseAccount_SubLeadgerType_FixedAssetType_SubLedgerAccountRole",
                schema: "accounting",
                table: "ChartOfAccounts");

            migrationBuilder.DropColumn(
                name: "FixedAssetType",
                schema: "accounting",
                table: "ChartOfAccounts");

            migrationBuilder.DropColumn(
                name: "SubLedgerAccountRole",
                schema: "accounting",
                table: "ChartOfAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "SubLeadgerType",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentId",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 14)
                .OldAnnotation("Relational:ColumnOrder", 16);

            migrationBuilder.AlterColumn<string>(
                name: "NameSecondLanguage",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 12)
                .OldAnnotation("Relational:ColumnOrder", 14);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 11)
                .OldAnnotation("Relational:ColumnOrder", 13);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 13)
                .OldAnnotation("Relational:ColumnOrder", 15);
        }
    }
}
