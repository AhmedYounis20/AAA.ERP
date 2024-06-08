using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class AddBanks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NameSecondLanguage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ChartOfAccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    BankAccount = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    BankAddress = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ModifiedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NodeType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bank_Bank_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Bank",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bank_ChartOfAccounts_ChartOfAccountId",
                        column: x => x.ChartOfAccountId,
                        principalTable: "ChartOfAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bank_ChartOfAccountId",
                table: "Bank",
                column: "ChartOfAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Bank_Name",
                table: "Bank",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bank_NameSecondLanguage",
                table: "Bank",
                column: "NameSecondLanguage",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bank_ParentId",
                table: "Bank",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bank");
        }
    }
}
