using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class PreferenceUpdateTransactionType2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 62L,
                column: "Key",
                value: "DefaultCustomer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 62L,
                column: "Key",
                value: "DefaultSupplier");
        }
    }
}
