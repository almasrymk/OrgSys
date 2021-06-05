using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class PreferencePurchases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 16L, false, null, "DefaultStore", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 17L, false, null, "DefaultSupplier", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 18L, false, null, "DefaultPaymentType", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 19L, false, null, "DiscountValue", null, 0L, "Invoice", 0, 2L, null, "" },
                    { 20L, false, null, "DefaultDiscountType", null, 0L, "Invoice", 0, 2L, null, "2" },
                    { 21L, false, null, "ServiceValue", null, 0L, "Invoice", 0, 2L, null, "" },
                    { 22L, false, null, "DefaultServiceType", null, 0L, "Invoice", 0, 2L, null, "2" },
                    { 23L, false, null, "TaxValue", null, 0L, "Invoice", 0, 2L, null, "14" },
                    { 24L, false, null, "DefaultTaxType", null, 0L, "Invoice", 0, 2L, null, "2" },
                    { 25L, false, null, "NumberLine", null, 0L, "Invoice", 0, 2L, null, "6" },
                    { 26L, false, null, "OrderTabe", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 27L, false, null, "AutoSave", null, 0L, "Invoice", 0, 2L, null, "0" },
                    { 28L, false, null, "TypeSerial", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 29L, false, null, "AllowRepeated", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 30L, false, null, "SaveLastStatusSetting", null, 0L, "Invoice", 0, 2L, null, "1" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 24L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 26L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 27L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 28L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 29L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 30L);
        }
    }
}
