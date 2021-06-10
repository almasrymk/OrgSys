using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class EditSeed3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 77L, false, null, "DefaultStore", null, 0L, "Transaction", 0, 3L, null, "1" },
                    { 78L, false, null, "NumberLine", null, 0L, "Transaction", 0, 3L, null, "6" },
                    { 79L, false, null, "OrderTabe", null, 0L, "Transaction", 0, 3L, null, "1" },
                    { 80L, false, null, "AutoSave", null, 0L, "Transaction", 0, 3L, null, "0" },
                    { 81L, false, null, "TypeSerial", null, 0L, "Transaction", 0, 3L, null, "1" },
                    { 82L, false, null, "AllowRepeated", null, 0L, "Transaction", 0, 3L, null, "1" },
                    { 83L, false, null, "SaveLastStatusSetting", null, 0L, "Transaction", 0, 3L, null, "1" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 77L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 78L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 79L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 80L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 81L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 82L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 83L);
        }
    }
}
