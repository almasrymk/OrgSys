using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class PreferenceTransactionType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TransactionType",
                columns: new[] { "Id", "Hide", "ImgPath", "InOut", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[] { 1L, false, null, -1, null, "Transaction In", 0L, 0, 0L });

            migrationBuilder.InsertData(
                table: "TransactionType",
                columns: new[] { "Id", "Hide", "ImgPath", "InOut", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[] { 2L, false, null, 1, null, "Transaction Out", 0L, 0, 0L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 2L);
        }
    }
}
