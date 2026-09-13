using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrgSys.DatabaseMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InventoryId",
                table: "Transaction",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_InventoryId",
                table: "Transaction",
                column: "InventoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Inventory_InventoryId",
                table: "Transaction",
                column: "InventoryId",
                principalTable: "Inventory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Inventory_InventoryId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_InventoryId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "InventoryId",
                table: "Transaction");
        }
    }
}
