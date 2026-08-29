using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MergeFinancialTypeWithTransactionTypeEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Financial_CashBox_CashBoxId",
                table: "Financial");

            migrationBuilder.DropIndex(
                name: "IX_Financial_CashBoxId",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "CashBoxId",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "FinancialTransactionType",
                table: "Financial");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CashBoxId",
                table: "Financial",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "FinancialTransactionType",
                table: "Financial",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Financial_CashBoxId",
                table: "Financial",
                column: "CashBoxId");

            migrationBuilder.AddForeignKey(
                name: "FK_Financial_CashBox_CashBoxId",
                table: "Financial",
                column: "CashBoxId",
                principalTable: "CashBox",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
