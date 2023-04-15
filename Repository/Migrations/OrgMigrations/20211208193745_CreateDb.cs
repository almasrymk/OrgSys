using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations.OrgMigrations
{
    public partial class CreateDb : Migration
    {
        public string _Schema { get; set; } = "org";
        public CreateDb() { }
        public CreateDb(string Schema) { this._Schema = Schema; }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: _Schema);

            migrationBuilder.CreateTable(
                name: "AccountType",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DebitOrCredit = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_AccountType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bank",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_Bank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Branch",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_Branch", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Classification",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BePurchased = table.Column<bool>(type: "bit", nullable: false),
                    BeSold = table.Column<bool>(type: "bit", nullable: false),
                    BeManufactured = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Classification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CompanyProfile",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone1 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Phone2 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Mobile1 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Mobile2 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Fax1 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Fax2 = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Email1 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Email2 = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CommercialRegister = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    TaxCard = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Watsapp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TypeActivity = table.Column<long>(type: "bigint", nullable: false),
                    NationalityId = table.Column<long>(type: "bigint", nullable: false),
                    SizeOfCompany = table.Column<long>(type: "bigint", nullable: false),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_CompanyProfile", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currency",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DealerGroup",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_DealerGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialType",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InOut = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_FinancialType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceType",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InOut = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Group = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_InvoiceType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogSys",
                schema: _Schema,
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
                    table.PrimaryKey("PK_LogSys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisappearanceAfter = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FromUserId = table.Column<long>(type: "bigint", nullable: false),
                    ToUserId = table.Column<long>(type: "bigint", nullable: false),
                    Read = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_Notification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderType",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_OrderType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Outlay",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_Outlay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentType",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_PaymentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Permission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Preference",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Preference", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Property",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_Property", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Safe",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_Safe", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shift",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    End = table.Column<TimeSpan>(type: "time", nullable: false),
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
                    table.PrimaryKey("PK_Shift", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Table",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfPeople = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_Table", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionType",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InOut = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_TransactionType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unit",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_Unit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccountTypeId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_AccountType_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalSchema: _Schema,
                        principalTable: "AccountType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BranchId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Stock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stock_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: _Schema,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "City",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_City_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: _Schema,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PropertyElement",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_PropertyElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyElement_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: _Schema,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: _Schema,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: _Schema,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    BranchId = table.Column<long>(type: "bigint", nullable: true),
                    LoginUserId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: _Schema,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: _Schema,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "District",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_District", x => x.Id);
                    table.ForeignKey(
                        name: "FK_District_City_CityId",
                        column: x => x.CityId,
                        principalSchema: _Schema,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_District_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: _Schema,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    StockId = table.Column<long>(type: "bigint", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Review = table.Column<bool>(type: "bit", nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
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
                    Posted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventory_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: _Schema,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Inventory_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalSchema: _Schema,
                        principalTable: "Shift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Inventory_Stock_StockId",
                        column: x => x.StockId,
                        principalSchema: _Schema,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Inventory_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Inventory_User_ModifyUserId",
                        column: x => x.ModifyUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Inventory_User_UserId",
                        column: x => x.UserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Journal",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: false),
                    RefranceId = table.Column<long>(type: "bigint", nullable: false),
                    RefranceTypeId = table.Column<long>(type: "bigint", nullable: false),
                    RefranceTable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Journal", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Journal_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: _Schema,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Journal_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: _Schema,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Journal_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalSchema: _Schema,
                        principalTable: "Shift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Journal_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Journal_User_ModifyUserId",
                        column: x => x.ModifyUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "BankBranch",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BankId = table.Column<long>(type: "bigint", nullable: false),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    DistrictId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_BankBranch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankBranch_Bank_BankId",
                        column: x => x.BankId,
                        principalSchema: _Schema,
                        principalTable: "Bank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BankBranch_City_CityId",
                        column: x => x.CityId,
                        principalSchema: _Schema,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BankBranch_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: _Schema,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BankBranch_District_DistrictId",
                        column: x => x.DistrictId,
                        principalSchema: _Schema,
                        principalTable: "District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Dealer",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DealerGroupId = table.Column<long>(type: "bigint", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    DistrictId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Dealer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dealer_City_CityId",
                        column: x => x.CityId,
                        principalSchema: _Schema,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Dealer_Country_CountryId",
                        column: x => x.CountryId,
                        principalSchema: _Schema,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Dealer_DealerGroup_DealerGroupId",
                        column: x => x.DealerGroupId,
                        principalSchema: _Schema,
                        principalTable: "DealerGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Dealer_District_DistrictId",
                        column: x => x.DistrictId,
                        principalSchema: _Schema,
                        principalTable: "District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "JournalItem",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JournalId = table.Column<long>(type: "bigint", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: false),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_JournalItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JournalItem_Account_AccountId",
                        column: x => x.AccountId,
                        principalSchema: _Schema,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_JournalItem_Journal_JournalId",
                        column: x => x.JournalId,
                        principalSchema: _Schema,
                        principalTable: "Journal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AccountBank",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Debit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BankId = table.Column<long>(type: "bigint", nullable: false),
                    BankBranchd = table.Column<long>(type: "bigint", nullable: true),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_AccountBank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountBank_Account_AccountId",
                        column: x => x.AccountId,
                        principalSchema: _Schema,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AccountBank_Bank_BankId",
                        column: x => x.BankId,
                        principalSchema: _Schema,
                        principalTable: "Bank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_AccountBank_BankBranch_BankBranchd",
                        column: x => x.BankBranchd,
                        principalSchema: _Schema,
                        principalTable: "BankBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Financial",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentTypeId = table.Column<long>(type: "bigint", nullable: false),
                    OutlayId = table.Column<long>(type: "bigint", nullable: true),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SafeId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountByDefaultCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_Financial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Financial_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: _Schema,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Financial_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: _Schema,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Financial_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalSchema: _Schema,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Financial_Outlay_OutlayId",
                        column: x => x.OutlayId,
                        principalSchema: _Schema,
                        principalTable: "Outlay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Financial_PaymentType_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalSchema: _Schema,
                        principalTable: "PaymentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Financial_Safe_SafeId",
                        column: x => x.SafeId,
                        principalSchema: _Schema,
                        principalTable: "Safe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Financial_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalSchema: _Schema,
                        principalTable: "Shift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Financial_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Financial_User_ModifyUserId",
                        column: x => x.ModifyUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nickname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ClassificationId = table.Column<long>(type: "bigint", nullable: false),
                    DealerId = table.Column<long>(type: "bigint", nullable: true),
                    Recipe = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Classification_ClassificationId",
                        column: x => x.ClassificationId,
                        principalSchema: _Schema,
                        principalTable: "Classification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Product_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalSchema: _Schema,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "InventoryProduct",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowNumber = table.Column<long>(type: "bigint", nullable: false),
                    InventoryId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    CalcBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ActualBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiffQuantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_InventoryProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryProduct_Inventory_InventoryId",
                        column: x => x.InventoryId,
                        principalSchema: _Schema,
                        principalTable: "Inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InventoryProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: _Schema,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InventoryProduct_Unit_UnitId",
                        column: x => x.UnitId,
                        principalSchema: _Schema,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProductPropertyElement",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: true),
                    PropertyId = table.Column<long>(type: "bigint", nullable: true),
                    PropertyElementId = table.Column<long>(type: "bigint", nullable: true),
                    IsChecked = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_ProductPropertyElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPropertyElement_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: _Schema,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductPropertyElement_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: _Schema,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductPropertyElement_PropertyElement_PropertyElementId",
                        column: x => x.PropertyElementId,
                        principalSchema: _Schema,
                        principalTable: "PropertyElement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProductRecipe",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    RecipeId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_ProductRecipe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRecipe_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: _Schema,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ProductUnit",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DefaultUnit = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_ProductUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductUnit_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: _Schema,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ProductUnit_Unit_UnitId",
                        column: x => x.UnitId,
                        principalSchema: _Schema,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerId = table.Column<long>(type: "bigint", nullable: false),
                    PaymentTypeId = table.Column<long>(type: "bigint", nullable: false),
                    StockId = table.Column<long>(type: "bigint", nullable: true),
                    TransactionId = table.Column<long>(type: "bigint", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountType = table.Column<int>(type: "int", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxType = table.Column<int>(type: "int", nullable: false),
                    Service = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServiceType = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<long>(type: "bigint", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Net = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetByDefaultCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Remaining = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Paid = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreditByDefaultCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_Invoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoice_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: _Schema,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Invoice_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: _Schema,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Invoice_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalSchema: _Schema,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Invoice_PaymentType_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalSchema: _Schema,
                        principalTable: "PaymentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Invoice_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalSchema: _Schema,
                        principalTable: "Shift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Invoice_Stock_StockId",
                        column: x => x.StockId,
                        principalSchema: _Schema,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Invoice_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Invoice_User_ModifyUserId",
                        column: x => x.ModifyUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FinancialInvoice",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowNumber = table.Column<long>(type: "bigint", nullable: false),
                    FinancialId = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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
                    table.PrimaryKey("PK_FinancialInvoice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialInvoice_Financial_FinancialId",
                        column: x => x.FinancialId,
                        principalSchema: _Schema,
                        principalTable: "Financial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FinancialInvoice_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: _Schema,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceProduct",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowNumber = table.Column<long>(type: "bigint", nullable: false),
                    InvoiceId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    StockId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Service = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Net = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_InvoiceProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceProduct_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: _Schema,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InvoiceProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: _Schema,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InvoiceProduct_Stock_StockId",
                        column: x => x.StockId,
                        principalSchema: _Schema,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_InvoiceProduct_Unit_UnitId",
                        column: x => x.UnitId,
                        principalSchema: _Schema,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableId = table.Column<long>(type: "bigint", nullable: true),
                    CloseTable = table.Column<bool>(type: "bit", nullable: false),
                    DealerId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: _Schema,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Order_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalSchema: _Schema,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Order_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: _Schema,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Order_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalSchema: _Schema,
                        principalTable: "Shift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Order_Table_TableId",
                        column: x => x.TableId,
                        principalSchema: _Schema,
                        principalTable: "Table",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Order_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Order_User_ModifyUserId",
                        column: x => x.ModifyUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "OrderProduct",
                schema: _Schema,
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
                    Service = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Net = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_OrderProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: _Schema,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: _Schema,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Unit_UnitId",
                        column: x => x.UnitId,
                        principalSchema: _Schema,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerId = table.Column<long>(type: "bigint", nullable: true),
                    StockId = table.Column<long>(type: "bigint", nullable: true),
                    ToStockId = table.Column<long>(type: "bigint", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
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
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: _Schema,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transaction_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalSchema: _Schema,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transaction_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: _Schema,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transaction_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalSchema: _Schema,
                        principalTable: "Shift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transaction_Stock_StockId",
                        column: x => x.StockId,
                        principalSchema: _Schema,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transaction_Stock_ToStockId",
                        column: x => x.ToStockId,
                        principalSchema: _Schema,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transaction_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Transaction_User_ModifyUserId",
                        column: x => x.ModifyUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "TransactionProduct",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowNumber = table.Column<long>(type: "bigint", nullable: false),
                    TransactionId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    StockId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_TransactionProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: _Schema,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TransactionProduct_Stock_StockId",
                        column: x => x.StockId,
                        principalSchema: _Schema,
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TransactionProduct_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: _Schema,
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TransactionProduct_Unit_UnitId",
                        column: x => x.UnitId,
                        principalSchema: _Schema,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_AccountTypeId",
                schema: _Schema,
                table: "Account",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountBank_AccountId",
                schema: _Schema,
                table: "AccountBank",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountBank_BankBranchd",
                schema: _Schema,
                table: "AccountBank",
                column: "BankBranchd");

            migrationBuilder.CreateIndex(
                name: "IX_AccountBank_BankId",
                schema: _Schema,
                table: "AccountBank",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_BankBranch_BankId",
                schema: _Schema,
                table: "BankBranch",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_BankBranch_CityId",
                schema: _Schema,
                table: "BankBranch",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_BankBranch_CountryId",
                schema: _Schema,
                table: "BankBranch",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_BankBranch_DistrictId",
                schema: _Schema,
                table: "BankBranch",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_City_CountryId",
                schema: _Schema,
                table: "City",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Dealer_CityId",
                schema: _Schema,
                table: "Dealer",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Dealer_CountryId",
                schema: _Schema,
                table: "Dealer",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Dealer_DealerGroupId",
                schema: _Schema,
                table: "Dealer",
                column: "DealerGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Dealer_DistrictId",
                schema: _Schema,
                table: "Dealer",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_District_CityId",
                schema: _Schema,
                table: "District",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_District_CountryId",
                schema: _Schema,
                table: "District",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Financial_BranchId",
                schema: _Schema,
                table: "Financial",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Financial_CreateUserId",
                schema: _Schema,
                table: "Financial",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Financial_CurrencyId",
                schema: _Schema,
                table: "Financial",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Financial_DealerId",
                schema: _Schema,
                table: "Financial",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_Financial_ModifyUserId",
                schema: _Schema,
                table: "Financial",
                column: "ModifyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Financial_OutlayId",
                schema: _Schema,
                table: "Financial",
                column: "OutlayId");

            migrationBuilder.CreateIndex(
                name: "IX_Financial_PaymentTypeId",
                schema: _Schema,
                table: "Financial",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Financial_SafeId",
                schema: _Schema,
                table: "Financial",
                column: "SafeId");

            migrationBuilder.CreateIndex(
                name: "IX_Financial_ShiftId",
                schema: _Schema,
                table: "Financial",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialInvoice_FinancialId",
                schema: _Schema,
                table: "FinancialInvoice",
                column: "FinancialId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialInvoice_InvoiceId",
                schema: _Schema,
                table: "FinancialInvoice",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_BranchId",
                schema: _Schema,
                table: "Inventory",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_CreateUserId",
                schema: _Schema,
                table: "Inventory",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ModifyUserId",
                schema: _Schema,
                table: "Inventory",
                column: "ModifyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_ShiftId",
                schema: _Schema,
                table: "Inventory",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_StockId",
                schema: _Schema,
                table: "Inventory",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_UserId",
                schema: _Schema,
                table: "Inventory",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryProduct_InventoryId",
                schema: _Schema,
                table: "InventoryProduct",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryProduct_ProductId",
                schema: _Schema,
                table: "InventoryProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryProduct_UnitId",
                schema: _Schema,
                table: "InventoryProduct",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_BranchId",
                schema: _Schema,
                table: "Invoice",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_CreateUserId",
                schema: _Schema,
                table: "Invoice",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_CurrencyId",
                schema: _Schema,
                table: "Invoice",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_DealerId",
                schema: _Schema,
                table: "Invoice",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_ModifyUserId",
                schema: _Schema,
                table: "Invoice",
                column: "ModifyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_PaymentTypeId",
                schema: _Schema,
                table: "Invoice",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_ShiftId",
                schema: _Schema,
                table: "Invoice",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_StockId",
                schema: _Schema,
                table: "Invoice",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_TransactionId",
                schema: _Schema,
                table: "Invoice",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceProduct_InvoiceId",
                schema: _Schema,
                table: "InvoiceProduct",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceProduct_ProductId",
                schema: _Schema,
                table: "InvoiceProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceProduct_StockId",
                schema: _Schema,
                table: "InvoiceProduct",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceProduct_UnitId",
                schema: _Schema,
                table: "InvoiceProduct",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Journal_BranchId",
                schema: _Schema,
                table: "Journal",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Journal_CreateUserId",
                schema: _Schema,
                table: "Journal",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Journal_CurrencyId",
                schema: _Schema,
                table: "Journal",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Journal_ModifyUserId",
                schema: _Schema,
                table: "Journal",
                column: "ModifyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Journal_ShiftId",
                schema: _Schema,
                table: "Journal",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalItem_AccountId",
                schema: _Schema,
                table: "JournalItem",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_JournalItem_JournalId",
                schema: _Schema,
                table: "JournalItem",
                column: "JournalId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_BranchId",
                schema: _Schema,
                table: "Order",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CreateUserId",
                schema: _Schema,
                table: "Order",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_DealerId",
                schema: _Schema,
                table: "Order",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_InvoiceId",
                schema: _Schema,
                table: "Order",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ModifyUserId",
                schema: _Schema,
                table: "Order",
                column: "ModifyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShiftId",
                schema: _Schema,
                table: "Order",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_TableId",
                schema: _Schema,
                table: "Order",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_OrderId",
                schema: _Schema,
                table: "OrderProduct",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_ProductId",
                schema: _Schema,
                table: "OrderProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_UnitId",
                schema: _Schema,
                table: "OrderProduct",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ClassificationId",
                schema: _Schema,
                table: "Product",
                column: "ClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_DealerId",
                schema: _Schema,
                table: "Product",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPropertyElement_ProductId",
                schema: _Schema,
                table: "ProductPropertyElement",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPropertyElement_PropertyElementId",
                schema: _Schema,
                table: "ProductPropertyElement",
                column: "PropertyElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPropertyElement_PropertyId",
                schema: _Schema,
                table: "ProductPropertyElement",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRecipe_ProductId",
                schema: _Schema,
                table: "ProductRecipe",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUnit_ProductId",
                schema: _Schema,
                table: "ProductUnit",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductUnit_UnitId",
                schema: _Schema,
                table: "ProductUnit",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyElement_PropertyId",
                schema: _Schema,
                table: "PropertyElement",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                schema: _Schema,
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                schema: _Schema,
                table: "RolePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Stock_BranchId",
                schema: _Schema,
                table: "Stock",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BranchId",
                schema: _Schema,
                table: "Transaction",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_CreateUserId",
                schema: _Schema,
                table: "Transaction",
                column: "CreateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DealerId",
                schema: _Schema,
                table: "Transaction",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_ModifyUserId",
                schema: _Schema,
                table: "Transaction",
                column: "ModifyUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_OrderId",
                schema: _Schema,
                table: "Transaction",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_ShiftId",
                schema: _Schema,
                table: "Transaction",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_StockId",
                schema: _Schema,
                table: "Transaction",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_ToStockId",
                schema: _Schema,
                table: "Transaction",
                column: "ToStockId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionProduct_ProductId",
                schema: _Schema,
                table: "TransactionProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionProduct_StockId",
                schema: _Schema,
                table: "TransactionProduct",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionProduct_TransactionId",
                schema: _Schema,
                table: "TransactionProduct",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionProduct_UnitId",
                schema: _Schema,
                table: "TransactionProduct",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_User_BranchId",
                schema: _Schema,
                table: "User",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                schema: _Schema,
                table: "User",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Transaction_TransactionId",
                schema: _Schema,
                table: "Invoice",
                column: "TransactionId",
                principalSchema: _Schema,
                principalTable: "Transaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dealer_City_CityId",
                schema: _Schema,
                table: "Dealer");

            migrationBuilder.DropForeignKey(
                name: "FK_District_City_CityId",
                schema: _Schema,
                table: "District");

            migrationBuilder.DropForeignKey(
                name: "FK_Dealer_Country_CountryId",
                schema: _Schema,
                table: "Dealer");

            migrationBuilder.DropForeignKey(
                name: "FK_District_Country_CountryId",
                schema: _Schema,
                table: "District");

            migrationBuilder.DropForeignKey(
                name: "FK_Dealer_District_DistrictId",
                schema: _Schema,
                table: "Dealer");

            migrationBuilder.DropForeignKey(
                name: "FK_Dealer_DealerGroup_DealerGroupId",
                schema: _Schema,
                table: "Dealer");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Branch_BranchId",
                schema: _Schema,
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Branch_BranchId",
                schema: _Schema,
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Stock_Branch_BranchId",
                schema: _Schema,
                table: "Stock");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Branch_BranchId",
                schema: _Schema,
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Branch_BranchId",
                schema: _Schema,
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Currency_CurrencyId",
                schema: _Schema,
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Dealer_DealerId",
                schema: _Schema,
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Dealer_DealerId",
                schema: _Schema,
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Dealer_DealerId",
                schema: _Schema,
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_PaymentType_PaymentTypeId",
                schema: _Schema,
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Shift_ShiftId",
                schema: _Schema,
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Shift_ShiftId",
                schema: _Schema,
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Shift_ShiftId",
                schema: _Schema,
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_User_CreateUserId",
                schema: _Schema,
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_User_ModifyUserId",
                schema: _Schema,
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_CreateUserId",
                schema: _Schema,
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_User_ModifyUserId",
                schema: _Schema,
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_User_CreateUserId",
                schema: _Schema,
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_User_ModifyUserId",
                schema: _Schema,
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Invoice_InvoiceId",
                schema: _Schema,
                table: "Order");

            migrationBuilder.DropTable(
                name: "AccountBank",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "CompanyProfile",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "FinancialInvoice",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "FinancialType",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "InventoryProduct",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "InvoiceProduct",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "InvoiceType",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "JournalItem",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "LogSys",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Notification",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "OrderProduct",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "OrderType",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Preference",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "ProductPropertyElement",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "ProductRecipe",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "ProductUnit",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "RolePermission",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "TransactionProduct",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "TransactionType",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "BankBranch",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Financial",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Inventory",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Account",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Journal",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "PropertyElement",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Permission",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Product",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Unit",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Bank",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Outlay",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Safe",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "AccountType",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Property",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Classification",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "City",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Country",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "District",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "DealerGroup",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Branch",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Currency",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Dealer",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "PaymentType",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Shift",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "User",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Role",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Invoice",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Transaction",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Order",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Stock",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Table",
                schema: _Schema);
        }
    }
}
