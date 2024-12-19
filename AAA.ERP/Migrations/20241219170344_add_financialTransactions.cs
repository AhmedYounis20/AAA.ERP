using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class add_financialTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "ParentId",
                table: "ChartOfAccounts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 15)
                .OldAnnotation("Relational:ColumnOrder", 14);

            migrationBuilder.AlterColumn<string>(
                name: "NameSecondLanguage",
                table: "ChartOfAccounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 13)
                .OldAnnotation("Relational:ColumnOrder", 12);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ChartOfAccounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 12)
                .OldAnnotation("Relational:ColumnOrder", 11);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ChartOfAccounts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 14)
                .OldAnnotation("Relational:ColumnOrder", 13);

            migrationBuilder.AddColumn<string>(
                name: "RelatedPaymentType",
                table: "ChartOfAccounts",
                type: "nvarchar(max)",
                nullable: true)
                .Annotation("Relational:ColumnOrder", 11);

            migrationBuilder.CreateTable(
                name: "FinancialTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ChartOfAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountNature = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderNumber = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ComplementTransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChequeBankId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChequeNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChequeIssueDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChequeCollectionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CollectionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PromissoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromissoryNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromissoryIdentityCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromissoryCollectionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    WireTransferReferenceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditCardLastDigits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialTransactions_Banks_ChequeBankId",
                        column: x => x.ChequeBankId,
                        principalTable: "Banks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinancialTransactions_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialTransactions_Entries_EntryId",
                        column: x => x.EntryId,
                        principalTable: "Entries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialTransactions_Entries_EntryId1",
                        column: x => x.EntryId1,
                        principalTable: "Entries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinancialTransactions_FinancialTransactions_ComplementTransactionId",
                        column: x => x.ComplementTransactionId,
                        principalTable: "FinancialTransactions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactions_ChartOfAccountId",
                table: "FinancialTransactions",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactions_ChequeBankId",
                table: "FinancialTransactions",
                column: "ChequeBankId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactions_ComplementTransactionId",
                table: "FinancialTransactions",
                column: "ComplementTransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactions_EntryId",
                table: "FinancialTransactions",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransactions_EntryId1",
                table: "FinancialTransactions",
                column: "EntryId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialTransactions");

            migrationBuilder.DropColumn(
                name: "RelatedPaymentType",
                table: "ChartOfAccounts");

            migrationBuilder.AlterColumn<Guid>(
                name: "ParentId",
                table: "ChartOfAccounts",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 14)
                .OldAnnotation("Relational:ColumnOrder", 15);

            migrationBuilder.AlterColumn<string>(
                name: "NameSecondLanguage",
                table: "ChartOfAccounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 12)
                .OldAnnotation("Relational:ColumnOrder", 13);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ChartOfAccounts",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100)
                .Annotation("Relational:ColumnOrder", 11)
                .OldAnnotation("Relational:ColumnOrder", 12);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ChartOfAccounts",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier")
                .Annotation("Relational:ColumnOrder", 13)
                .OldAnnotation("Relational:ColumnOrder", 14);
        }
    }
}
