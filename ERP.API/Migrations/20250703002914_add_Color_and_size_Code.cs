using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AAA.ERP.Migrations
{
    public partial class add_Color_and_size_Code : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ColorCode",
                table: "Colors",
                newName: "ColorValue");

            migrationBuilder.RenameIndex(
                name: "IX_Colors_ColorCode",
                table: "Colors",
                newName: "IX_Colors_ColorValue");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Sizes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "ColorValue",
                table: "Colors",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("Relational:ColumnOrder", 4)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Colors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 3);

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_Code",
                table: "Sizes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Colors_Code",
                table: "Colors",
                column: "Code",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sizes_Code",
                table: "Sizes");

            migrationBuilder.DropIndex(
                name: "IX_Colors_Code",
                table: "Colors");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Sizes");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Colors");

            migrationBuilder.RenameColumn(
                name: "ColorValue",
                table: "Colors",
                newName: "ColorCode");

            migrationBuilder.RenameIndex(
                name: "IX_Colors_ColorValue",
                table: "Colors",
                newName: "IX_Colors_ColorCode");

            migrationBuilder.AlterColumn<string>(
                name: "ColorCode",
                table: "Colors",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)")
                .Annotation("Relational:ColumnOrder", 3)
                .OldAnnotation("Relational:ColumnOrder", 4);
        }
    }
}
