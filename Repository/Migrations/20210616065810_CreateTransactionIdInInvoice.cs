using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class CreateTransactionIdInInvoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Invoice_InvoiceId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_InvoiceId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "InvoiceId",
                table: "Transaction");

            migrationBuilder.AddColumn<long>(
                name: "TransactionId",
                table: "Invoice",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_TransactionId",
                table: "Invoice",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Transaction_TransactionId",
                table: "Invoice",
                column: "TransactionId",
                principalTable: "Transaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Transaction_TransactionId",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_TransactionId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Invoice");

            migrationBuilder.AddColumn<long>(
                name: "InvoiceId",
                table: "Transaction",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_InvoiceId",
                table: "Transaction",
                column: "InvoiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Invoice_InvoiceId",
                table: "Transaction",
                column: "InvoiceId",
                principalTable: "Invoice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
