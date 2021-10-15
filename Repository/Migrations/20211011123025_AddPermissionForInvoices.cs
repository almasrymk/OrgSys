using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class AddPermissionForInvoices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3010105L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "SalesInvoices.Cancel", "Cancel" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3010205L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "SalesReturns.Cancel", "Cancel" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3020105L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "PurchasesInvoices.Cancel", "Cancel" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3020205L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "PurchasesReturns.Cancel", "Cancel" });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 3010106L, null, 0L, false, null, "SalesInvoices.Preference", null, "Preference", 30101L, 0, 1L },
                    { 3010206L, null, 0L, false, null, "SalesReturns.Preference", null, "Preference", 30102L, 0, 1L },
                    { 3020106L, null, 0L, false, null, "PurchasesInvoices.Preference", null, "Preference", 30201L, 0, 1L },
                    { 3020206L, null, 0L, false, null, "PurchasesReturns.Preference", null, "Preference", 30202L, 0, 1L }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3010106L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3010206L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3020106L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3020206L);

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3010105L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "SalesInvoices.Preference", "Preference" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3010205L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "SalesReturns.Preference", "Preference" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3020105L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "PurchasesInvoices.Preference", "Preference" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 3020205L,
                columns: new[] { "Key", "Name" },
                values: new object[] { "PurchasesReturns.Preference", "Preference" });
        }
    }
}
