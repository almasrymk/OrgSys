using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class InsertPermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "ImgPath", "Key", "MaskText", "ParentId", "Status", "TypeId", "Value" },
                values: new object[,]
                {
                    { 101L, null, "Branchs", null, 10L, 0, 0L, "Branchs" },
                    { 10402L, null, "Add", null, 104L, 0, 0L, "Add" },
                    { 10401L, null, "View", null, 104L, 0, 0L, "View" },
                    { 104L, null, "Shifts", null, 10L, 0, 0L, "Shifts" },
                    { 10304L, null, "Delete", null, 103L, 0, 0L, "Delete" },
                    { 10303L, null, "Edit", null, 103L, 0, 0L, "Edit" },
                    { 10302L, null, "Add", null, 103L, 0, 0L, "Add" },
                    { 10301L, null, "View", null, 103L, 0, 0L, "View" },
                    { 103L, null, "Users", null, 10L, 0, 0L, "Users" },
                    { 10204L, null, "Delete", null, 102L, 0, 0L, "Delete" },
                    { 10203L, null, "Edit", null, 102L, 0, 0L, "Edit" },
                    { 10202L, null, "Add", null, 102L, 0, 0L, "Add" },
                    { 10201L, null, "View", null, 102L, 0, 0L, "View" },
                    { 102L, null, "Roles", null, 10L, 0, 0L, "Roles" },
                    { 10104L, null, "Delete", null, 101L, 0, 0L, "Delete" },
                    { 10103L, null, "Edit", null, 101L, 0, 0L, "Edit" },
                    { 10102L, null, "Add", null, 101L, 0, 0L, "Add" },
                    { 10101L, null, "View", null, 101L, 0, 0L, "View" },
                    { 10403L, null, "Edit", null, 104L, 0, 0L, "Edit" },
                    { 10404L, null, "Delete", null, 104L, 0, 0L, "Delete" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 103L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 104L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10103L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10104L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10201L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10202L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10203L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10204L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10301L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10302L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10303L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10304L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10401L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10402L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10403L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10404L);
        }
    }
}
