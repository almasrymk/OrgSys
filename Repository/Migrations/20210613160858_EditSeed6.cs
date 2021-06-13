using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class EditSeed6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 96L,
                columns: new[] { "Key", "TypeId", "Value" },
                values: new object[] { "DiscountValue", 1L, "" });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 97L,
                columns: new[] { "Key", "TypeId" },
                values: new object[] { "DefaultDiscountType", 1L });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 98L,
                columns: new[] { "Key", "TypeId", "Value" },
                values: new object[] { "ServiceValue", 1L, "" });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 99L,
                columns: new[] { "Key", "TypeId", "Value" },
                values: new object[] { "DefaultServiceType", 1L, "2" });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 100L,
                columns: new[] { "Key", "TypeId", "Value" },
                values: new object[] { "TaxValue", 1L, "14" });

            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 112L, false, null, "DefaultTaxType", null, 0L, "Order", 0, 2L, null, "2" },
                    { 111L, false, null, "TaxValue", null, 0L, "Order", 0, 2L, null, "14" },
                    { 110L, false, null, "DefaultServiceType", null, 0L, "Order", 0, 2L, null, "2" },
                    { 109L, false, null, "ServiceValue", null, 0L, "Order", 0, 2L, null, "" },
                    { 107L, false, null, "DiscountValue", null, 0L, "Order", 0, 2L, null, "" },
                    { 106L, false, null, "AllowRepeated", null, 0L, "Order", 0, 2L, null, "1" },
                    { 105L, false, null, "TypeSerial", null, 0L, "Order", 0, 2L, null, "1" },
                    { 104L, false, null, "AutoSave", null, 0L, "Order", 0, 2L, null, "0" },
                    { 103L, false, null, "OrderTabe", null, 0L, "Order", 0, 2L, null, "2" },
                    { 102L, false, null, "NumberLine", null, 0L, "Order", 0, 2L, null, "6" },
                    { 108L, false, null, "DefaultDiscountType", null, 0L, "Order", 0, 2L, null, "2" },
                    { 101L, false, null, "DefaultTaxType", null, 0L, "Order", 0, 1L, null, "2" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 101L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 102L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 103L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 104L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 105L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 106L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 107L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 108L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 109L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 110L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 111L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 112L);

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 96L,
                columns: new[] { "Key", "TypeId", "Value" },
                values: new object[] { "NumberLine", 2L, "6" });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 97L,
                columns: new[] { "Key", "TypeId" },
                values: new object[] { "OrderTabe", 2L });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 98L,
                columns: new[] { "Key", "TypeId", "Value" },
                values: new object[] { "AutoSave", 2L, "0" });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 99L,
                columns: new[] { "Key", "TypeId", "Value" },
                values: new object[] { "TypeSerial", 2L, "1" });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 100L,
                columns: new[] { "Key", "TypeId", "Value" },
                values: new object[] { "AllowRepeated", 2L, "1" });
        }
    }
}
