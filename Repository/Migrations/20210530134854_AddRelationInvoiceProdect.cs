using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class AddRelationInvoiceProdect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                table: "InvoiceProduct",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceProduct_InvoiceId",
                table: "InvoiceProduct",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_InvoiceProduct_Invoice_InvoiceId",
                table: "InvoiceProduct",
                column: "InvoiceId",
                principalTable: "Invoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InvoiceProduct_Invoice_InvoiceId",
                table: "InvoiceProduct");

            migrationBuilder.DropIndex(
                name: "IX_InvoiceProduct_InvoiceId",
                table: "InvoiceProduct");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "InvoiceProduct");
        }
    }
}
