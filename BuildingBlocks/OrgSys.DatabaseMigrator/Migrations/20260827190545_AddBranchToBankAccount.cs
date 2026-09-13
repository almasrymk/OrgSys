using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrgSys.DatabaseMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddBranchToBankAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "BankAccount");

            migrationBuilder.AddColumn<long>(
                name: "BranchId",
                table: "BankAccount",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BankAccount_BranchId",
                table: "BankAccount",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankAccount_Branch_BranchId",
                table: "BankAccount",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankAccount_Branch_BranchId",
                table: "BankAccount");

            migrationBuilder.DropIndex(
                name: "IX_BankAccount_BranchId",
                table: "BankAccount");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "BankAccount");

            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "BankAccount",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }
    }
}
