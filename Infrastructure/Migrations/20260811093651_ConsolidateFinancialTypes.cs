using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConsolidateFinancialTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                MERGE [FinancialType] AS target
                USING (VALUES
                    (CAST(1 AS bigint), N'OpeningBalance', 1, N'iconsminds-start-2'),
                    (CAST(2 AS bigint), N'Receipt', 1, N'iconsminds-financial'),
                    (CAST(3 AS bigint), N'Payment', -1, N'iconsminds-handshake'),
                    (CAST(4 AS bigint), N'Transfer', 0, N'simple-icon-shuffle'),
                    (CAST(5 AS bigint), N'Deposit', 1, N'iconsminds-down-1'),
                    (CAST(6 AS bigint), N'Withdrawal', -1, N'iconsminds-up-1'),
                    (CAST(7 AS bigint), N'Fee', -1, N'iconsminds-receipt-4'),
                    (CAST(8 AS bigint), N'Interest', 1, N'iconsminds-line-chart-1'),
                    (CAST(9 AS bigint), N'Cheque', 0, N'iconsminds-check'),
                    (CAST(10 AS bigint), N'Adjustment', 0, N'iconsminds-gear')
                ) AS source ([Id], [Name], [InOut], [Icon])
                ON target.[Id] = source.[Id]
                WHEN MATCHED THEN UPDATE SET
                    target.[Name] = source.[Name], target.[InOut] = source.[InOut],
                    target.[Icon] = source.[Icon], target.[Hide] = 0
                WHEN NOT MATCHED THEN INSERT
                    ([Id], [Name], [InOut], [Icon], [CodeNumber], [ParentId], [TypeId], [Hide], [Status])
                VALUES
                    (source.[Id], source.[Name], source.[InOut], source.[Icon], source.[Id], 0, 0, 0, 0);
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_Financial_FinancialTransactionType_FinancialTransactionTypeId",
                table: "Financial");

            migrationBuilder.DropTable(
                name: "FinancialTransactionType");

            migrationBuilder.RenameColumn(
                name: "FinancialTransactionTypeId",
                table: "Financial",
                newName: "FinancialTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Financial_FinancialTransactionTypeId",
                table: "Financial",
                newName: "IX_Financial_FinancialTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Financial_FinancialType_FinancialTypeId",
                table: "Financial",
                column: "FinancialTypeId",
                principalTable: "FinancialType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Financial_FinancialType_FinancialTypeId",
                table: "Financial");

            migrationBuilder.RenameColumn(
                name: "FinancialTypeId",
                table: "Financial",
                newName: "FinancialTransactionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Financial_FinancialTypeId",
                table: "Financial",
                newName: "IX_Financial_FinancialTransactionTypeId");

            migrationBuilder.CreateTable(
                name: "FinancialTransactionType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeNumber = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialTransactionType", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Financial_FinancialTransactionType_FinancialTransactionTypeId",
                table: "Financial",
                column: "FinancialTransactionTypeId",
                principalTable: "FinancialTransactionType",
                principalColumn: "Id");
        }
    }
}
