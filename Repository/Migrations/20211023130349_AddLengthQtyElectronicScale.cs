using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class AddLengthQtyElectronicScale : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[] { 20L, null, 0L, false, null, "LengthQtyElectronicScale", null, 0L, "Invoice", 0, 1L, null, "5" });

            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[] { 120L, null, 0L, false, null, "LengthQtyElectronicScale", null, 0L, "Invoice", 0, 2L, null, "5" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 120L);
        }
    }
}
