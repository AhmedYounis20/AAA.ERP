using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ERP.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MoveAttachmentsToPlatformSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Attachments",
                schema: "accounting",
                newName: "Attachments",
                newSchema: "platform");

            migrationBuilder.AlterColumn<byte[]>(
                name: "FileData",
                schema: "platform",
                table: "Attachments",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddColumn<string>(
                name: "StorageKey",
                schema: "platform",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StorageProvider",
                schema: "platform",
                table: "Attachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "UPDATE [platform].[Attachments] SET [StorageKey] = CONVERT(nvarchar(36), [Id]) WHERE [StorageKey] IS NULL;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageKey",
                schema: "platform",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "StorageProvider",
                schema: "platform",
                table: "Attachments");

            migrationBuilder.RenameTable(
                name: "Attachments",
                schema: "platform",
                newName: "Attachments",
                newSchema: "accounting");

            migrationBuilder.AlterColumn<byte[]>(
                name: "FileData",
                schema: "accounting",
                table: "Attachments",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);
        }
    }
}
