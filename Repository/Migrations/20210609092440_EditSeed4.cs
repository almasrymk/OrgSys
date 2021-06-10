using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class EditSeed4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 64L,
                column: "Value",
                value: "2");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 72L,
                column: "Value",
                value: "2");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 79L,
                column: "Value",
                value: "2");

            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 84L, false, null, "AutoReceived", null, 0L, "Transaction", 0, 3L, null, "0" },
                    { 85L, false, null, "DefaultStore", null, 0L, "Transaction", 0, 4L, null, "1" },
                    { 86L, false, null, "NumberLine", null, 0L, "Transaction", 0, 4L, null, "6" },
                    { 87L, false, null, "OrderTabe", null, 0L, "Transaction", 0, 4L, null, "2" },
                    { 88L, false, null, "AutoSave", null, 0L, "Transaction", 0, 4L, null, "0" },
                    { 89L, false, null, "TypeSerial", null, 0L, "Transaction", 0, 4L, null, "1" },
                    { 90L, false, null, "AllowRepeated", null, 0L, "Transaction", 0, 4L, null, "1" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 84L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 85L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 86L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 87L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 88L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 89L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 90L);

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 64L,
                column: "Value",
                value: "1");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 72L,
                column: "Value",
                value: "1");

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 79L,
                column: "Value",
                value: "1");
        }
    }
}
