using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class ReportTypeOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderType_OrderTypeId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_OrderTypeId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderTypeId",
                table: "Order");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrderTypeId",
                table: "Order",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderTypeId",
                table: "Order",
                column: "OrderTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderType_OrderTypeId",
                table: "Order",
                column: "OrderTypeId",
                principalTable: "OrderType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
