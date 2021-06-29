using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class AddCurrenctInInvoicesAndFinancial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CurrencyId",
                table: "Invoice",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "CurrencyId",
                table: "Financial",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_CurrencyId",
                table: "Invoice",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Financial_CurrencyId",
                table: "Financial",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Financial_Currency_CurrencyId",
                table: "Financial",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Currency_CurrencyId",
                table: "Invoice",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Financial_Currency_CurrencyId",
                table: "Financial");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Currency_CurrencyId",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_CurrencyId",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Financial_CurrencyId",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Financial");
        }
    }
}
