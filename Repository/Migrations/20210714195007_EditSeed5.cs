using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class EditSeed5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10102L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "Stores.All", "Stores" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10203L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "Shifts.All", "Shifts" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010201L,
                column: "Key",
                value: "Stores.View");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010202L,
                column: "Key",
                value: "Stores.Add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010203L,
                column: "Key",
                value: "Stores.Edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010204L,
                column: "Key",
                value: "Stores.Delete");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020301L,
                column: "Key",
                value: "Shifts.View");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020302L,
                column: "Key",
                value: "Shifts.Add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020303L,
                column: "Key",
                value: "Shifts.Edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020304L,
                column: "Key",
                value: "Shifts.Delete");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10102L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "Store.All", "Store" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10203L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "Sfilts.All", "Sfilts" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010201L,
                column: "Key",
                value: "Store.View");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010202L,
                column: "Key",
                value: "Store.Add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010203L,
                column: "Key",
                value: "Store.Edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010204L,
                column: "Key",
                value: "Store.Delete");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020301L,
                column: "Key",
                value: "Sfilts.View");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020302L,
                column: "Key",
                value: "Sfilts.Add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020303L,
                column: "Key",
                value: "Sfilts.Edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020304L,
                column: "Key",
                value: "Sfilts.Delete");
        }
    }
}
