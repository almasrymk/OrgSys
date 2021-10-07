using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class AddCancelForOrgder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2010105L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "Internal.Cancel", "Cancel" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2010205L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "External.Cancel", "Cancel" });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 2010106L, null, 0L, false, null, "Internal.Preference", null, "Preference", 20101L, 0, 1L },
                    { 2010206L, null, 0L, false, null, "External.Preference", null, "Preference", 20102L, 0, 1L }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2010106L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2010206L);

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2010105L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "Internal.Preference", "Preference" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 2010205L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "External.Preference", "Preference" });
        }
    }
}
