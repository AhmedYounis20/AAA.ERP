using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoveSubLedgerBaseAccountBinding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubLedgerBaseAccounts",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChartOfAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubLedgerType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FixedAssetType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubLedgerBaseAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubLedgerBaseAccounts_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalSchema: "accounting",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubLedgerBaseAccounts_ChartOfAccountId",
                schema: "accounting",
                table: "SubLedgerBaseAccounts",
                column: "ChartOfAccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubLedgerBaseAccounts_CreatedAt",
                schema: "accounting",
                table: "SubLedgerBaseAccounts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_SubLedgerBaseAccounts_SubLedgerType_Role_FixedAssetType",
                schema: "accounting",
                table: "SubLedgerBaseAccounts",
                columns: new[] { "SubLedgerType", "Role", "FixedAssetType" },
                unique: true,
                filter: "[FixedAssetType] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SubLedgerBaseAccounts_SubLedgerType_Role",
                schema: "accounting",
                table: "SubLedgerBaseAccounts",
                columns: new[] { "SubLedgerType", "Role" },
                unique: true,
                filter: "[FixedAssetType] IS NULL");

            migrationBuilder.Sql("""
                INSERT INTO accounting.SubLedgerBaseAccounts
                    (Id, ChartOfAccountId, SubLedgerType, Role, FixedAssetType, CreatedAt, ModifiedAt)
                SELECT
                    Id,
                    Id,
                    SubLeadgerType,
                    ISNULL(SubLedgerAccountRole, 'Default'),
                    FixedAssetType,
                    SYSUTCDATETIME(),
                    SYSUTCDATETIME()
                FROM accounting.ChartOfAccounts
                WHERE IsSubLeadgerBaseAccount = 1
                  AND SubLeadgerType IS NOT NULL;
                """);

            migrationBuilder.DropIndex(
                name: "IX_ChartOfAccounts_IsSubLeadgerBaseAccount_SubLeadgerType_FixedAssetType_SubLedgerAccountRole",
                schema: "accounting",
                table: "ChartOfAccounts");

            migrationBuilder.DropColumn(
                name: "FixedAssetType",
                schema: "accounting",
                table: "ChartOfAccounts");

            migrationBuilder.DropColumn(
                name: "IsSubLeadgerBaseAccount",
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
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 9)
                .OldAnnotation("Relational:ColumnOrder", 10);

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentId",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 13)
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
                .Annotation("Relational:ColumnOrder", 11)
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
                .Annotation("Relational:ColumnOrder", 10)
                .OldAnnotation("Relational:ColumnOrder", 13);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 12)
                .OldAnnotation("Relational:ColumnOrder", 15);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FixedAssetType",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSubLeadgerBaseAccount",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SubLedgerAccountRole",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE account
                SET
                    account.IsSubLeadgerBaseAccount = 1,
                    account.SubLeadgerType = binding.SubLedgerType,
                    account.SubLedgerAccountRole = binding.Role,
                    account.FixedAssetType = binding.FixedAssetType
                FROM accounting.ChartOfAccounts AS account
                INNER JOIN accounting.SubLedgerBaseAccounts AS binding
                    ON binding.ChartOfAccountId = account.Id;
                """);

            migrationBuilder.DropTable(
                name: "SubLedgerBaseAccounts",
                schema: "accounting");

            migrationBuilder.AlterColumn<string>(
                name: "SubLeadgerType",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 10)
                .OldAnnotation("Relational:ColumnOrder", 9);

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
                .OldAnnotation("Relational:ColumnOrder", 13);

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
                .OldAnnotation("Relational:ColumnOrder", 11);

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
                .OldAnnotation("Relational:ColumnOrder", 10);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                schema: "accounting",
                table: "ChartOfAccounts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 15)
                .OldAnnotation("Relational:ColumnOrder", 12);

            migrationBuilder.CreateIndex(
                name: "IX_ChartOfAccounts_IsSubLeadgerBaseAccount_SubLeadgerType_FixedAssetType_SubLedgerAccountRole",
                schema: "accounting",
                table: "ChartOfAccounts",
                columns: new[] { "IsSubLeadgerBaseAccount", "SubLeadgerType", "FixedAssetType", "SubLedgerAccountRole" },
                unique: true,
                filter: "[IsSubLeadgerBaseAccount] = 1");
        }
    }
}
