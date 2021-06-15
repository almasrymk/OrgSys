using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class CreateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Branch",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Branch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classification",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BePurchased = table.Column<bool>(type: "bit", nullable: false),
                    BeSold = table.Column<bool>(type: "bit", nullable: false),
                    BeManufactured = table.Column<bool>(type: "bit", nullable: false),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dealer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeNumber = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dealer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InOut = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogSys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ResourceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScreenName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    EstimateBySecond = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Line = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogType = table.Column<int>(type: "int", nullable: false),
                    LogStatus = table.Column<int>(type: "int", nullable: false),
                    LogAccessLevel = table.Column<int>(type: "int", nullable: false),
                    Sent = table.Column<bool>(type: "bit", nullable: false),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogSys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Preference",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preference", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Property",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shift",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shift", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Table",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfPeople = table.Column<int>(type: "int", nullable: false),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InOut = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Store_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeNumber = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClassificationId = table.Column<long>(type: "bigint", nullable: false),
                    DealerId = table.Column<long>(type: "bigint", nullable: true),
                    Recipe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Classification_ClassificationId",
                        column: x => x.ClassificationId,
                        principalTable: "Classification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Product_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PropertyElement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyElement_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeNumber = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DealerId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentTypeId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    ShiftId = table.Column<long>(type: "bigint", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxType = table.Column<int>(type: "int", nullable: false),
                    Service = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceType = table.Column<int>(type: "int", nullable: false),
                    Net = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Remaining = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Paid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoice_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Invoice_PaymentType_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Invoice_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoice_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProductRecipe",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    RecipeId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRecipe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRecipe_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProductUnit",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DefaultUnit = table.Column<bool>(type: "bit", nullable: false),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductUnit_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductUnit_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProductPropertyElement",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    PropertyElementId = table.Column<long>(type: "bigint", nullable: false),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPropertyElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPropertyElement_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductPropertyElement_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductPropertyElement_PropertyElement_PropertyElementId",
                        column: x => x.PropertyElementId,
                        principalTable: "PropertyElement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeNumber = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Review = table.Column<bool>(type: "bit", nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventory_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowNumber = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Net = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceProduct_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InvoiceProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InvoiceProduct_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InvoiceProduct_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeNumber = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableId = table.Column<long>(type: "bigint", nullable: true),
                    CloseTable = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DealerId = table.Column<long>(type: "bigint", nullable: true),
                    ShiftId = table.Column<long>(type: "bigint", nullable: true),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxType = table.Column<int>(type: "int", nullable: false),
                    Service = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceType = table.Column<int>(type: "int", nullable: false),
                    Net = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Table_TableId",
                        column: x => x.TableId,
                        principalTable: "Table",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryStore",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryId = table.Column<long>(type: "bigint", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Review = table.Column<bool>(type: "bit", nullable: false),
                    CreateTransaction = table.Column<bool>(type: "bit", nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryStore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryStore_Inventory_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InventoryStore_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InventoryStore_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowNumber = table.Column<long>(type: "bigint", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Net = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeNumber = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DealerId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    ToStoreId = table.Column<long>(type: "bigint", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transaction_Store_ToStoreId",
                        column: x => x.ToStoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowNumber = table.Column<long>(type: "bigint", nullable: false),
                    InventoryStoreId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    CalcBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiffQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryProduct_InventoryStore_InventoryStoreId",
                        column: x => x.InventoryStoreId,
                        principalTable: "InventoryStore",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InventoryProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InventoryProduct_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TransactionProduct",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowNumber = table.Column<long>(type: "bigint", nullable: false),
                    TransactionId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TransactionProduct_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TransactionProduct_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TransactionProduct_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "Branch",
                columns: new[] { "Id", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[] { 1L, false, null, null, "Main Branch", 0L, 0, 0L });

            migrationBuilder.InsertData(
                table: "Dealer",
                columns: new[] { "Id", "Address", "Code", "CodeNumber", "Email", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Phone", "Status", "TypeId" },
                values: new object[] { 1L, null, "1", 1L, null, false, null, null, "...", 0L, null, 0, 0L });

            migrationBuilder.InsertData(
                table: "InvoiceType",
                columns: new[] { "Id", "Group", "Hide", "Icon", "ImgPath", "InOut", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 1L, "Sales", false, "simple-icon-basket-loaded", null, -1, null, "Invoice", 0L, 0, 0L },
                    { 2L, "Purchases", false, "simple-icon-basket-loaded", null, 1, null, "Invoice", 0L, 0, 0L },
                    { 3L, "Sales", false, "simple-icon-action-undo", null, 1, null, "Return", 0L, 0, 0L },
                    { 4L, "Purchases", false, "simple-icon-action-undo", null, -1, null, "Return", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                table: "OrderType",
                columns: new[] { "Id", "Hide", "Icon", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 1L, false, "iconsminds-right-1", null, null, "Internal", 0L, 0, 0L },
                    { 2L, false, "iconsminds-left-1", null, null, "External", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                table: "PaymentType",
                columns: new[] { "Id", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 2L, false, null, null, "Check", 0L, 0, 0L },
                    { 1L, false, null, null, "Cash", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Status", "TypeId", "Value" },
                values: new object[,]
                {
                    { 20203L, false, null, "Edit", null, 202L, 0, 0L, "Classifications.Edit" },
                    { 201L, false, null, "Products", null, 20L, 0, 0L, "Products.All" },
                    { 20101L, false, null, "View", null, 201L, 0, 0L, "Products.View" },
                    { 20102L, false, null, "Add", null, 201L, 0, 0L, "Products.Add" },
                    { 20103L, false, null, "Edit", null, 201L, 0, 0L, "Products.Edit" },
                    { 20104L, false, null, "Delete", null, 201L, 0, 0L, "Products.Delete" },
                    { 202L, false, null, "Classifications", null, 20L, 0, 0L, "Classifications.All" },
                    { 20201L, false, null, "View", null, 202L, 0, 0L, "Classifications.View" },
                    { 20204L, false, null, "Delete", null, 202L, 0, 0L, "Classifications.Delete" },
                    { 20304L, false, null, "Delete", null, 203L, 0, 0L, "UnitsMeasure.Delete" },
                    { 20301L, false, null, "View", null, 203L, 0, 0L, "UnitsMeasure.View" },
                    { 20302L, false, null, "Add", null, 203L, 0, 0L, "UnitsMeasure.Add" },
                    { 20303L, false, null, "Edit", null, 203L, 0, 0L, "UnitsMeasure.Edit" },
                    { 20L, false, null, "Sales", null, 1L, 0, 0L, "Sales.All" },
                    { 204L, false, null, "Clients", null, 20L, 0, 0L, "Clients.All" },
                    { 20401L, false, null, "View", null, 204L, 0, 0L, "Clients.View" },
                    { 20402L, false, null, "Add", null, 204L, 0, 0L, "Clients.Add" },
                    { 20403L, false, null, "Edit", null, 204L, 0, 0L, "Clients.Edit" },
                    { 20404L, false, null, "Delete", null, 204L, 0, 0L, "Clients.Delete" },
                    { 203L, false, null, "UnitsMeasure", null, 20L, 0, 0L, "UnitsMeasure.All" },
                    { 10404L, false, null, "Delete", null, 104L, 0, 0L, "Shifts.Delete" },
                    { 20202L, false, null, "Add", null, 202L, 0, 0L, "Classifications.Add" },
                    { 10402L, false, null, "Add", null, 104L, 0, 0L, "Shifts.Add" },
                    { 10403L, false, null, "Edit", null, 104L, 0, 0L, "Shifts.Edit" },
                    { 1L, false, null, "AllPage", null, 0L, 0, 0L, "AllPage.All" },
                    { 10L, false, null, "Setting", null, 1L, 0, 0L, "Setting.All" },
                    { 101L, false, null, "Branchs", null, 10L, 0, 0L, "Branchs.All" },
                    { 10101L, false, null, "View", null, 101L, 0, 0L, "Branchs.View" },
                    { 10103L, false, null, "Edit", null, 101L, 0, 0L, "Branchs.Edit" },
                    { 10104L, false, null, "Delete", null, 101L, 0, 0L, "Branchs.Delete" },
                    { 102L, false, null, "Roles", null, 10L, 0, 0L, "Roles.All" },
                    { 10201L, false, null, "View", null, 102L, 0, 0L, "Roles.View" }
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Status", "TypeId", "Value" },
                values: new object[,]
                {
                    { 10202L, false, null, "Add", null, 102L, 0, 0L, "Roles.Add" },
                    { 10102L, false, null, "Add", null, 101L, 0, 0L, "Branchs.Add" },
                    { 10204L, false, null, "Delete", null, 102L, 0, 0L, "Roles.Delete" },
                    { 103L, false, null, "Users", null, 10L, 0, 0L, "Users.All" },
                    { 10301L, false, null, "View", null, 103L, 0, 0L, "Users.View" },
                    { 10302L, false, null, "Add", null, 103L, 0, 0L, "Users.Add" },
                    { 10303L, false, null, "Edit", null, 103L, 0, 0L, "Users.Edit" },
                    { 10304L, false, null, "Delete", null, 103L, 0, 0L, "Users.Delete" },
                    { 104L, false, null, "Shifts", null, 10L, 0, 0L, "Shifts.All" },
                    { 10203L, false, null, "Edit", null, 102L, 0, 0L, "Roles.Edit" },
                    { 10401L, false, null, "View", null, 104L, 0, 0L, "Shifts.View" }
                });

            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 505L, false, null, "AutoSave", null, 0L, "Transaction", 0, 2L, null, "0" },
                    { 506L, false, null, "TypeSerial", null, 0L, "Transaction", 0, 2L, null, "1" },
                    { 507L, false, null, "AllowRepeated", null, 0L, "Transaction", 0, 2L, null, "1" },
                    { 508L, false, null, "SaveLastStatusSetting", null, 0L, "Transaction", 0, 2L, null, "1" },
                    { 601L, false, null, "DefaultStore", null, 0L, "Transaction", 0, 3L, null, "1" },
                    { 605L, false, null, "TypeSerial", null, 0L, "Transaction", 0, 3L, null, "1" },
                    { 603L, false, null, "OrderTabe", null, 0L, "Transaction", 0, 3L, null, "2" },
                    { 604L, false, null, "AutoSave", null, 0L, "Transaction", 0, 3L, null, "0" },
                    { 606L, false, null, "AllowRepeated", null, 0L, "Transaction", 0, 3L, null, "1" },
                    { 504L, false, null, "OrderTabe", null, 0L, "Transaction", 0, 2L, null, "2" },
                    { 607L, false, null, "SaveLastStatusSetting", null, 0L, "Transaction", 0, 3L, null, "1" },
                    { 602L, false, null, "NumberLine", null, 0L, "Transaction", 0, 3L, null, "6" },
                    { 503L, false, null, "NumberLine", null, 0L, "Transaction", 0, 2L, null, "6" },
                    { 401L, false, null, "DefaultStore", null, 0L, "Transaction", 0, 1L, null, "1" },
                    { 501L, false, null, "DefaultStore", null, 0L, "Transaction", 0, 2L, null, "1" },
                    { 408L, false, null, "SaveLastStatusSetting", null, 0L, "Transaction", 0, 1L, null, "1" },
                    { 407L, false, null, "AllowRepeated", null, 0L, "Transaction", 0, 1L, null, "1" },
                    { 406L, false, null, "TypeSerial", null, 0L, "Transaction", 0, 1L, null, "1" },
                    { 405L, false, null, "AutoSave", null, 0L, "Transaction", 0, 1L, null, "0" },
                    { 404L, false, null, "OrderTabe", null, 0L, "Transaction", 0, 1L, null, "2" },
                    { 403L, false, null, "NumberLine", null, 0L, "Transaction", 0, 1L, null, "6" },
                    { 402L, false, null, "DefaultSupplier", null, 0L, "Transaction", 0, 1L, null, "1" },
                    { 316L, false, null, "AutoCreateTransaction", null, 0L, "Invoice", 0, 4L, null, "0" },
                    { 315L, false, null, "SaveLastStatusSetting", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 314L, false, null, "AllowRepeated", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 608L, false, null, "AutoReceived", null, 0L, "Transaction", 0, 3L, null, "0" },
                    { 502L, false, null, "DefaultCustomer", null, 0L, "Transaction", 0, 2L, null, "1" },
                    { 701L, false, null, "DefaultStore", null, 0L, "Transaction", 0, 4L, null, "1" },
                    { 910L, false, null, "TaxValue", null, 0L, "Order", 0, 2L, null, "14" },
                    { 703L, false, null, "OrderTabe", null, 0L, "Transaction", 0, 4L, null, "2" },
                    { 912L, false, null, "AutoCreateInvoice", null, 0L, "Order", 0, 2L, null, "0" }
                });

            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 911L, false, null, "DefaultTaxType", null, 0L, "Order", 0, 2L, null, "2" },
                    { 313L, false, null, "TypeSerial", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 909L, false, null, "DefaultServiceType", null, 0L, "Order", 0, 2L, null, "2" },
                    { 908L, false, null, "ServiceValue", null, 0L, "Order", 0, 2L, null, "" },
                    { 907L, false, null, "DefaultDiscountType", null, 0L, "Order", 0, 2L, null, "2" },
                    { 906L, false, null, "DiscountValue", null, 0L, "Order", 0, 2L, null, "" },
                    { 905L, false, null, "AllowRepeated", null, 0L, "Order", 0, 2L, null, "1" },
                    { 904L, false, null, "TypeSerial", null, 0L, "Order", 0, 2L, null, "1" },
                    { 903L, false, null, "AutoSave", null, 0L, "Order", 0, 2L, null, "0" },
                    { 902L, false, null, "OrderTabe", null, 0L, "Order", 0, 2L, null, "2" },
                    { 901L, false, null, "NumberLine", null, 0L, "Order", 0, 2L, null, "6" },
                    { 812L, false, null, "AutoCreateInvoice", null, 0L, "Order", 0, 1L, null, "0" },
                    { 811L, false, null, "DefaultTaxType", null, 0L, "Order", 0, 1L, null, "2" },
                    { 810L, false, null, "TaxValue", null, 0L, "Order", 0, 1L, null, "14" },
                    { 809L, false, null, "DefaultServiceType", null, 0L, "Order", 0, 1L, null, "2" },
                    { 808L, false, null, "ServiceValue", null, 0L, "Order", 0, 1L, null, "" },
                    { 807L, false, null, "DefaultDiscountType", null, 0L, "Order", 0, 1L, null, "2" },
                    { 806L, false, null, "DiscountValue", null, 0L, "Order", 0, 1L, null, "" },
                    { 805L, false, null, "AllowRepeated", null, 0L, "Order", 0, 1L, null, "1" },
                    { 804L, false, null, "TypeSerial", null, 0L, "Order", 0, 1L, null, "1" },
                    { 803L, false, null, "AutoSave", null, 0L, "Order", 0, 1L, null, "0" },
                    { 802L, false, null, "OrderTabe", null, 0L, "Order", 0, 1L, null, "2" },
                    { 801L, false, null, "NumberLine", null, 0L, "Order", 0, 1L, null, "6" },
                    { 706L, false, null, "AllowRepeated", null, 0L, "Transaction", 0, 4L, null, "1" },
                    { 705L, false, null, "TypeSerial", null, 0L, "Transaction", 0, 4L, null, "1" },
                    { 704L, false, null, "AutoSave", null, 0L, "Transaction", 0, 4L, null, "0" },
                    { 702L, false, null, "NumberLine", null, 0L, "Transaction", 0, 4L, null, "6" },
                    { 312L, false, null, "AutoSave", null, 0L, "Invoice", 0, 4L, null, "0" },
                    { 203L, false, null, "DefaultPaymentType", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 310L, false, null, "NumberLine", null, 0L, "Invoice", 0, 4L, null, "6" },
                    { 110L, false, null, "NumberLine", null, 0L, "Invoice", 0, 2L, null, "6" },
                    { 109L, false, null, "DefaultTaxType", null, 0L, "Invoice", 0, 2L, null, "2" },
                    { 108L, false, null, "TaxValue", null, 0L, "Invoice", 0, 2L, null, "14" },
                    { 107L, false, null, "DefaultServiceType", null, 0L, "Invoice", 0, 2L, null, "2" },
                    { 106L, false, null, "ServiceValue", null, 0L, "Invoice", 0, 2L, null, "" },
                    { 105L, false, null, "DefaultDiscountType", null, 0L, "Invoice", 0, 2L, null, "2" },
                    { 104L, false, null, "DiscountValue", null, 0L, "Invoice", 0, 2L, null, "" },
                    { 103L, false, null, "DefaultPaymentType", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 102L, false, null, "DefaultSupplier", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 101L, false, null, "DefaultStore", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 16L, false, null, "AutoCreateTransaction", null, 0L, "Invoice", 0, 1L, null, "0" },
                    { 15L, false, null, "SaveLastStatusSetting", null, 0L, "Invoice", 0, 1L, null, "1" }
                });

            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 311L, false, null, "OrderTabe", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 14L, false, null, "AllowRepeated", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 12L, false, null, "AutoSave", null, 0L, "Invoice", 0, 1L, null, "0" },
                    { 11L, false, null, "OrderTabe", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 10L, false, null, "NumberLine", null, 0L, "Invoice", 0, 1L, null, "6" },
                    { 9L, false, null, "DefaultTaxType", null, 0L, "Invoice", 0, 1L, null, "2" },
                    { 8L, false, null, "TaxValue", null, 0L, "Invoice", 0, 1L, null, "14" },
                    { 7L, false, null, "DefaultServiceType", null, 0L, "Invoice", 0, 1L, null, "2" },
                    { 6L, false, null, "ServiceValue", null, 0L, "Invoice", 0, 1L, null, "" },
                    { 5L, false, null, "DefaultDiscountType", null, 0L, "Invoice", 0, 1L, null, "2" },
                    { 4L, false, null, "DiscountValue", null, 0L, "Invoice", 0, 1L, null, "" },
                    { 3L, false, null, "DefaultPaymentType", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 2L, false, null, "DefaultCustomer", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 1L, false, null, "DefaultStore", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 13L, false, null, "TypeSerial", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 112L, false, null, "AutoSave", null, 0L, "Invoice", 0, 2L, null, "0" },
                    { 111L, false, null, "OrderTabe", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 114L, false, null, "AllowRepeated", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 309L, false, null, "DefaultTaxType", null, 0L, "Invoice", 0, 4L, null, "2" },
                    { 308L, false, null, "TaxValue", null, 0L, "Invoice", 0, 4L, null, "14" },
                    { 307L, false, null, "DefaultServiceType", null, 0L, "Invoice", 0, 4L, null, "2" },
                    { 306L, false, null, "ServiceValue", null, 0L, "Invoice", 0, 4L, null, "" },
                    { 305L, false, null, "DefaultDiscountType", null, 0L, "Invoice", 0, 4L, null, "2" },
                    { 304L, false, null, "DiscountValue", null, 0L, "Invoice", 0, 4L, null, "" },
                    { 303L, false, null, "DefaultPaymentType", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 302L, false, null, "DefaultSupplier", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 301L, false, null, "DefaultStore", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 216L, false, null, "AutoCreateTransaction", null, 0L, "Invoice", 0, 4L, null, "0" },
                    { 215L, false, null, "SaveLastStatusSetting", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 113L, false, null, "TypeSerial", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 213L, false, null, "TypeSerial", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 214L, false, null, "AllowRepeated", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 211L, false, null, "OrderTabe", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 210L, false, null, "NumberLine", null, 0L, "Invoice", 0, 3L, null, "6" },
                    { 209L, false, null, "DefaultTaxType", null, 0L, "Invoice", 0, 3L, null, "2" },
                    { 208L, false, null, "TaxValue", null, 0L, "Invoice", 0, 3L, null, "14" },
                    { 207L, false, null, "DefaultServiceType", null, 0L, "Invoice", 0, 3L, null, "2" },
                    { 206L, false, null, "ServiceValue", null, 0L, "Invoice", 0, 3L, null, "" },
                    { 205L, false, null, "DefaultDiscountType", null, 0L, "Invoice", 0, 3L, null, "2" },
                    { 204L, false, null, "DiscountValue", null, 0L, "Invoice", 0, 3L, null, "" },
                    { 212L, false, null, "AutoSave", null, 0L, "Invoice", 0, 3L, null, "0" },
                    { 202L, false, null, "DefaultCustomer", null, 0L, "Invoice", 0, 3L, null, "1" }
                });

            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 201L, false, null, "DefaultStore", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 116L, false, null, "AutoCreateTransaction", null, 0L, "Invoice", 0, 2L, null, "0" },
                    { 115L, false, null, "SaveLastStatusSetting", null, 0L, "Invoice", 0, 2L, null, "1" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[] { 1L, false, null, null, "Owner", 0L, 0, 0L });

            migrationBuilder.InsertData(
                table: "TransactionType",
                columns: new[] { "Id", "Hide", "Icon", "ImgPath", "InOut", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 2L, false, "iconsminds-up-1", null, -1, null, "Issue", 0L, 0, 0L },
                    { 3L, false, "iconsminds-shuffle-1", null, -1, null, "Transafer", 0L, 0, 0L },
                    { 1L, false, "iconsminds-down-1", null, 1, null, "Addition", 0L, 0, 0L },
                    { 4L, false, "iconsminds-file-edit", null, 1, null, "Received", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                table: "Store",
                columns: new[] { "Id", "BranchId", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[] { 1L, 1L, false, null, null, "Main Store", 0L, 0, 0L });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "BranchId", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Password", "RoleId", "Status", "TypeId", "UserName" },
                values: new object[] { 1L, null, false, null, null, "Owner", 0L, "iebLM3YfOZ4fcXYL1jInxA==", 1L, 0, 0L, "Owner" });

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_UserId",
                table: "Inventory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryProduct_InventoryStoreId",
                table: "InventoryProduct",
                column: "InventoryStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryProduct_ProductId",
                table: "InventoryProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryProduct_UnitId",
                table: "InventoryProduct",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryStore_InventoryId",
                table: "InventoryStore",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryStore_StoreId",
                table: "InventoryStore",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryStore_UserId",
                table: "InventoryStore",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_DealerId",
                table: "Invoice",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_PaymentTypeId",
                table: "Invoice",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_ShiftId",
                table: "Invoice",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_StoreId",
                table: "Invoice",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceProduct_InvoiceId",
                table: "InvoiceProduct",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceProduct_ProductId",
                table: "InvoiceProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceProduct_StoreId",
                table: "InvoiceProduct",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceProduct_UnitId",
                table: "InvoiceProduct",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DealerId",
                table: "Order",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_InvoiceId",
                table: "Order",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShiftId",
                table: "Order",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_TableId",
                table: "Order",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_OrderId",
                table: "OrderProduct",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_ProductId",
                table: "OrderProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_UnitId",
                table: "OrderProduct",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ClassificationId",
                table: "Product",
                column: "ClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_DealerId",
                table: "Product",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPropertyElement_ProductId",
                table: "ProductPropertyElement",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPropertyElement_PropertyElementId",
                table: "ProductPropertyElement",
                column: "PropertyElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPropertyElement_PropertyId",
                table: "ProductPropertyElement",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecipe_ProductId",
                table: "ProductRecipe",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUnit_ProductId",
                table: "ProductUnit",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUnit_UnitId",
                table: "ProductUnit",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyElement_PropertyId",
                table: "PropertyElement",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Store_BranchId",
                table: "Store",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DealerId",
                table: "Transaction",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_InvoiceId",
                table: "Transaction",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_OrderId",
                table: "Transaction",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_StoreId",
                table: "Transaction",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_ToStoreId",
                table: "Transaction",
                column: "ToStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionProduct_ProductId",
                table: "TransactionProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionProduct_StoreId",
                table: "TransactionProduct",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionProduct_TransactionId",
                table: "TransactionProduct",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionProduct_UnitId",
                table: "TransactionProduct",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_User_BranchId",
                table: "User",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryProduct");

            migrationBuilder.DropTable(
                name: "InvoiceProduct");

            migrationBuilder.DropTable(
                name: "InvoiceType");

            migrationBuilder.DropTable(
                name: "LogSys");

            migrationBuilder.DropTable(
                name: "OrderProduct");

            migrationBuilder.DropTable(
                name: "OrderType");

            migrationBuilder.DropTable(
                name: "Permission");

            migrationBuilder.DropTable(
                name: "Preference");

            migrationBuilder.DropTable(
                name: "ProductPropertyElement");

            migrationBuilder.DropTable(
                name: "ProductRecipe");

            migrationBuilder.DropTable(
                name: "ProductUnit");

            migrationBuilder.DropTable(
                name: "TransactionProduct");

            migrationBuilder.DropTable(
                name: "TransactionType");

            migrationBuilder.DropTable(
                name: "InventoryStore");

            migrationBuilder.DropTable(
                name: "PropertyElement");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "Property");

            migrationBuilder.DropTable(
                name: "Classification");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "Table");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Dealer");

            migrationBuilder.DropTable(
                name: "PaymentType");

            migrationBuilder.DropTable(
                name: "Shift");

            migrationBuilder.DropTable(
                name: "Store");

            migrationBuilder.DropTable(
                name: "Branch");
        }
    }
}
