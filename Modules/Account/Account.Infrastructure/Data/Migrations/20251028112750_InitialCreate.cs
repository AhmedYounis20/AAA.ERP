using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Account.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Account");

            migrationBuilder.CreateTable(
                name: "AccountGuides",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountGuides", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileData = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CollectionBooks",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionBooks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CostCenters",
                schema: "Account",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Percent = table.Column<int>(type: "int", nullable: false),
                    CostCenterType = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NodeType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostCenters_CostCenters_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Account",
                        principalTable: "CostCenters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialPeriods",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    YearNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PeriodTypeByMonth = table.Column<byte>(type: "tinyint", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialPeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GLSettings",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAllowingEditVoucher = table.Column<bool>(type: "bit", nullable: false),
                    IsAllowingDeleteVoucher = table.Column<bool>(type: "bit", nullable: false),
                    IsAllowingNegativeBalances = table.Column<bool>(type: "bit", nullable: false),
                    DecimalDigitsNumber = table.Column<byte>(type: "tinyint", nullable: false),
                    DepreciationApplication = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MonthDays = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GLSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Commission = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChartOfAccounts",
                schema: "Account",
                columns: table => new
                {
                    AccountGuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    AccountNature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDepreciable = table.Column<bool>(type: "bit", nullable: false),
                    IsPostedAccount = table.Column<bool>(type: "bit", nullable: false),
                    IsStopDealing = table.Column<bool>(type: "bit", nullable: false),
                    IsActiveAccount = table.Column<bool>(type: "bit", nullable: false),
                    IsCreatedFromSubLeadger = table.Column<bool>(type: "bit", nullable: false),
                    IsSubLeadgerBaseAccount = table.Column<bool>(type: "bit", nullable: false),
                    SubLeadgerType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NodeType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChartOfAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChartOfAccounts_AccountGuides_AccountGuidId",
                        column: x => x.AccountGuidId,
                        principalSchema: "Account",
                        principalTable: "AccountGuides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChartOfAccounts_ChartOfAccounts_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Account",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Banks",
                schema: "Account",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChartOfAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    BankAccount = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    BankAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NodeType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Banks_Banks_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Account",
                        principalTable: "Banks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Banks_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalSchema: "Account",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Branches",
                schema: "Account",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChartOfAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NodeType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalSchema: "Account",
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Branches_Branches_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Account",
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Branches_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalSchema: "Account",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CashInBoxes",
                schema: "Account",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChartOfAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NodeType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashInBoxes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashInBoxes_CashInBoxes_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Account",
                        principalTable: "CashInBoxes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CashInBoxes_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalSchema: "Account",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CostCenterChartOfAccounts",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CostCenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChartOfAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCenterChartOfAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CostCenterChartOfAccounts_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalSchema: "Account",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CostCenterChartOfAccounts_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalSchema: "Account",
                        principalTable: "CostCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                schema: "Account",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChartOfAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NodeType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalSchema: "Account",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_Customers_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Account",
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FixedAssets",
                schema: "Account",
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
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NodeType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixedAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedAssets_ChartOfAccounts_AccumlatedAccountId",
                        column: x => x.AccumlatedAccountId,
                        principalSchema: "Account",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FixedAssets_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalSchema: "Account",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FixedAssets_ChartOfAccounts_ExpensesAccountId",
                        column: x => x.ExpensesAccountId,
                        principalSchema: "Account",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FixedAssets_FixedAssets_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Account",
                        principalTable: "FixedAssets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                schema: "Account",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChartOfAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CustomerType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    TaxNumber = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NodeType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suppliers_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalSchema: "Account",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Suppliers_Suppliers_ParentId",
                        column: x => x.ParentId,
                        principalSchema: "Account",
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Entries",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntryType = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FinancialPeriodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BranchId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Entries_Branches_BranchId",
                        column: x => x.BranchId,
                        principalSchema: "Account",
                        principalTable: "Branches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Entries_Branches_BranchId1",
                        column: x => x.BranchId1,
                        principalSchema: "Account",
                        principalTable: "Branches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entries_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: "Account",
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entries_Currencies_CurrencyId1",
                        column: x => x.CurrencyId1,
                        principalSchema: "Account",
                        principalTable: "Currencies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Entries_FinancialPeriods_FinancialPeriodId",
                        column: x => x.FinancialPeriodId,
                        principalSchema: "Account",
                        principalTable: "FinancialPeriods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryAttachments",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttachmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryAttachments_Attachments_AttachmentId",
                        column: x => x.AttachmentId,
                        principalSchema: "Account",
                        principalTable: "Attachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntryAttachments_Entries_EntryId",
                        column: x => x.EntryId,
                        principalSchema: "Account",
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryCostCenters",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CostCenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountNature = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryCostCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryCostCenters_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalSchema: "Account",
                        principalTable: "CostCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntryCostCenters_Entries_EntryId",
                        column: x => x.EntryId,
                        principalSchema: "Account",
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialTransactions",
                schema: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChartOfAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNature = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CollectionBookId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CashAgentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CashPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChequeBankId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChequeNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChequeIssueDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChequeCollectionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PromissoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromissoryNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromissoryCollectionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PromissoryIdentityCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WireTransferReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AtmReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditCardLastDigits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPaymentTransaction = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ComplementTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialTransactions_Banks_ChequeBankId",
                        column: x => x.ChequeBankId,
                        principalSchema: "Account",
                        principalTable: "Banks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinancialTransactions_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalSchema: "Account",
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialTransactions_CollectionBooks_CollectionBookId",
                        column: x => x.CollectionBookId,
                        principalSchema: "Account",
                        principalTable: "CollectionBooks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinancialTransactions_Entries_EntryId",
                        column: x => x.EntryId,
                        principalSchema: "Account",
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountGuides_Name",
                schema: "Account",
                table: "AccountGuides",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountGuides_NameSecondLanguage",
                schema: "Account",
                table: "AccountGuides",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banks_ChartOfAccountId",
                schema: "Account",
                table: "Banks",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_Name",
                schema: "Account",
                table: "Banks",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banks_NameSecondLanguage",
                schema: "Account",
                table: "Banks",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banks_ParentId",
                schema: "Account",
                table: "Banks",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_AttachmentId",
                schema: "Account",
                table: "Branches",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_ChartOfAccountId",
                schema: "Account",
                table: "Branches",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_Name",
                schema: "Account",
                table: "Branches",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_NameSecondLanguage",
                schema: "Account",
                table: "Branches",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Branches_ParentId",
                schema: "Account",
                table: "Branches",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CashInBoxes_ChartOfAccountId",
                schema: "Account",
                table: "CashInBoxes",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CashInBoxes_Name",
                schema: "Account",
                table: "CashInBoxes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashInBoxes_NameSecondLanguage",
                schema: "Account",
                table: "CashInBoxes",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashInBoxes_ParentId",
                schema: "Account",
                table: "CashInBoxes",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ChartOfAccounts_AccountGuidId",
                schema: "Account",
                table: "ChartOfAccounts",
                column: "AccountGuidId");

            migrationBuilder.CreateIndex(
                name: "IX_ChartOfAccounts_Code",
                schema: "Account",
                table: "ChartOfAccounts",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChartOfAccounts_Name",
                schema: "Account",
                table: "ChartOfAccounts",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChartOfAccounts_NameSecondLanguage",
                schema: "Account",
                table: "ChartOfAccounts",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChartOfAccounts_ParentId",
                schema: "Account",
                table: "ChartOfAccounts",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionBooks_Name",
                schema: "Account",
                table: "CollectionBooks",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollectionBooks_NameSecondLanguage",
                schema: "Account",
                table: "CollectionBooks",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostCenterChartOfAccounts_ChartOfAccountId_CostCenterId",
                schema: "Account",
                table: "CostCenterChartOfAccounts",
                columns: new[] { "ChartOfAccountId", "CostCenterId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostCenterChartOfAccounts_CostCenterId",
                schema: "Account",
                table: "CostCenterChartOfAccounts",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_Name",
                schema: "Account",
                table: "CostCenters",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_NameSecondLanguage",
                schema: "Account",
                table: "CostCenters",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CostCenters_ParentId",
                schema: "Account",
                table: "CostCenters",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Name",
                schema: "Account",
                table: "Currencies",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_NameSecondLanguage",
                schema: "Account",
                table: "Currencies",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Currencies_Symbol",
                schema: "Account",
                table: "Currencies",
                column: "Symbol",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ChartOfAccountId",
                schema: "Account",
                table: "Customers",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_Name",
                schema: "Account",
                table: "Customers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_NameSecondLanguage",
                schema: "Account",
                table: "Customers",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ParentId",
                schema: "Account",
                table: "Customers",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_BranchId",
                schema: "Account",
                table: "Entries",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_BranchId1",
                schema: "Account",
                table: "Entries",
                column: "BranchId1");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_CurrencyId",
                schema: "Account",
                table: "Entries",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_CurrencyId1",
                schema: "Account",
                table: "Entries",
                column: "CurrencyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_EntryType",
                schema: "Account",
                table: "Entries",
                column: "EntryType");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_FinancialPeriodId",
                schema: "Account",
                table: "Entries",
                column: "FinancialPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryAttachments_AttachmentId",
                schema: "Account",
                table: "EntryAttachments",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryAttachments_EntryId",
                schema: "Account",
                table: "EntryAttachments",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryCostCenters_CostCenterId",
                schema: "Account",
                table: "EntryCostCenters",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryCostCenters_EntryId",
                schema: "Account",
                table: "EntryCostCenters",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialPeriods_YearNumber",
                schema: "Account",
                table: "FinancialPeriods",
                column: "YearNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactions_ChartOfAccountId",
                schema: "Account",
                table: "FinancialTransactions",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactions_ChequeBankId",
                schema: "Account",
                table: "FinancialTransactions",
                column: "ChequeBankId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactions_CollectionBookId",
                schema: "Account",
                table: "FinancialTransactions",
                column: "CollectionBookId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactions_EntryId",
                schema: "Account",
                table: "FinancialTransactions",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_AccumlatedAccountId",
                schema: "Account",
                table: "FixedAssets",
                column: "AccumlatedAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_ChartOfAccountId",
                schema: "Account",
                table: "FixedAssets",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_ExpensesAccountId",
                schema: "Account",
                table: "FixedAssets",
                column: "ExpensesAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_Name",
                schema: "Account",
                table: "FixedAssets",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_NameSecondLanguage",
                schema: "Account",
                table: "FixedAssets",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FixedAssets_ParentId",
                schema: "Account",
                table: "FixedAssets",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                schema: "Account",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_NameSecondLanguage",
                schema: "Account",
                table: "Roles",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_ChartOfAccountId",
                schema: "Account",
                table: "Suppliers",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Name",
                schema: "Account",
                table: "Suppliers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_NameSecondLanguage",
                schema: "Account",
                table: "Suppliers",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_ParentId",
                schema: "Account",
                table: "Suppliers",
                column: "ParentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashInBoxes",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "CostCenterChartOfAccounts",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Customers",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "EntryAttachments",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "EntryCostCenters",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "FinancialTransactions",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "FixedAssets",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "GLSettings",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Suppliers",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "CostCenters",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Banks",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "CollectionBooks",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Entries",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Branches",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Currencies",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "FinancialPeriods",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "Attachments",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "ChartOfAccounts",
                schema: "Account");

            migrationBuilder.DropTable(
                name: "AccountGuides",
                schema: "Account");
        }
    }
}
