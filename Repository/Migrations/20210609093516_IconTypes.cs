using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class IconTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "TransactionType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "InvoiceType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Icon",
                value: "simple-icon-basket-loaded");

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Icon",
                value: "simple-icon-basket-loaded");

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Icon",
                value: "simple-icon-action-undo");

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Icon",
                value: "simple-icon-action-undo");

            migrationBuilder.UpdateData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Icon",
                value: "iconsminds-down-1");

            migrationBuilder.UpdateData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Icon",
                value: "iconsminds-up-1");

            migrationBuilder.UpdateData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Icon",
                value: "iconsminds-shuffle-1");

            migrationBuilder.UpdateData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Icon",
                value: "iconsminds-file-edit");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "TransactionType");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "InvoiceType");
        }
    }
}
