using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class SaveLastStatusSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[] { 15L, false, null, "SaveLastStatusSetting", null, 0L, "Invoice", 0, 1L, null, "1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 15L);
        }
    }
}
