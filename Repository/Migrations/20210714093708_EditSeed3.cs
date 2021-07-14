using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class EditSeed3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10104L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010401L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010402L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010403L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010404L);

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10501L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "Safes.All", "Safes" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10502L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "OutlayTerms.All", "Outlay Terms" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050101L,
                column: "Key",
                value: "Safes.View");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050102L,
                column: "Key",
                value: "Safes.Add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050103L,
                column: "Key",
                value: "Safes.Edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050104L,
                column: "Key",
                value: "Safes.Delete");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050201L,
                column: "Key",
                value: "OutlayTerms.View");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050202L,
                column: "Key",
                value: "OutlayTerms.Add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050203L,
                column: "Key",
                value: "OutlayTerms.Edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050204L,
                column: "Key",
                value: "OutlayTerms.Delete");

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "Name", "Name2", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 10503L, false, null, "Currencies.All", null, "Currencies", null, 105L, 0, 0L },
                    { 1050301L, false, null, "Currencies.View", null, "View", null, 10503L, 0, 1L },
                    { 1050302L, false, null, "Currencies.Add", null, "Add", null, 10503L, 0, 1L },
                    { 1050303L, false, null, "Currencies.Edit", null, "Edit", null, 10503L, 0, 1L },
                    { 1050304L, false, null, "Currencies.Delete", null, "Delete", null, 10503L, 0, 1L }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10503L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050301L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050302L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050303L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050304L);

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10501L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "OutlayTerms.All", "Outlay Terms" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10502L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "Currencies.All", "Currencies" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050101L,
                column: "Key",
                value: "OutlayTerms.View");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050102L,
                column: "Key",
                value: "OutlayTerms.Add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050103L,
                column: "Key",
                value: "OutlayTerms.Edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050104L,
                column: "Key",
                value: "OutlayTerms.Delete");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050201L,
                column: "Key",
                value: "Currencies.View");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050202L,
                column: "Key",
                value: "Currencies.Add");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050203L,
                column: "Key",
                value: "Currencies.Edit");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050204L,
                column: "Key",
                value: "Currencies.Delete");

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "Name", "Name2", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 10104L, false, null, "Safes.All", null, "Safes", null, 101L, 0, 0L },
                    { 1010401L, false, null, "Safes.View", null, "View", null, 10104L, 0, 1L },
                    { 1010402L, false, null, "Safes.Add", null, "Add", null, 10104L, 0, 1L },
                    { 1010403L, false, null, "Safes.Edit", null, "Edit", null, 10104L, 0, 1L },
                    { 1010404L, false, null, "Safes.Delete", null, "Delete", null, 10104L, 0, 1L }
                });
        }
    }
}
