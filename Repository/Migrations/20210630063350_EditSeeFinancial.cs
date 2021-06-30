using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class EditSeeFinancial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 1000L, false, null, "DefaultClient", null, 0L, "Financial", 0, 1L, null, "1" },
                    { 1203L, false, null, "DefaultCurrency", null, 0L, "Financial", 0, 3L, null, "1" },
                    { 1202L, false, null, "DefaultPaymentType", null, 0L, "Financial", 0, 3L, null, "1" },
                    { 1201L, false, null, "DefaultSafe", null, 0L, "Financial", 0, 3L, null, "1" },
                    { 1200L, false, null, "DefaultOutlay", null, 0L, "Financial", 0, 3L, null, "1" },
                    { 1105L, false, null, "TypeSerial", null, 0L, "Financial", 0, 2L, null, "1" },
                    { 1104L, false, null, "AutoSave", null, 0L, "Financial", 0, 2L, null, "0" },
                    { 1103L, false, null, "DefaultCurrency", null, 0L, "Financial", 0, 2L, null, "1" },
                    { 1102L, false, null, "DefaultPaymentType", null, 0L, "Financial", 0, 2L, null, "1" },
                    { 1101L, false, null, "DefaultSafe", null, 0L, "Financial", 0, 2L, null, "1" },
                    { 1100L, false, null, "DefaultSupplier", null, 0L, "Financial", 0, 2L, null, "1" },
                    { 1005L, false, null, "TypeSerial", null, 0L, "Financial", 0, 1L, null, "1" },
                    { 1004L, false, null, "AutoSave", null, 0L, "Financial", 0, 1L, null, "0" },
                    { 1003L, false, null, "DefaultCurrency", null, 0L, "Financial", 0, 1L, null, "1" },
                    { 1002L, false, null, "DefaultPaymentType", null, 0L, "Financial", 0, 1L, null, "1" },
                    { 1001L, false, null, "DefaultSafe", null, 0L, "Financial", 0, 1L, null, "1" },
                    { 1204L, false, null, "AutoSave", null, 0L, "Financial", 0, 3L, null, "0" },
                    { 1205L, false, null, "TypeSerial", null, 0L, "Financial", 0, 3L, null, "1" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1000L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1001L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1002L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1003L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1004L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1005L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1100L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1101L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1102L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1103L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1104L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1105L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1200L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1201L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1202L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1203L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1204L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 1205L);
        }
    }
}
