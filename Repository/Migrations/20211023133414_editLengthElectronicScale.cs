using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class editLengthElectronicScale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 19L,
                column: "Value",
                value: "7");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 119L,
                column: "Value",
                value: "7");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 19L,
                column: "Value",
                value: "4");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 119L,
                column: "Value",
                value: "4");
        }
    }
}
