using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class PreferenceTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 61L, false, null, "DefaultStore", null, 0L, "Transaction", 0, 1L, null, "1" },
                    { 62L, false, null, "DefaultSupplier", null, 0L, "Transaction", 0, 1L, null, "1" },
                    { 63L, false, null, "NumberLine", null, 0L, "Transaction", 0, 1L, null, "6" },
                    { 64L, false, null, "OrderTabe", null, 0L, "Transaction", 0, 1L, null, "1" },
                    { 65L, false, null, "AutoSave", null, 0L, "Transaction", 0, 1L, null, "0" },
                    { 66L, false, null, "TypeSerial", null, 0L, "Transaction", 0, 1L, null, "1" },
                    { 67L, false, null, "AllowRepeated", null, 0L, "Transaction", 0, 1L, null, "1" },
                    { 68L, false, null, "SaveLastStatusSetting", null, 0L, "Transaction", 0, 1L, null, "1" },
                    { 69L, false, null, "DefaultStore", null, 0L, "Transaction", 0, 2L, null, "1" },
                    { 70L, false, null, "DefaultSupplier", null, 0L, "Transaction", 0, 2L, null, "1" },
                    { 71L, false, null, "NumberLine", null, 0L, "Transaction", 0, 2L, null, "6" },
                    { 72L, false, null, "OrderTabe", null, 0L, "Transaction", 0, 2L, null, "1" },
                    { 73L, false, null, "AutoSave", null, 0L, "Transaction", 0, 2L, null, "0" },
                    { 74L, false, null, "TypeSerial", null, 0L, "Transaction", 0, 2L, null, "1" },
                    { 75L, false, null, "AllowRepeated", null, 0L, "Transaction", 0, 2L, null, "1" },
                    { 76L, false, null, "SaveLastStatusSetting", null, 0L, "Transaction", 0, 2L, null, "1" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 61L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 62L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 63L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 64L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 65L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 66L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 67L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 68L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 69L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 70L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 71L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 72L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 73L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 74L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 75L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 76L);
        }
    }
}
