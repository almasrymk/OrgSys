using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class CreatePreferenceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InOut",
                table: "InvoiceType",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Preference",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preference", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Branch",
                columns: new[] { "Id", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[] { 1L, false, null, null, "Main Branch", 0L, 0, 0L });

            migrationBuilder.InsertData(
                table: "Dealer",
                columns: new[] { "Id", "Address", "Code", "CodeNumber", "Email", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Phone", "Status", "TypeId" },
                values: new object[] { 1L, null, "1", 1L, null, false, null, null, "Unspecified", 0L, null, 0, 1L });

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Hide", "InOut" },
                values: new object[] { false, -1 });

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Hide", "InOut" },
                values: new object[] { false, 1 });

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Hide", "InOut" },
                values: new object[] { false, 1 });

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "Hide", "InOut" },
                values: new object[] { false, -1 });

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Hide",
                value: false);

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Hide",
                value: false);

            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 12L, false, null, "AutoSave", null, 0L, "Invoice", 0, 1L, null, "0" },
                    { 11L, false, null, "OrderTabe", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 10L, false, null, "NumberLine", null, 0L, "Invoice", 0, 1L, null, "6" },
                    { 9L, false, null, "DefaultTaxType", null, 0L, "Invoice", 0, 1L, null, "2" },
                    { 8L, false, null, "TaxValue", null, 0L, "Invoice", 0, 1L, null, "14" },
                    { 7L, false, null, "DefaultServiceType", null, 0L, "Invoice", 0, 1L, null, "2" },
                    { 6L, false, null, "ServiceValue", null, 0L, "Invoice", 0, 1L, null, "" },
                    { 5L, false, null, "DefaultDiscountType", null, 0L, "Invoice", 0, 1L, null, "2" },
                    { 4L, false, null, "DiscountValue", null, 0L, "Invoice", 0, 1L, null, "" },
                    { 3L, false, null, "DefaultPaymentType", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 2L, false, null, "DefaultCustomer", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 1L, false, null, "DefaultStore", null, 0L, "Invoice", 0, 1L, null, "1" }
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Hide",
                value: false);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Hide",
                value: false);

            migrationBuilder.InsertData(
                table: "Store",
                columns: new[] { "Id", "BranchId", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[] { 1L, 1L, false, null, null, "Main Store", 0L, 0, 0L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Preference");

            migrationBuilder.DeleteData(
                table: "Dealer",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Store",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Branch",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DropColumn(
                name: "InOut",
                table: "InvoiceType");

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Hide",
                value: true);

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Hide",
                value: true);

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 3L,
                column: "Hide",
                value: true);

            migrationBuilder.UpdateData(
                table: "InvoiceType",
                keyColumn: "Id",
                keyValue: 4L,
                column: "Hide",
                value: true);

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Hide",
                value: true);

            migrationBuilder.UpdateData(
                table: "PaymentType",
                keyColumn: "Id",
                keyValue: 2L,
                column: "Hide",
                value: true);

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Hide",
                value: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Hide",
                value: true);
        }
    }
}
