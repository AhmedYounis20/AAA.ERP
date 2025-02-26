using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class addSubLeadgerTypetoChartOfAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RelatedPaymentType",
                table: "ChartOfAccounts",
                newName: "SubLeadgerType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubLeadgerType",
                table: "ChartOfAccounts",
                newName: "RelatedPaymentType");
        }
    }
}
