using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameAccountBankToBankAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Pure rename — preserves existing BankAccount data. EF's scaffolder defaults to
            // DropTable+CreateTable for renames; that would delete every existing bank account,
            // so this migration is hand-written to use RenameTable/sp_rename instead.
            migrationBuilder.RenameTable(
                name: "AccountBank",
                newName: "BankAccount");

            migrationBuilder.Sql("EXEC sp_rename 'PK_AccountBank', 'PK_BankAccount', 'OBJECT';");
            migrationBuilder.Sql("EXEC sp_rename 'FK_AccountBank_Account_AccountId', 'FK_BankAccount_Account_AccountId', 'OBJECT';");
            migrationBuilder.Sql("EXEC sp_rename 'FK_AccountBank_BankBranch_BankBranchd', 'FK_BankAccount_BankBranch_BankBranchd', 'OBJECT';");
            migrationBuilder.Sql("EXEC sp_rename 'FK_AccountBank_Bank_BankId', 'FK_BankAccount_Bank_BankId', 'OBJECT';");
            migrationBuilder.Sql("EXEC sp_rename 'FK_AccountBank_FinancialAccount_FinancialAccountId', 'FK_BankAccount_FinancialAccount_FinancialAccountId', 'OBJECT';");

            migrationBuilder.RenameIndex(
                name: "IX_AccountBank_AccountId", table: "BankAccount", newName: "IX_BankAccount_AccountId");
            migrationBuilder.RenameIndex(
                name: "IX_AccountBank_BankBranchd", table: "BankAccount", newName: "IX_BankAccount_BankBranchd");
            migrationBuilder.RenameIndex(
                name: "IX_AccountBank_BankId", table: "BankAccount", newName: "IX_BankAccount_BankId");
            migrationBuilder.RenameIndex(
                name: "IX_AccountBank_FinancialAccountId", table: "BankAccount", newName: "IX_BankAccount_FinancialAccountId");

            migrationBuilder.AddColumn<int>(
                name: "FinancialTransactionType",
                table: "Financial",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceNumber",
                table: "Financial",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            // Backfill FinancialTransactionType from the existing FinancialTypeId/Direction combo
            // (seeded FinancialType ids: 2 = Receipt, 3 = Payment, 4 = Transfer;
            //  FinancialTransactionDirection: 1 = In, 2 = Out).
            migrationBuilder.Sql(@"
UPDATE Financial SET FinancialTransactionType = 1 WHERE FinancialTypeId = 2; -- Receipt
UPDATE Financial SET FinancialTransactionType = 2 WHERE FinancialTypeId = 3; -- Payment
UPDATE Financial SET FinancialTransactionType = 3 WHERE FinancialTypeId = 4 AND Direction = 2; -- TransferOut
UPDATE Financial SET FinancialTransactionType = 4 WHERE FinancialTypeId = 4 AND Direction = 1; -- TransferIn
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinancialTransactionType",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "ReferenceNumber",
                table: "Financial");

            migrationBuilder.RenameIndex(
                name: "IX_BankAccount_AccountId", table: "BankAccount", newName: "IX_AccountBank_AccountId");
            migrationBuilder.RenameIndex(
                name: "IX_BankAccount_BankBranchd", table: "BankAccount", newName: "IX_AccountBank_BankBranchd");
            migrationBuilder.RenameIndex(
                name: "IX_BankAccount_BankId", table: "BankAccount", newName: "IX_AccountBank_BankId");
            migrationBuilder.RenameIndex(
                name: "IX_BankAccount_FinancialAccountId", table: "BankAccount", newName: "IX_AccountBank_FinancialAccountId");

            migrationBuilder.Sql("EXEC sp_rename 'FK_BankAccount_Account_AccountId', 'FK_AccountBank_Account_AccountId', 'OBJECT';");
            migrationBuilder.Sql("EXEC sp_rename 'FK_BankAccount_BankBranch_BankBranchd', 'FK_AccountBank_BankBranch_BankBranchd', 'OBJECT';");
            migrationBuilder.Sql("EXEC sp_rename 'FK_BankAccount_Bank_BankId', 'FK_AccountBank_Bank_BankId', 'OBJECT';");
            migrationBuilder.Sql("EXEC sp_rename 'FK_BankAccount_FinancialAccount_FinancialAccountId', 'FK_AccountBank_FinancialAccount_FinancialAccountId', 'OBJECT';");
            migrationBuilder.Sql("EXEC sp_rename 'PK_BankAccount', 'PK_AccountBank', 'OBJECT';");

            migrationBuilder.RenameTable(
                name: "BankAccount",
                newName: "AccountBank");
        }
    }
}
