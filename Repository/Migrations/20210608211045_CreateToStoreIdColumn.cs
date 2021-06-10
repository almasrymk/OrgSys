using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class CreateToStoreIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ToStoreId",
                table: "Transaction",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_ToStoreId",
                table: "Transaction",
                column: "ToStoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Store_ToStoreId",
                table: "Transaction",
                column: "ToStoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Store_ToStoreId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_ToStoreId",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "ToStoreId",
                table: "Transaction");
        }
    }
}
