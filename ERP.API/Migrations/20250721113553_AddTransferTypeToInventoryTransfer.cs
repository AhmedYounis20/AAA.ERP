using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class AddTransferTypeToInventoryTransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TransferType",
                table: "InventoryTransfers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransferType",
                table: "InventoryTransfers");
        }
    }
}
