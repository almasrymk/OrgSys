using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class EditSeed8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[] { 813L, false, null, "DefaultCustomer", null, 0L, "Order", 0, 1L, null, "1" });

            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[] { 913L, false, null, "DefaultCustomer", null, 0L, "Order", 0, 2L, null, "1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 813L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 913L);
        }
    }
}
