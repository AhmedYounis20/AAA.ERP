using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class AddCostCenterChartOfAccountIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CostCenterChartOfAccounts_ChartOfAccountId",
                table: "CostCenterChartOfAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenterChartOfAccounts_ChartOfAccountId_CostCenterId",
                table: "CostCenterChartOfAccounts",
                columns: new[] { "ChartOfAccountId", "CostCenterId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CostCenterChartOfAccounts_ChartOfAccountId_CostCenterId",
                table: "CostCenterChartOfAccounts");

            migrationBuilder.CreateIndex(
                name: "IX_CostCenterChartOfAccounts_ChartOfAccountId",
                table: "CostCenterChartOfAccounts",
                column: "ChartOfAccountId");
        }
    }
}
