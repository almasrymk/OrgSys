using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class PreferenceUpdateTransactionType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "InOut",
                value: 1);

            migrationBuilder.UpdateData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "InOut",
                value: -1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "InOut",
                value: -1);

            migrationBuilder.UpdateData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "InOut",
                value: 1);
        }
    }
}
