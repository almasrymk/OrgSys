using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class DealerIdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Dealer_DealerId",
                table: "Transaction");

            migrationBuilder.AlterColumn<long>(
                name: "DealerId",
                table: "Transaction",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.InsertData(
                table: "TransactionType",
                columns: new[] { "Id", "Hide", "ImgPath", "InOut", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[] { 3L, false, null, -1, null, "Transafer", 0L, 0, 0L });

            migrationBuilder.InsertData(
                table: "TransactionType",
                columns: new[] { "Id", "Hide", "ImgPath", "InOut", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[] { 4L, false, null, 1, null, "Received", 0L, 0, 0L });

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Dealer_DealerId",
                table: "Transaction",
                column: "DealerId",
                principalTable: "Dealer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Dealer_DealerId",
                table: "Transaction");

            migrationBuilder.DeleteData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "TransactionType",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.AlterColumn<long>(
                name: "DealerId",
                table: "Transaction",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Dealer_DealerId",
                table: "Transaction",
                column: "DealerId",
                principalTable: "Dealer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
