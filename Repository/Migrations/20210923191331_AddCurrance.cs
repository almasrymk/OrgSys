using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class AddCurrance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.InsertData(
            //    table: "Currency",
            //    columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "IsDefault", "MaskText", "Name", "ParentId", "Rate", "Status", "TypeId" },
            //    values: new object[] { 1L, null, 0L, false, null, false, null, "Epg", 0L, 0m, 0, 0L });

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 216L,
                column: "TypeId",
                value: 3L);

            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 17L, null, 0L, false, null, "DefaultCurrency", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 117L, null, 0L, false, null, "DefaultCurrency", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 217L, null, 0L, false, null, "DefaultCurrency", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 317L, null, 0L, false, null, "DefaultCurrency", null, 0L, "Invoice", 0, 4L, null, "1" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currency",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 117L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 217L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 317L);

            migrationBuilder.UpdateData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 216L,
                column: "TypeId",
                value: 4L);
        }
    }
}
