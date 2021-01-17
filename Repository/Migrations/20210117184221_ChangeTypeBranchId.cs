using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class ChangeTypeBranchId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Store_Branch_BranchId1",
                table: "Store");

            migrationBuilder.DropIndex(
                name: "IX_Store_BranchId1",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "BranchId1",
                table: "Store");

            migrationBuilder.AlterColumn<long>(
                name: "BranchId",
                table: "Store",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Store_BranchId",
                table: "Store",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Store_Branch_BranchId",
                table: "Store",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Store_Branch_BranchId",
                table: "Store");

            migrationBuilder.DropIndex(
                name: "IX_Store_BranchId",
                table: "Store");

            migrationBuilder.AlterColumn<string>(
                name: "BranchId",
                table: "Store",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "BranchId1",
                table: "Store",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Store_BranchId1",
                table: "Store",
                column: "BranchId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Store_Branch_BranchId1",
                table: "Store",
                column: "BranchId1",
                principalTable: "Branch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
