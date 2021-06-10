using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class GroupInvoiceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "InvoiceType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Group",
                value: "Sales");

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Group",
                value: "Purchases");

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Group",
                value: "Sales");

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Group",
                value: "Purchases");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Group",
                table: "InvoiceType");
        }
    }
}
