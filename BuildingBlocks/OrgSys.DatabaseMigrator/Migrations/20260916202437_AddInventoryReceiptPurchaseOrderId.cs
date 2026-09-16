using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrgSys.DatabaseMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryReceiptPurchaseOrderId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PurchaseOrderId",
                table: "InventoryReceipt",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryReceipt_PurchaseOrderId",
                table: "InventoryReceipt",
                column: "PurchaseOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryReceipt_PurchaseOrder_PurchaseOrderId",
                table: "InventoryReceipt",
                column: "PurchaseOrderId",
                principalTable: "PurchaseOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryReceipt_PurchaseOrder_PurchaseOrderId",
                table: "InventoryReceipt");

            migrationBuilder.DropIndex(
                name: "IX_InventoryReceipt_PurchaseOrderId",
                table: "InventoryReceipt");

            migrationBuilder.DropColumn(
                name: "PurchaseOrderId",
                table: "InventoryReceipt");
        }
    }
}
