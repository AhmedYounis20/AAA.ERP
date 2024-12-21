using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class update_branches_changeLogoDeleteBehavior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Attachments_AttachmentId",
                table: "Branches");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Attachments_AttachmentId",
                table: "Branches",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Attachments_AttachmentId",
                table: "Branches");

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Attachments_AttachmentId",
                table: "Branches",
                column: "AttachmentId",
                principalTable: "Attachments",
                principalColumn: "Id");
        }
    }
}
