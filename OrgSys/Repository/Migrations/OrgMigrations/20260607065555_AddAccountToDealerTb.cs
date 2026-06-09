using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations.OrgMigrations
{
    /// <inheritdoc />
    public partial class AddAccountToDealerTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                schema: "org",
                table: "Dealer",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DealerBalanceReport",
                schema: "org",
                columns: table => new
                {
                    DealerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealerImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OpenningBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalInvoice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalReturnInvoice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCreditInvoice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPaidInvoice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerBalanceReport", x => x.DealerId);
                });

            migrationBuilder.CreateTable(
                name: "DealerListReport",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealerGroupId = table.Column<long>(type: "bigint", nullable: true),
                    DealerGroupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    CityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DistrictId = table.Column<long>(type: "bigint", nullable: true),
                    DistrictName = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_DealerListReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DealerStatmentReport",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<long>(type: "bigint", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<long>(type: "bigint", nullable: true),
                    OpenningBalance = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DealerId = table.Column<long>(type: "bigint", nullable: false),
                    DealerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealerImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InOut = table.Column<int>(type: "int", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealerStatmentReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductBalanceReport",
                schema: "org",
                columns: table => new
                {
                    ProductId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassificationId = table.Column<long>(type: "bigint", nullable: false),
                    ClassificationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockId = table.Column<long>(type: "bigint", nullable: false),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductBalanceReport", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "ProductListReport",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClassificationId = table.Column<long>(type: "bigint", nullable: false),
                    ClassificationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemId = table.Column<long>(type: "bigint", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BarCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalesPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductListReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductStatmentReport",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<long>(type: "bigint", nullable: true),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<long>(type: "bigint", nullable: true),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockId = table.Column<long>(type: "bigint", nullable: false),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassificationId = table.Column<long>(type: "bigint", nullable: false),
                    ClassificationName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStatmentReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SafeBalanceReport",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SafeId = table.Column<long>(type: "bigint", nullable: false),
                    SafeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealerId = table.Column<long>(type: "bigint", nullable: false),
                    DealerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SafeImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafeBalanceReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SafeListReport",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafeListReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SafeStatmentReport",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<long>(type: "bigint", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: true),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SafeId = table.Column<long>(type: "bigint", nullable: false),
                    SafeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DealerId = table.Column<long>(type: "bigint", nullable: false),
                    DealerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: false),
                    CurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SafeStatmentReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesBalanceReport",
                schema: "org",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OutAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Net = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesBalanceReport", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "SalesClientReport",
                schema: "org",
                columns: table => new
                {
                    DealerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OutAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Net = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesClientReport", x => x.DealerId);
                });

            migrationBuilder.CreateTable(
                name: "StockBalanceReport",
                schema: "org",
                columns: table => new
                {
                    StockId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClassificationId = table.Column<long>(type: "bigint", nullable: false),
                    ClassificationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockBalanceReport", x => x.StockId);
                });

            migrationBuilder.CreateTable(
                name: "StockListReport",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockListReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockStatmentReport",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReferenceId = table.Column<long>(type: "bigint", nullable: true),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<long>(type: "bigint", nullable: true),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StockId = table.Column<long>(type: "bigint", nullable: false),
                    StockName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockStatmentReport", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dealer_AccountId",
                schema: "org",
                table: "Dealer",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dealer_Account_AccountId",
                schema: "org",
                table: "Dealer",
                column: "AccountId",
                principalSchema: "org",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dealer_Account_AccountId",
                schema: "org",
                table: "Dealer");

            migrationBuilder.DropTable(
                name: "DealerBalanceReport",
                schema: "org");

            migrationBuilder.DropTable(
                name: "DealerListReport",
                schema: "org");

            migrationBuilder.DropTable(
                name: "DealerStatmentReport",
                schema: "org");

            migrationBuilder.DropTable(
                name: "ProductBalanceReport",
                schema: "org");

            migrationBuilder.DropTable(
                name: "ProductListReport",
                schema: "org");

            migrationBuilder.DropTable(
                name: "ProductStatmentReport",
                schema: "org");

            migrationBuilder.DropTable(
                name: "SafeBalanceReport",
                schema: "org");

            migrationBuilder.DropTable(
                name: "SafeListReport",
                schema: "org");

            migrationBuilder.DropTable(
                name: "SafeStatmentReport",
                schema: "org");

            migrationBuilder.DropTable(
                name: "SalesBalanceReport",
                schema: "org");

            migrationBuilder.DropTable(
                name: "SalesClientReport",
                schema: "org");

            migrationBuilder.DropTable(
                name: "StockBalanceReport",
                schema: "org");

            migrationBuilder.DropTable(
                name: "StockListReport",
                schema: "org");

            migrationBuilder.DropTable(
                name: "StockStatmentReport",
                schema: "org");

            migrationBuilder.DropIndex(
                name: "IX_Dealer_AccountId",
                schema: "org",
                table: "Dealer");

            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "org",
                table: "Dealer");
        }
    }
}
