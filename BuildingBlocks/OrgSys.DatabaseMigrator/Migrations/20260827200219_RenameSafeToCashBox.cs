using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrgSys.DatabaseMigrator.Migrations
{
    /// <inheritdoc />
    public partial class RenameSafeToCashBox : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Financial_Safe_SafeId",
                table: "Financial");

            // Rename rather than drop+recreate — the auto-scaffolded diff treated this as a brand new
            // table (the CLR type changed name too, not just the table), which would silently drop every
            // existing Cash Box row. Table/column data survives a rename; only constraint/index names
            // need the explicit sp_rename calls below so they match what a fresh CreateTable would produce.
            migrationBuilder.RenameTable(
                name: "Safe",
                newName: "CashBox");

            migrationBuilder.RenameColumn(
                name: "SafeId",
                table: "Financial",
                newName: "CashBoxId");

            migrationBuilder.RenameIndex(
                name: "IX_Financial_SafeId",
                table: "Financial",
                newName: "IX_Financial_CashBoxId");

            migrationBuilder.RenameIndex(
                name: "IX_Safe_AccountId",
                table: "CashBox",
                newName: "IX_CashBox_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Safe_BranchId",
                table: "CashBox",
                newName: "IX_CashBox_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_Safe_FinancialAccountId",
                table: "CashBox",
                newName: "IX_CashBox_FinancialAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Safe_KeeperUserId",
                table: "CashBox",
                newName: "IX_CashBox_KeeperUserId");

            migrationBuilder.Sql("EXEC sp_rename 'PK_Safe', 'PK_CashBox';");
            migrationBuilder.Sql("EXEC sp_rename 'FK_Safe_Account_AccountId', 'FK_CashBox_Account_AccountId';");
            migrationBuilder.Sql("EXEC sp_rename 'FK_Safe_Branch_BranchId', 'FK_CashBox_Branch_BranchId';");
            migrationBuilder.Sql("EXEC sp_rename 'FK_Safe_FinancialAccount_FinancialAccountId', 'FK_CashBox_FinancialAccount_FinancialAccountId';");
            migrationBuilder.Sql("EXEC sp_rename 'FK_Safe_User_KeeperUserId', 'FK_CashBox_User_KeeperUserId';");

            migrationBuilder.AddForeignKey(
                name: "FK_Financial_CashBox_CashBoxId",
                table: "Financial",
                column: "CashBoxId",
                principalTable: "CashBox",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Financial_CashBox_CashBoxId",
                table: "Financial");

            migrationBuilder.Sql("EXEC sp_rename 'PK_CashBox', 'PK_Safe';");
            migrationBuilder.Sql("EXEC sp_rename 'FK_CashBox_Account_AccountId', 'FK_Safe_Account_AccountId';");
            migrationBuilder.Sql("EXEC sp_rename 'FK_CashBox_Branch_BranchId', 'FK_Safe_Branch_BranchId';");
            migrationBuilder.Sql("EXEC sp_rename 'FK_CashBox_FinancialAccount_FinancialAccountId', 'FK_Safe_FinancialAccount_FinancialAccountId';");
            migrationBuilder.Sql("EXEC sp_rename 'FK_CashBox_User_KeeperUserId', 'FK_Safe_User_KeeperUserId';");

            migrationBuilder.RenameIndex(
                name: "IX_CashBox_AccountId",
                table: "CashBox",
                newName: "IX_Safe_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_CashBox_BranchId",
                table: "CashBox",
                newName: "IX_Safe_BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_CashBox_FinancialAccountId",
                table: "CashBox",
                newName: "IX_Safe_FinancialAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_CashBox_KeeperUserId",
                table: "CashBox",
                newName: "IX_Safe_KeeperUserId");

            migrationBuilder.RenameColumn(
                name: "CashBoxId",
                table: "Financial",
                newName: "SafeId");

            migrationBuilder.RenameIndex(
                name: "IX_Financial_CashBoxId",
                table: "Financial",
                newName: "IX_Financial_SafeId");

            migrationBuilder.RenameTable(
                name: "CashBox",
                newName: "Safe");

            migrationBuilder.AddForeignKey(
                name: "FK_Financial_Safe_SafeId",
                table: "Financial",
                column: "SafeId",
                principalTable: "Safe",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
