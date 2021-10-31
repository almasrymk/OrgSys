using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class AddLengthElectronicScale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[] { 19L, null, 0L, false, null, "LengthElectronicScale", null, 0L, "Invoice", 0, 1L, null, "4" });

            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[] { 119L, null, 0L, false, null, "LengthElectronicScale", null, 0L, "Invoice", 0, 2L, null, "4" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 119L);
        }
    }
}
