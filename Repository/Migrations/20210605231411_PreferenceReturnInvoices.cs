using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class PreferenceReturnInvoices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 31L, false, null, "DefaultStore", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 58L, false, null, "TypeSerial", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 57L, false, null, "AutoSave", null, 0L, "Invoice", 0, 4L, null, "0" },
                    { 56L, false, null, "OrderTabe", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 55L, false, null, "NumberLine", null, 0L, "Invoice", 0, 4L, null, "6" },
                    { 54L, false, null, "DefaultTaxType", null, 0L, "Invoice", 0, 4L, null, "2" },
                    { 53L, false, null, "TaxValue", null, 0L, "Invoice", 0, 4L, null, "14" },
                    { 52L, false, null, "DefaultServiceType", null, 0L, "Invoice", 0, 4L, null, "2" },
                    { 51L, false, null, "ServiceValue", null, 0L, "Invoice", 0, 4L, null, "" },
                    { 50L, false, null, "DefaultDiscountType", null, 0L, "Invoice", 0, 4L, null, "2" },
                    { 49L, false, null, "DiscountValue", null, 0L, "Invoice", 0, 4L, null, "" },
                    { 48L, false, null, "DefaultPaymentType", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 47L, false, null, "DefaultSupplier", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 46L, false, null, "DefaultStore", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 45L, false, null, "SaveLastStatusSetting", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 44L, false, null, "AllowRepeated", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 43L, false, null, "TypeSerial", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 42L, false, null, "AutoSave", null, 0L, "Invoice", 0, 3L, null, "0" },
                    { 41L, false, null, "OrderTabe", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 40L, false, null, "NumberLine", null, 0L, "Invoice", 0, 3L, null, "6" },
                    { 39L, false, null, "DefaultTaxType", null, 0L, "Invoice", 0, 3L, null, "2" },
                    { 38L, false, null, "TaxValue", null, 0L, "Invoice", 0, 3L, null, "14" },
                    { 37L, false, null, "DefaultServiceType", null, 0L, "Invoice", 0, 3L, null, "2" },
                    { 36L, false, null, "ServiceValue", null, 0L, "Invoice", 0, 3L, null, "" },
                    { 35L, false, null, "DefaultDiscountType", null, 0L, "Invoice", 0, 3L, null, "2" },
                    { 34L, false, null, "DiscountValue", null, 0L, "Invoice", 0, 3L, null, "" },
                    { 33L, false, null, "DefaultPaymentType", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 32L, false, null, "DefaultCustomer", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 59L, false, null, "AllowRepeated", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 60L, false, null, "SaveLastStatusSetting", null, 0L, "Invoice", 0, 4L, null, "1" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 31L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 32L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 33L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 34L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 35L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 36L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 37L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 38L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 39L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 40L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 41L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 42L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 43L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 44L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 45L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 46L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 47L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 48L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 49L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 50L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 51L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 52L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 53L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 54L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 55L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 56L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 57L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 58L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 59L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 60L);
        }
    }
}
