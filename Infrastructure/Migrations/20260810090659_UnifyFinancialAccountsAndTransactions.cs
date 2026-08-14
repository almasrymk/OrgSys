using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UnifyFinancialAccountsAndTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BranchId",
                table: "Safe",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FinancialAccountId",
                table: "Safe",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "KeeperUserId",
                table: "Safe",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ContraFinancialAccountId",
                table: "Financial",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Direction",
                table: "Financial",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FinancialAccountId",
                table: "Financial",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FinancialTransactionTypeId",
                table: "Financial",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FinancialTransferId",
                table: "Financial",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "JournalId",
                table: "Financial",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReferenceId",
                table: "Financial",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReferenceType",
                table: "Financial",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "AccountNumber",
                table: "AccountBank",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "AccountBank",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FinancialAccountId",
                table: "AccountBank",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IBAN",
                table: "AccountBank",
                type: "nvarchar(34)",
                maxLength: 34,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SwiftCode",
                table: "AccountBank",
                type: "nvarchar(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FinancialAccount",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FinancialAccountType = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CodeNumber = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialAccount_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinancialAccount_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FinancialTransactionType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CodeNumber = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialTransactionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialTransfer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromFinancialAccountId = table.Column<long>(type: "bigint", nullable: false),
                    ToFinancialAccountId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: false),
                    ExchangeRate = table.Column<decimal>(type: "decimal(18,6)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CodeNumber = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifyUserId = table.Column<long>(type: "bigint", nullable: true),
                    ModifyDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ShiftId = table.Column<long>(type: "bigint", nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    HasJournal = table.Column<bool>(type: "bit", nullable: false),
                    Review = table.Column<bool>(type: "bit", nullable: false),
                    Posted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialTransfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialTransfer_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinancialTransfer_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialTransfer_FinancialAccount_FromFinancialAccountId",
                        column: x => x.FromFinancialAccountId,
                        principalTable: "FinancialAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinancialTransfer_FinancialAccount_ToFinancialAccountId",
                        column: x => x.ToFinancialAccountId,
                        principalTable: "FinancialAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinancialTransfer_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shift",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FinancialTransfer_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialTransfer_User_ModifyUserId",
                        column: x => x.ModifyUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.Sql("""
                INSERT INTO [FinancialTransactionType]
                    ([Id], [Name], [CodeNumber], [ParentId], [TypeId], [Hide], [Status])
                VALUES
                    (1, N'OpeningBalance', 1, 0, 0, 0, 0),
                    (2, N'Receipt', 2, 0, 0, 0, 0),
                    (3, N'Payment', 3, 0, 0, 0, 0),
                    (4, N'Transfer', 4, 0, 0, 0, 0),
                    (5, N'Deposit', 5, 0, 0, 0, 0),
                    (6, N'Withdrawal', 6, 0, 0, 0, 0),
                    (7, N'Fee', 7, 0, 0, 0, 0),
                    (8, N'Interest', 8, 0, 0, 0, 0),
                    (9, N'Cheque', 9, 0, 0, 0, 0),
                    (10, N'Adjustment', 10, 0, 0, 0, 0);

                DECLARE @DefaultCurrencyId bigint =
                    COALESCE((SELECT TOP (1) [Id] FROM [Currency] WHERE [IsDefault] = 1 ORDER BY [Id]),
                             (SELECT TOP (1) [Id] FROM [Currency] ORDER BY [Id]));
                DECLARE @SafeAccounts TABLE ([SafeId] bigint NOT NULL, [FinancialAccountId] bigint NOT NULL);

                MERGE [FinancialAccount] AS target
                USING [Safe] AS source ON 1 = 0
                WHEN NOT MATCHED THEN INSERT
                    ([Name], [FinancialAccountType], [AccountId], [CurrencyId], [IsActive],
                     [CodeNumber], [Code], [MaskText], [ParentId], [TypeId], [Hide], [ImgPath], [Status])
                VALUES
                    (COALESCE(source.[Name], N'Cash Box'), 1, source.[AccountId], @DefaultCurrencyId,
                     CASE WHEN source.[Hide] = 1 OR source.[Status] IN (5, 35) THEN 0 ELSE 1 END,
                     source.[CodeNumber], source.[Code], source.[MaskText], source.[ParentId], source.[TypeId],
                     source.[Hide], source.[ImgPath], source.[Status])
                OUTPUT source.[Id], inserted.[Id] INTO @SafeAccounts;

                UPDATE s SET s.[FinancialAccountId] = m.[FinancialAccountId]
                FROM [Safe] s INNER JOIN @SafeAccounts m ON m.[SafeId] = s.[Id];

                DECLARE @BankAccounts TABLE ([BankAccountId] bigint NOT NULL, [FinancialAccountId] bigint NOT NULL);
                MERGE [FinancialAccount] AS target
                USING [AccountBank] AS source ON 1 = 0
                WHEN NOT MATCHED THEN INSERT
                    ([Name], [FinancialAccountType], [AccountId], [CurrencyId], [IsActive],
                     [CodeNumber], [Code], [MaskText], [ParentId], [TypeId], [Hide], [ImgPath], [Status])
                VALUES
                    (COALESCE(source.[Name], N'Bank Account'), 2, source.[AccountId], @DefaultCurrencyId,
                     CASE WHEN source.[Hide] = 1 OR source.[Status] IN (5, 35) THEN 0 ELSE 1 END,
                     source.[CodeNumber], source.[Code], source.[MaskText], source.[ParentId], source.[TypeId],
                     source.[Hide], source.[ImgPath], source.[Status])
                OUTPUT source.[Id], inserted.[Id] INTO @BankAccounts;

                UPDATE b SET b.[FinancialAccountId] = m.[FinancialAccountId]
                FROM [AccountBank] b INNER JOIN @BankAccounts m ON m.[BankAccountId] = b.[Id];

                UPDATE f SET
                    f.[FinancialAccountId] = s.[FinancialAccountId],
                    f.[FinancialTransactionTypeId] = CASE WHEN f.[TypeId] = 1 THEN 1 ELSE 2 END,
                    f.[Direction] = CASE WHEN f.[TypeId] = 1 THEN 1 ELSE 2 END,
                    f.[ReferenceType] = CASE
                        WHEN EXISTS (SELECT 1 FROM [FinancialInvoice] fi WHERE fi.[FinancialId] = f.[Id]) THEN 6
                        WHEN f.[OutlayId] IS NOT NULL THEN 4
                        WHEN f.[DealerId] IS NOT NULL AND f.[TypeId] = 1 THEN 1
                        WHEN f.[DealerId] IS NOT NULL THEN 2
                        ELSE 0 END,
                    f.[ReferenceId] = CASE
                        WHEN EXISTS (SELECT 1 FROM [FinancialInvoice] fi WHERE fi.[FinancialId] = f.[Id])
                            THEN (SELECT TOP (1) fi.[InvoiceId] FROM [FinancialInvoice] fi WHERE fi.[FinancialId] = f.[Id] ORDER BY fi.[RowNumber])
                        WHEN f.[OutlayId] IS NOT NULL THEN f.[OutlayId]
                        ELSE f.[DealerId] END
                FROM [Financial] f INNER JOIN [Safe] s ON s.[Id] = f.[SafeId];
                """);

            migrationBuilder.CreateIndex(
                name: "IX_Safe_BranchId",
                table: "Safe",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Safe_FinancialAccountId",
                table: "Safe",
                column: "FinancialAccountId",
                unique: true,
                filter: "[FinancialAccountId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Safe_KeeperUserId",
                table: "Safe",
                column: "KeeperUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Financial_ContraFinancialAccountId",
                table: "Financial",
                column: "ContraFinancialAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Financial_FinancialAccountId",
                table: "Financial",
                column: "FinancialAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Financial_FinancialTransactionTypeId",
                table: "Financial",
                column: "FinancialTransactionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Financial_FinancialTransferId",
                table: "Financial",
                column: "FinancialTransferId");

            migrationBuilder.CreateIndex(
                name: "IX_Financial_JournalId",
                table: "Financial",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountBank_FinancialAccountId",
                table: "AccountBank",
                column: "FinancialAccountId",
                unique: true,
                filter: "[FinancialAccountId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccount_AccountId",
                table: "FinancialAccount",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccount_CurrencyId",
                table: "FinancialAccount",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransfer_BranchId",
                table: "FinancialTransfer",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransfer_CreateUserId",
                table: "FinancialTransfer",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransfer_CurrencyId",
                table: "FinancialTransfer",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransfer_FromFinancialAccountId",
                table: "FinancialTransfer",
                column: "FromFinancialAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransfer_ModifyUserId",
                table: "FinancialTransfer",
                column: "ModifyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransfer_ShiftId",
                table: "FinancialTransfer",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransfer_ToFinancialAccountId",
                table: "FinancialTransfer",
                column: "ToFinancialAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountBank_FinancialAccount_FinancialAccountId",
                table: "AccountBank",
                column: "FinancialAccountId",
                principalTable: "FinancialAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Financial_FinancialAccount_ContraFinancialAccountId",
                table: "Financial",
                column: "ContraFinancialAccountId",
                principalTable: "FinancialAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Financial_FinancialAccount_FinancialAccountId",
                table: "Financial",
                column: "FinancialAccountId",
                principalTable: "FinancialAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Financial_FinancialTransactionType_FinancialTransactionTypeId",
                table: "Financial",
                column: "FinancialTransactionTypeId",
                principalTable: "FinancialTransactionType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Financial_FinancialTransfer_FinancialTransferId",
                table: "Financial",
                column: "FinancialTransferId",
                principalTable: "FinancialTransfer",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Financial_Journal_JournalId",
                table: "Financial",
                column: "JournalId",
                principalTable: "Journal",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Safe_Branch_BranchId",
                table: "Safe",
                column: "BranchId",
                principalTable: "Branch",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Safe_FinancialAccount_FinancialAccountId",
                table: "Safe",
                column: "FinancialAccountId",
                principalTable: "FinancialAccount",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Safe_User_KeeperUserId",
                table: "Safe",
                column: "KeeperUserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountBank_FinancialAccount_FinancialAccountId",
                table: "AccountBank");

            migrationBuilder.DropForeignKey(
                name: "FK_Financial_FinancialAccount_ContraFinancialAccountId",
                table: "Financial");

            migrationBuilder.DropForeignKey(
                name: "FK_Financial_FinancialAccount_FinancialAccountId",
                table: "Financial");

            migrationBuilder.DropForeignKey(
                name: "FK_Financial_FinancialTransactionType_FinancialTransactionTypeId",
                table: "Financial");

            migrationBuilder.DropForeignKey(
                name: "FK_Financial_FinancialTransfer_FinancialTransferId",
                table: "Financial");

            migrationBuilder.DropForeignKey(
                name: "FK_Financial_Journal_JournalId",
                table: "Financial");

            migrationBuilder.DropForeignKey(
                name: "FK_Safe_Branch_BranchId",
                table: "Safe");

            migrationBuilder.DropForeignKey(
                name: "FK_Safe_FinancialAccount_FinancialAccountId",
                table: "Safe");

            migrationBuilder.DropForeignKey(
                name: "FK_Safe_User_KeeperUserId",
                table: "Safe");

            migrationBuilder.DropTable(
                name: "FinancialTransactionType");

            migrationBuilder.DropTable(
                name: "FinancialTransfer");

            migrationBuilder.DropTable(
                name: "FinancialAccount");

            migrationBuilder.DropIndex(
                name: "IX_Safe_BranchId",
                table: "Safe");

            migrationBuilder.DropIndex(
                name: "IX_Safe_FinancialAccountId",
                table: "Safe");

            migrationBuilder.DropIndex(
                name: "IX_Safe_KeeperUserId",
                table: "Safe");

            migrationBuilder.DropIndex(
                name: "IX_Financial_ContraFinancialAccountId",
                table: "Financial");

            migrationBuilder.DropIndex(
                name: "IX_Financial_FinancialAccountId",
                table: "Financial");

            migrationBuilder.DropIndex(
                name: "IX_Financial_FinancialTransactionTypeId",
                table: "Financial");

            migrationBuilder.DropIndex(
                name: "IX_Financial_FinancialTransferId",
                table: "Financial");

            migrationBuilder.DropIndex(
                name: "IX_Financial_JournalId",
                table: "Financial");

            migrationBuilder.DropIndex(
                name: "IX_AccountBank_FinancialAccountId",
                table: "AccountBank");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Safe");

            migrationBuilder.DropColumn(
                name: "FinancialAccountId",
                table: "Safe");

            migrationBuilder.DropColumn(
                name: "KeeperUserId",
                table: "Safe");

            migrationBuilder.DropColumn(
                name: "ContraFinancialAccountId",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "Direction",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "FinancialAccountId",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "FinancialTransactionTypeId",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "FinancialTransferId",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "JournalId",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "ReferenceId",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "ReferenceType",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "AccountNumber",
                table: "AccountBank");

            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "AccountBank");

            migrationBuilder.DropColumn(
                name: "FinancialAccountId",
                table: "AccountBank");

            migrationBuilder.DropColumn(
                name: "IBAN",
                table: "AccountBank");

            migrationBuilder.DropColumn(
                name: "SwiftCode",
                table: "AccountBank");
        }
    }
}
