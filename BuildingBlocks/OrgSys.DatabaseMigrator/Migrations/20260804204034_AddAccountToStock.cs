using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrgSys.DatabaseMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountToStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                table: "Stock",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stock_AccountId",
                table: "Stock",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stock_Account_AccountId",
                table: "Stock",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stock_Account_AccountId",
                table: "Stock");

            migrationBuilder.DropIndex(
                name: "IX_Stock_AccountId",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Stock");
        }
    }
}
