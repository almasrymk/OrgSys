using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class UnspecifiedType0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Dealer",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Name", "TypeId" },
                values: new object[] { "...", 0L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Dealer",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Name", "TypeId" },
                values: new object[] { "Unspecified", 1L });
        }
    }
}
