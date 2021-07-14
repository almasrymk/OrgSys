using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class EditSeed4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 201L,
                column: "Name",
                value: "Order Notices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 201L,
                column: "Name",
                value: "Orders");
        }
    }
}
