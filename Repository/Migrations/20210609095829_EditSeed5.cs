using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class EditSeed5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Name",
                value: "Invoice");

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Name",
                value: "Invoice");

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Name",
                value: "Return");

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Name",
                value: "Return");

            migrationBuilder.UpdateData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Name",
                value: "Addition");

            migrationBuilder.UpdateData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Name",
                value: "Issue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Name",
                value: "Sales");

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Name",
                value: "Purchase");

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Name",
                value: "Return Sales");

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Name",
                value: "Return Purchase");

            migrationBuilder.UpdateData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Name",
                value: "Transaction In");

            migrationBuilder.UpdateData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Name",
                value: "Transaction Out");
        }
    }
}
