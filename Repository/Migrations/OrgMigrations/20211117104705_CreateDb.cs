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
                });

            migrationBuilder.CreateTable(
                name: "FinancialType",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    table.PrimaryKey("PK_PaymentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
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
                name: "Store",
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
                    table.PrimaryKey("PK_Store", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Store_Branch_BranchId",
                        column: x => x.BranchId,
                        principalSchema: _Schema,
                        principalTable: "Branch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalSchema: _Schema,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: _Schema,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: _Schema,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductUnit_Unit_UnitId",
                        column: x => x.UnitId,
                        principalSchema: _Schema,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPropertyElement_Property_PropertyId",
                        column: x => x.PropertyId,
                        principalSchema: _Schema,
                        principalTable: "Property",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPropertyElement_PropertyElement_PropertyElementId",
                        column: x => x.PropertyElementId,
                        principalSchema: _Schema,
                        principalTable: "PropertyElement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    BranchId = table.Column<long>(type: "bigint", nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Financial_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: _Schema,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Financial_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalSchema: _Schema,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Financial_Outlay_OutlayId",
                        column: x => x.OutlayId,
                        principalSchema: _Schema,
                        principalTable: "Outlay",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Financial_PaymentType_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalSchema: _Schema,
                        principalTable: "PaymentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Financial_Safe_SafeId",
                        column: x => x.SafeId,
                        principalSchema: _Schema,
                        principalTable: "Safe",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Financial_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalSchema: _Schema,
                        principalTable: "Shift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Financial_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Financial_User_ModifyUserId",
                        column: x => x.ModifyUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inventory",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
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
                    BranchId = table.Column<long>(type: "bigint", nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inventory_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalSchema: _Schema,
                        principalTable: "Shift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inventory_Store_StoreId",
                        column: x => x.StoreId,
                        principalSchema: _Schema,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inventory_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventory_User_ModifyUserId",
                        column: x => x.ModifyUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inventory_User_UserId",
                        column: x => x.UserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: _Schema,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryProduct_Unit_UnitId",
                        column: x => x.UnitId,
                        principalSchema: _Schema,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
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
                    BranchId = table.Column<long>(type: "bigint", nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoice_Currency_CurrencyId",
                        column: x => x.CurrencyId,
                        principalSchema: _Schema,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoice_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalSchema: _Schema,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoice_PaymentType_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalSchema: _Schema,
                        principalTable: "PaymentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoice_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalSchema: _Schema,
                        principalTable: "Shift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoice_Store_StoreId",
                        column: x => x.StoreId,
                        principalSchema: _Schema,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoice_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Invoice_User_ModifyUserId",
                        column: x => x.ModifyUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialInvoice_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: _Schema,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: _Schema,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InvoiceProduct_Store_StoreId",
                        column: x => x.StoreId,
                        principalSchema: _Schema,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InvoiceProduct_Unit_UnitId",
                        column: x => x.UnitId,
                        principalSchema: _Schema,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    BranchId = table.Column<long>(type: "bigint", nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalSchema: _Schema,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Invoice_InvoiceId",
                        column: x => x.InvoiceId,
                        principalSchema: _Schema,
                        principalTable: "Invoice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalSchema: _Schema,
                        principalTable: "Shift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_Table_TableId",
                        column: x => x.TableId,
                        principalSchema: _Schema,
                        principalTable: "Table",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Order_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_User_ModifyUserId",
                        column: x => x.ModifyUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalSchema: _Schema,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderProduct_Unit_UnitId",
                        column: x => x.UnitId,
                        principalSchema: _Schema,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                schema: _Schema,
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
                    ToStoreId = table.Column<long>(type: "bigint", nullable: true),
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
                    BranchId = table.Column<long>(type: "bigint", nullable: true)
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalSchema: _Schema,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: _Schema,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Shift_ShiftId",
                        column: x => x.ShiftId,
                        principalSchema: _Schema,
                        principalTable: "Shift",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Store_StoreId",
                        column: x => x.StoreId,
                        principalSchema: _Schema,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_Store_ToStoreId",
                        column: x => x.ToStoreId,
                        principalSchema: _Schema,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transaction_User_CreateUserId",
                        column: x => x.CreateUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_User_ModifyUserId",
                        column: x => x.ModifyUserId,
                        principalSchema: _Schema,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                    StoreId = table.Column<long>(type: "bigint", nullable: true),
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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionProduct_Store_StoreId",
                        column: x => x.StoreId,
                        principalSchema: _Schema,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionProduct_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: _Schema,
                        principalTable: "Transaction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionProduct_Unit_UnitId",
                        column: x => x.UnitId,
                        principalSchema: _Schema,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "Branch",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[] { 1L, null, 0L, false, null, null, "Main Branch", 0L, 0, 0L });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "CompanyProfile",
                columns: new[] { "Id", "Address1", "Address2", "ClientId", "Code", "CodeNumber", "CommercialRegister", "DateCreated", "Description", "Email1", "Email2", "Fax1", "Fax2", "Hide", "ImgPath", "MaskText", "Mobile1", "Mobile2", "Name", "NationalityId", "ParentId", "Phone1", "Phone2", "SizeOfCompany", "Status", "TaxCard", "TypeActivity", "TypeId", "Watsapp", "Website" },
                values: new object[] { 1L, null, null, 1L, "1", 1L, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "info@org.com", null, null, null, true, null, null, "0201111105784", null, "Owner", 68L, 0L, "0201111105784", null, 1L, 0, null, 0L, 0L, null, null });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "Currency",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "IsDefault", "MaskText", "Name", "ParentId", "Rate", "Status", "TypeId" },
                values: new object[] { 1L, null, 0L, false, null, false, null, "Epg", 0L, 0m, 0, 0L });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "Dealer",
                columns: new[] { "Id", "Address", "Code", "CodeNumber", "Email", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Phone", "Status", "TypeId" },
                values: new object[] { 1L, null, "1", 1L, null, false, null, null, "...", 0L, null, 0, 0L });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "FinancialType",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "Icon", "ImgPath", "InOut", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 1L, null, 0L, false, "iconsminds-financial", null, 1, null, "Collection", 0L, 0, 0L },
                    { 2L, null, 0L, false, "iconsminds-handshake", null, -1, null, "Payment", 0L, 0, 0L },
                    { 3L, null, 0L, false, "iconsminds-wallet", null, -1, null, "Outlay", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "InvoiceType",
                columns: new[] { "Id", "Code", "CodeNumber", "Group", "Hide", "Icon", "ImgPath", "InOut", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 1L, null, 0L, "Sales", false, "simple-icon-basket-loaded", null, -1, null, "Invoice", 0L, 0, 0L },
                    { 2L, null, 0L, "Purchases", false, "simple-icon-basket-loaded", null, 1, null, "Invoice", 0L, 0, 0L },
                    { 3L, null, 0L, "Sales", false, "simple-icon-action-undo", null, 1, null, "Return", 0L, 0, 0L },
                    { 4L, null, 0L, "Purchases", false, "simple-icon-action-undo", null, -1, null, "Return", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "OrderType",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "Icon", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 2L, null, 0L, false, "iconsminds-left-1", null, null, "External", 0L, 0, 0L },
                    { 1L, null, 0L, false, "iconsminds-right-1", null, null, "Internal", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "PaymentType",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 1L, null, 0L, false, null, null, "Cash", 0L, 0, 0L },
                    { 2L, null, 0L, false, null, null, "Check", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "Permission",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 3020102L, null, 0L, false, null, "PurchasesInvoices.Add", null, "Add", 30201L, 0, 1L },
                    { 3020103L, null, 0L, false, null, "PurchasesInvoices.Edit", null, "Edit", 30201L, 0, 1L },
                    { 3020104L, null, 0L, false, null, "PurchasesInvoices.Delete", null, "Delete", 30201L, 0, 1L },
                    { 3020105L, null, 0L, false, null, "PurchasesInvoices.Cancel", null, "Cancel", 30201L, 0, 1L },
                    { 3020106L, null, 0L, false, null, "PurchasesInvoices.Preference", null, "Preference", 30201L, 0, 1L },
                    { 30202L, null, 0L, false, null, "PurchasesReturns.All", null, "Returns", 302L, 0, 0L },
                    { 3020201L, null, 0L, false, null, "PurchasesReturns.View", null, "View", 30202L, 0, 1L },
                    { 3020202L, null, 0L, false, null, "PurchasesReturns.Add", null, "Add", 30202L, 0, 1L },
                    { 3020203L, null, 0L, false, null, "PurchasesReturns.Edit", null, "Edit", 30202L, 0, 1L },
                    { 4010101L, null, 0L, false, null, "Addition.View", null, "View", 40101L, 0, 1L },
                    { 3020205L, null, 0L, false, null, "PurchasesReturns.Cancel", null, "Cancel", 30202L, 0, 1L },
                    { 3020206L, null, 0L, false, null, "PurchasesReturns.Preference", null, "Preference", 30202L, 0, 1L },
                    { 40L, null, 0L, false, null, "Transactions.All", null, "Transactions", 1L, 0, 0L },
                    { 401L, null, 0L, false, null, "TransactionNotices", null, "Transaction Notices", 40L, 0, 0L },
                    { 40101L, null, 0L, false, null, "Addition.All", null, "Addition", 401L, 0, 0L },
                    { 3020101L, null, 0L, false, null, "PurchasesInvoices.View", null, "View", 30201L, 0, 1L },
                    { 4010102L, null, 0L, false, null, "Addition.Add", null, "Add", 40101L, 0, 1L },
                    { 4010103L, null, 0L, false, null, "Addition.Edit", null, "Edit", 40101L, 0, 1L },
                    { 3020204L, null, 0L, false, null, "PurchasesReturns.Delete", null, "Delete", 30202L, 0, 1L },
                    { 30201L, null, 0L, false, null, "PurchasesInvoices.All", null, "Invoices", 302L, 0, 0L },
                    { 3010201L, null, 0L, false, null, "SalesReturns.View", null, "View", 30102L, 0, 1L },
                    { 3010206L, null, 0L, false, null, "SalesReturns.Preference", null, "Preference", 30102L, 0, 1L },
                    { 2010204L, null, 0L, false, null, "External.Delete", null, "Delete", 20102L, 0, 1L },
                    { 2010205L, null, 0L, false, null, "External.Cancel", null, "Cancel", 20102L, 0, 1L },
                    { 2010206L, null, 0L, false, null, "External.Preference", null, "Preference", 20102L, 0, 1L },
                    { 30L, null, 0L, false, null, "Invoices.All", null, "Invoices", 1L, 0, 0L },
                    { 301L, null, 0L, false, null, "Sales", null, "Sales", 30L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "Permission",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 30101L, null, 0L, false, null, "SalesInvoices.All", null, "Invoices", 301L, 0, 0L },
                    { 3010101L, null, 0L, false, null, "SalesInvoices.View", null, "View", 30101L, 0, 1L },
                    { 3010102L, null, 0L, false, null, "SalesInvoices.Add", null, "Add", 30101L, 0, 1L },
                    { 302L, null, 0L, false, null, "Purchases", null, "Purchases", 30L, 0, 0L },
                    { 3010103L, null, 0L, false, null, "SalesInvoices.Edit", null, "Edit", 30101L, 0, 1L },
                    { 3010105L, null, 0L, false, null, "SalesInvoices.Cancel", null, "Cancel", 30101L, 0, 1L },
                    { 3010106L, null, 0L, false, null, "SalesInvoices.Preference", null, "Preference", 30101L, 0, 1L },
                    { 30102L, null, 0L, false, null, "SalesReturns.All", null, "Returns", 301L, 0, 0L },
                    { 4010104L, null, 0L, false, null, "Addition.Delete", null, "Delete", 40101L, 0, 1L },
                    { 3010202L, null, 0L, false, null, "SalesReturns.Add", null, "Add", 30102L, 0, 1L },
                    { 3010203L, null, 0L, false, null, "SalesReturns.Edit", null, "Edit", 30102L, 0, 1L },
                    { 3010204L, null, 0L, false, null, "SalesReturns.Delete", null, "Delete", 30102L, 0, 1L },
                    { 3010205L, null, 0L, false, null, "SalesReturns.Cancel", null, "Cancel", 30102L, 0, 1L },
                    { 3010104L, null, 0L, false, null, "SalesInvoices.Delete", null, "Delete", 30101L, 0, 1L },
                    { 4010105L, null, 0L, false, null, "Addition.Preference", null, "Preference", 40101L, 0, 1L },
                    { 4010301L, null, 0L, false, null, "Transafer.View", null, "View", 40103L, 0, 1L },
                    { 4010201L, null, 0L, false, null, "Issue.View", null, "View", 40102L, 0, 1L },
                    { 501L, null, 0L, false, null, "SafeNotices", null, "Safe Notices", 50L, 0, 0L },
                    { 50101L, null, 0L, false, null, "Collection.All", null, "Collection", 501L, 0, 0L },
                    { 5010101L, null, 0L, false, null, "Collection.View", null, "View", 50101L, 0, 1L },
                    { 5010103L, null, 0L, false, null, "Collection.Edit", null, "Edit", 50101L, 0, 1L },
                    { 5010104L, null, 0L, false, null, "Collection.Delete", null, "Delete", 50101L, 0, 1L },
                    { 5010105L, null, 0L, false, null, "Collection.Preference", null, "Preference", 50101L, 0, 1L },
                    { 50102L, null, 0L, false, null, "Payment.All", null, "Payment", 501L, 0, 0L },
                    { 5010201L, null, 0L, false, null, "Payment.View", null, "View", 50102L, 0, 1L },
                    { 50L, null, 0L, false, null, "Financials.All", null, "Financials", 1L, 0, 0L },
                    { 5010202L, null, 0L, false, null, "Payment.Add", null, "Add", 50102L, 0, 1L },
                    { 5010204L, null, 0L, false, null, "Payment.Delete", null, "Delete", 50102L, 0, 1L },
                    { 5010205L, null, 0L, false, null, "Payment.Preference", null, "Preference", 50102L, 0, 1L },
                    { 50103L, null, 0L, false, null, "Outlay.All", null, "Outlay", 501L, 0, 0L },
                    { 5010301L, null, 0L, false, null, "Outlay.View", null, "View", 50103L, 0, 1L },
                    { 5010302L, null, 0L, false, null, "Outlay.Add", null, "Add", 50103L, 0, 1L },
                    { 5010303L, null, 0L, false, null, "Outlay.Edit", null, "Edit", 50103L, 0, 1L },
                    { 5010304L, null, 0L, false, null, "Outlay.Delete", null, "Delete", 50103L, 0, 1L },
                    { 5010305L, null, 0L, false, null, "Outlay.Preference", null, "Preference", 50103L, 0, 1L },
                    { 5010203L, null, 0L, false, null, "Payment.Edit", null, "Edit", 50102L, 0, 1L },
                    { 4010505L, null, 0L, false, null, "Inventory.Preference", null, "Preference", 40105L, 0, 1L },
                    { 4010504L, null, 0L, false, null, "Inventory.Delete", null, "Delete", 40105L, 0, 1L },
                    { 4010503L, null, 0L, false, null, "Inventory.Edit", null, "Edit", 40105L, 0, 1L },
                    { 4010202L, null, 0L, false, null, "Issue.Add", null, "Add", 40102L, 0, 1L },
                    { 4010203L, null, 0L, false, null, "Issue.Edit", null, "Edit", 40102L, 0, 1L },
                    { 4010204L, null, 0L, false, null, "Issue.Delete", null, "Delete", 40102L, 0, 1L }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "Permission",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 4010205L, null, 0L, false, null, "Issue.Preference", null, "Preference", 40102L, 0, 1L },
                    { 40103L, null, 0L, false, null, "Transafer.All", null, "Transafer", 401L, 0, 0L },
                    { 2010203L, null, 0L, false, null, "External.Edit", null, "Edit", 20102L, 0, 1L },
                    { 4010302L, null, 0L, false, null, "Transafer.Add", null, "Add", 40103L, 0, 1L },
                    { 4010303L, null, 0L, false, null, "Transafer.Edit", null, "Edit", 40103L, 0, 1L },
                    { 4010304L, null, 0L, false, null, "Transafer.Delete", null, "Delete", 40103L, 0, 1L },
                    { 4010305L, null, 0L, false, null, "Transafer.Preference", null, "Preference", 40103L, 0, 1L },
                    { 40104L, null, 0L, false, null, "Received.All", null, "Received", 401L, 0, 0L },
                    { 4010401L, null, 0L, false, null, "Received.View", null, "View", 40104L, 0, 1L },
                    { 4010402L, null, 0L, false, null, "Received.Add", null, "Add", 40104L, 0, 1L },
                    { 4010403L, null, 0L, false, null, "Received.Edit", null, "Edit", 40104L, 0, 1L },
                    { 4010404L, null, 0L, false, null, "Received.Delete", null, "Delete", 40104L, 0, 1L },
                    { 4010405L, null, 0L, false, null, "Received.Preference", null, "Preference", 40104L, 0, 1L },
                    { 40105L, null, 0L, false, null, "Inventory.All", null, "Inventory", 401L, 0, 0L },
                    { 4010501L, null, 0L, false, null, "Inventory.View", null, "View", 40105L, 0, 1L },
                    { 4010502L, null, 0L, false, null, "Inventory.Add", null, "Add", 40105L, 0, 1L },
                    { 40102L, null, 0L, false, null, "Issue.All", null, "Issue", 401L, 0, 0L },
                    { 2010202L, null, 0L, false, null, "External.Add", null, "Add", 20102L, 0, 1L },
                    { 5010102L, null, 0L, false, null, "Collection.Add", null, "Add", 50101L, 0, 1L },
                    { 20102L, null, 0L, false, null, "External.All", null, "External", 201L, 0, 0L },
                    { 1020104L, null, 0L, false, null, "Roles.Delete", null, "Delete", 10201L, 0, 1L },
                    { 10202L, null, 0L, false, null, "Users.All", null, "Users", 102L, 0, 0L },
                    { 1020201L, null, 0L, false, null, "Users.View", null, "View", 10202L, 0, 1L },
                    { 1020202L, null, 0L, false, null, "Users.Add", null, "Add", 10202L, 0, 1L },
                    { 1020203L, null, 0L, false, null, "Users.Edit", null, "Edit", 10202L, 0, 1L },
                    { 1020204L, null, 0L, false, null, "Users.Delete", null, "Delete", 10202L, 0, 1L },
                    { 10203L, null, 0L, false, null, "Shifts.All", null, "Shifts", 102L, 0, 0L },
                    { 1020301L, null, 0L, false, null, "Shifts.View", null, "View", 10203L, 0, 1L },
                    { 1020103L, null, 0L, false, null, "Roles.Edit", null, "Edit", 10201L, 0, 1L },
                    { 1020302L, null, 0L, false, null, "Shifts.Add", null, "Add", 10203L, 0, 1L },
                    { 1020304L, null, 0L, false, null, "Shifts.Delete", null, "Delete", 10203L, 0, 1L },
                    { 103L, null, 0L, false, null, "Products", null, "Products", 10L, 0, 0L },
                    { 10301L, null, 0L, false, null, "Products.All", null, "Products", 103L, 0, 0L },
                    { 1030101L, null, 0L, false, null, "Products.View", null, "View", 10301L, 0, 1L },
                    { 1030102L, null, 0L, false, null, "Products.Add", null, "Add", 10301L, 0, 1L },
                    { 1030103L, null, 0L, false, null, "Products.Edit", null, "Edit", 10301L, 0, 1L },
                    { 1030104L, null, 0L, false, null, "Products.Delete", null, "Delete", 10301L, 0, 1L },
                    { 10302L, null, 0L, false, null, "Classifications.All", null, "Classifications", 103L, 0, 0L },
                    { 1020303L, null, 0L, false, null, "Shifts.Edit", null, "Edit", 10203L, 0, 1L },
                    { 1020102L, null, 0L, false, null, "Roles.Add", null, "Add", 10201L, 0, 1L },
                    { 1020101L, null, 0L, false, null, "Roles.View", null, "View", 10201L, 0, 1L },
                    { 10201L, null, 0L, false, null, "Roles.All", null, "Roles", 102L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "Permission",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 2010201L, null, 0L, false, null, "External.View", null, "View", 20102L, 0, 1L },
                    { 1L, null, 0L, false, null, "Organizer", null, "Organizer", 0L, 0, 0L },
                    { 10L, null, 0L, false, null, "Data.All", null, "Data", 1L, 0, 0L },
                    { 101L, null, 0L, false, null, "Organization", null, "Organization", 10L, 0, 0L },
                    { 10101L, null, 0L, false, null, "Branchs.All", null, "Branchs", 101L, 0, 0L },
                    { 1010101L, null, 0L, false, null, "Branchs.View", null, "View", 10101L, 0, 1L },
                    { 1010102L, null, 0L, false, null, "Branchs.Add", null, "Add", 10101L, 0, 1L },
                    { 1010104L, null, 0L, false, null, "Branchs.Delete", null, "Delete", 10101L, 0, 1L },
                    { 10102L, null, 0L, false, null, "Stores.All", null, "Stores", 101L, 0, 0L },
                    { 1010201L, null, 0L, false, null, "Stores.View", null, "View", 10102L, 0, 1L },
                    { 1010202L, null, 0L, false, null, "Stores.Add", null, "Add", 10102L, 0, 1L },
                    { 1010203L, null, 0L, false, null, "Stores.Edit", null, "Edit", 10102L, 0, 1L },
                    { 1010204L, null, 0L, false, null, "Stores.Delete", null, "Delete", 10102L, 0, 1L },
                    { 10103L, null, 0L, false, null, "Tables.All", null, "Tables", 101L, 0, 0L },
                    { 1010301L, null, 0L, false, null, "Tables.View", null, "View", 10103L, 0, 1L },
                    { 1010302L, null, 0L, false, null, "Tables.Add", null, "Add", 10103L, 0, 1L },
                    { 1010303L, null, 0L, false, null, "Tables.Edit", null, "Edit", 10103L, 0, 1L },
                    { 1010304L, null, 0L, false, null, "Tables.Delete", null, "Delete", 10103L, 0, 1L },
                    { 102L, null, 0L, false, null, "Security", null, "Security", 10L, 0, 0L },
                    { 1030201L, null, 0L, false, null, "Classifications.View", null, "View", 10302L, 0, 1L },
                    { 1030202L, null, 0L, false, null, "Classifications.Add", null, "Add", 10302L, 0, 1L },
                    { 1010103L, null, 0L, false, null, "Branchs.Edit", null, "Edit", 10101L, 0, 1L },
                    { 1030204L, null, 0L, false, null, "Classifications.Delete", null, "Delete", 10302L, 0, 1L },
                    { 10502L, null, 0L, false, null, "OutlayTerms.All", null, "Outlay Terms", 105L, 0, 0L },
                    { 1050201L, null, 0L, false, null, "OutlayTerms.View", null, "View", 10502L, 0, 1L },
                    { 1050202L, null, 0L, false, null, "OutlayTerms.Add", null, "Add", 10502L, 0, 1L },
                    { 1050203L, null, 0L, false, null, "OutlayTerms.Edit", null, "Edit", 10502L, 0, 1L },
                    { 1050204L, null, 0L, false, null, "OutlayTerms.Delete", null, "Delete", 10502L, 0, 1L },
                    { 10503L, null, 0L, false, null, "Currencies.All", null, "Currencies", 105L, 0, 0L },
                    { 1050301L, null, 0L, false, null, "Currencies.View", null, "View", 10503L, 0, 1L },
                    { 1050302L, null, 0L, false, null, "Currencies.Add", null, "Add", 10503L, 0, 1L },
                    { 1050104L, null, 0L, false, null, "Safes.Delete", null, "Delete", 10501L, 0, 1L },
                    { 1050303L, null, 0L, false, null, "Currencies.Edit", null, "Edit", 10503L, 0, 1L },
                    { 20L, null, 0L, false, null, "Orders.All", null, "Orders", 1L, 0, 0L },
                    { 201L, null, 0L, false, null, "Orders", null, "Order Notices", 20L, 0, 0L },
                    { 20101L, null, 0L, false, null, "Internal.All", null, "Internal", 201L, 0, 0L },
                    { 2010101L, null, 0L, false, null, "Internal.View", null, "View", 20101L, 0, 1L },
                    { 2010103L, null, 0L, false, null, "Internal.Edit", null, "Edit", 20101L, 0, 1L },
                    { 2010104L, null, 0L, false, null, "Internal.Delete", null, "Delete", 20101L, 0, 1L },
                    { 2010105L, null, 0L, false, null, "Internal.Cancel", null, "Cancel", 20101L, 0, 1L },
                    { 1030203L, null, 0L, false, null, "Classifications.Edit", null, "Edit", 10302L, 0, 1L },
                    { 1050304L, null, 0L, false, null, "Currencies.Delete", null, "Delete", 10503L, 0, 1L }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "Permission",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 1050103L, null, 0L, false, null, "Safes.Edit", null, "Edit", 10501L, 0, 1L },
                    { 2010102L, null, 0L, false, null, "Internal.Add", null, "Add", 20101L, 0, 1L },
                    { 1050101L, null, 0L, false, null, "Safes.View", null, "View", 10501L, 0, 1L },
                    { 10303L, null, 0L, false, null, "UnitsMeasure.All", null, "Units Measure", 103L, 0, 0L },
                    { 1050102L, null, 0L, false, null, "Safes.Add", null, "Add", 10501L, 0, 1L },
                    { 1030301L, null, 0L, false, null, "UnitsMeasure.View", null, "View", 10303L, 0, 1L },
                    { 1030302L, null, 0L, false, null, "UnitsMeasure.Add", null, "Add", 10303L, 0, 1L },
                    { 1030303L, null, 0L, false, null, "UnitsMeasure.Edit", null, "Edit", 10303L, 0, 1L },
                    { 1030304L, null, 0L, false, null, "UnitsMeasure.Delete", null, "Delete", 10303L, 0, 1L },
                    { 104L, null, 0L, false, null, "Dealers", null, "Dealers", 10L, 0, 0L },
                    { 10401L, null, 0L, false, null, "Clients.All", null, "Clients", 104L, 0, 0L },
                    { 1040101L, null, 0L, false, null, "Clients.View", null, "View", 10401L, 0, 1L },
                    { 1040102L, null, 0L, false, null, "Clients.Add", null, "Add", 10401L, 0, 1L },
                    { 2010106L, null, 0L, false, null, "Internal.Preference", null, "Preference", 20101L, 0, 1L },
                    { 1040104L, null, 0L, false, null, "Clients.Delete", null, "Delete", 10401L, 0, 1L },
                    { 10402L, null, 0L, false, null, "Suppliers.All", null, "Suppliers", 104L, 0, 0L },
                    { 1040201L, null, 0L, false, null, "Suppliers.View", null, "View", 10402L, 0, 1L },
                    { 1040202L, null, 0L, false, null, "Suppliers.Add", null, "Add", 10402L, 0, 1L },
                    { 1040103L, null, 0L, false, null, "Clients.Edit", null, "Edit", 10401L, 0, 1L },
                    { 1040203L, null, 0L, false, null, "Suppliers.Edit", null, "Edit", 10402L, 0, 1L },
                    { 1040204L, null, 0L, false, null, "Suppliers.Delete", null, "Delete", 10402L, 0, 1L },
                    { 10501L, null, 0L, false, null, "Safes.All", null, "Safes", 105L, 0, 0L },
                    { 105L, null, 0L, false, null, "Financials", null, "Financials", 10L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "Preference",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 608L, null, 0L, false, null, "AutoReceived", null, 0L, "Transaction", 0, 3L, null, "0" },
                    { 701L, null, 0L, false, null, "DefaultStore", null, 0L, "Transaction", 0, 4L, null, "1" },
                    { 702L, null, 0L, false, null, "NumberLine", null, 0L, "Transaction", 0, 4L, null, "6" },
                    { 703L, null, 0L, false, null, "OrderTabe", null, 0L, "Transaction", 0, 4L, null, "2" },
                    { 704L, null, 0L, false, null, "AutoSave", null, 0L, "Transaction", 0, 4L, null, "0" },
                    { 705L, null, 0L, false, null, "TypeSerial", null, 0L, "Transaction", 0, 4L, null, "1" },
                    { 803L, null, 0L, false, null, "AutoSave", null, 0L, "Order", 0, 1L, null, "0" },
                    { 801L, null, 0L, false, null, "NumberLine", null, 0L, "Order", 0, 1L, null, "6" },
                    { 802L, null, 0L, false, null, "OrderTabe", null, 0L, "Order", 0, 1L, null, "2" },
                    { 804L, null, 0L, false, null, "TypeSerial", null, 0L, "Order", 0, 1L, null, "1" },
                    { 805L, null, 0L, false, null, "AllowRepeated", null, 0L, "Order", 0, 1L, null, "1" },
                    { 806L, null, 0L, false, null, "DiscountValue", null, 0L, "Order", 0, 1L, null, "" },
                    { 807L, null, 0L, false, null, "DefaultDiscountType", null, 0L, "Order", 0, 1L, null, "2" },
                    { 808L, null, 0L, false, null, "ServiceValue", null, 0L, "Order", 0, 1L, null, "" },
                    { 809L, null, 0L, false, null, "DefaultServiceType", null, 0L, "Order", 0, 1L, null, "2" },
                    { 706L, null, 0L, false, null, "AllowRepeated", null, 0L, "Transaction", 0, 4L, null, "1" },
                    { 607L, null, 0L, false, null, "SaveLastStatusSetting", null, 0L, "Transaction", 0, 3L, null, "1" },
                    { 604L, null, 0L, false, null, "AutoSave", null, 0L, "Transaction", 0, 3L, null, "0" },
                    { 605L, null, 0L, false, null, "TypeSerial", null, 0L, "Transaction", 0, 3L, null, "1" }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "Preference",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 406L, null, 0L, false, null, "TypeSerial", null, 0L, "Transaction", 0, 1L, null, "1" },
                    { 810L, null, 0L, false, null, "TaxValue", null, 0L, "Order", 0, 1L, null, "14" },
                    { 405L, null, 0L, false, null, "AutoSave", null, 0L, "Transaction", 0, 1L, null, "0" },
                    { 407L, null, 0L, false, null, "AllowRepeated", null, 0L, "Transaction", 0, 1L, null, "1" },
                    { 408L, null, 0L, false, null, "SaveLastStatusSetting", null, 0L, "Transaction", 0, 1L, null, "1" },
                    { 501L, null, 0L, false, null, "DefaultStore", null, 0L, "Transaction", 0, 2L, null, "1" },
                    { 502L, null, 0L, false, null, "DefaultCustomer", null, 0L, "Transaction", 0, 2L, null, "1" },
                    { 503L, null, 0L, false, null, "NumberLine", null, 0L, "Transaction", 0, 2L, null, "6" },
                    { 504L, null, 0L, false, null, "OrderTabe", null, 0L, "Transaction", 0, 2L, null, "2" },
                    { 505L, null, 0L, false, null, "AutoSave", null, 0L, "Transaction", 0, 2L, null, "0" },
                    { 507L, null, 0L, false, null, "AllowRepeated", null, 0L, "Transaction", 0, 2L, null, "1" },
                    { 508L, null, 0L, false, null, "SaveLastStatusSetting", null, 0L, "Transaction", 0, 2L, null, "1" },
                    { 601L, null, 0L, false, null, "DefaultStore", null, 0L, "Transaction", 0, 3L, null, "1" },
                    { 602L, null, 0L, false, null, "NumberLine", null, 0L, "Transaction", 0, 3L, null, "6" },
                    { 603L, null, 0L, false, null, "OrderTabe", null, 0L, "Transaction", 0, 3L, null, "2" },
                    { 606L, null, 0L, false, null, "AllowRepeated", null, 0L, "Transaction", 0, 3L, null, "1" },
                    { 506L, null, 0L, false, null, "TypeSerial", null, 0L, "Transaction", 0, 2L, null, "1" },
                    { 1004L, null, 0L, false, null, "AutoSave", null, 0L, "Financial", 0, 1L, null, "0" },
                    { 812L, null, 0L, false, null, "AutoCreateInvoice", null, 0L, "Order", 0, 1L, null, "0" },
                    { 1101L, null, 0L, false, null, "DefaultSafe", null, 0L, "Financial", 0, 2L, null, "1" },
                    { 1102L, null, 0L, false, null, "DefaultPaymentType", null, 0L, "Financial", 0, 2L, null, "1" },
                    { 1103L, null, 0L, false, null, "DefaultCurrency", null, 0L, "Financial", 0, 2L, null, "1" },
                    { 1104L, null, 0L, false, null, "AutoSave", null, 0L, "Financial", 0, 2L, null, "0" },
                    { 1105L, null, 0L, false, null, "TypeSerial", null, 0L, "Financial", 0, 2L, null, "1" },
                    { 1200L, null, 0L, false, null, "DefaultOutlay", null, 0L, "Financial", 0, 3L, null, "1" },
                    { 1201L, null, 0L, false, null, "DefaultSafe", null, 0L, "Financial", 0, 3L, null, "1" },
                    { 1202L, null, 0L, false, null, "DefaultPaymentType", null, 0L, "Financial", 0, 3L, null, "1" },
                    { 1203L, null, 0L, false, null, "DefaultCurrency", null, 0L, "Financial", 0, 3L, null, "1" },
                    { 1204L, null, 0L, false, null, "AutoSave", null, 0L, "Financial", 0, 3L, null, "0" },
                    { 1205L, null, 0L, false, null, "TypeSerial", null, 0L, "Financial", 0, 3L, null, "1" },
                    { 1300L, null, 0L, false, null, "DefaultStore", null, 0L, "Inventory", 0, 0L, null, "1" },
                    { 1301L, null, 0L, false, null, "AutoSave", null, 0L, "Inventory", 0, 0L, null, "0" },
                    { 404L, null, 0L, false, null, "OrderTabe", null, 0L, "Transaction", 0, 1L, null, "2" },
                    { 1302L, null, 0L, false, null, "TypeSerial", null, 0L, "Inventory", 0, 0L, null, "1" },
                    { 1100L, null, 0L, false, null, "DefaultSupplier", null, 0L, "Financial", 0, 2L, null, "1" },
                    { 1005L, null, 0L, false, null, "TypeSerial", null, 0L, "Financial", 0, 1L, null, "1" },
                    { 1003L, null, 0L, false, null, "DefaultCurrency", null, 0L, "Financial", 0, 1L, null, "1" },
                    { 1002L, null, 0L, false, null, "DefaultPaymentType", null, 0L, "Financial", 0, 1L, null, "1" },
                    { 813L, null, 0L, false, null, "DefaultCustomer", null, 0L, "Order", 0, 1L, null, "1" },
                    { 901L, null, 0L, false, null, "NumberLine", null, 0L, "Order", 0, 2L, null, "6" },
                    { 902L, null, 0L, false, null, "OrderTabe", null, 0L, "Order", 0, 2L, null, "2" },
                    { 903L, null, 0L, false, null, "AutoSave", null, 0L, "Order", 0, 2L, null, "0" }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "Preference",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 904L, null, 0L, false, null, "TypeSerial", null, 0L, "Order", 0, 2L, null, "1" },
                    { 905L, null, 0L, false, null, "AllowRepeated", null, 0L, "Order", 0, 2L, null, "1" },
                    { 906L, null, 0L, false, null, "DiscountValue", null, 0L, "Order", 0, 2L, null, "" },
                    { 811L, null, 0L, false, null, "DefaultTaxType", null, 0L, "Order", 0, 1L, null, "2" },
                    { 907L, null, 0L, false, null, "DefaultDiscountType", null, 0L, "Order", 0, 2L, null, "2" },
                    { 909L, null, 0L, false, null, "DefaultServiceType", null, 0L, "Order", 0, 2L, null, "2" },
                    { 910L, null, 0L, false, null, "TaxValue", null, 0L, "Order", 0, 2L, null, "14" },
                    { 911L, null, 0L, false, null, "DefaultTaxType", null, 0L, "Order", 0, 2L, null, "2" },
                    { 912L, null, 0L, false, null, "AutoCreateInvoice", null, 0L, "Order", 0, 2L, null, "0" },
                    { 913L, null, 0L, false, null, "DefaultCustomer", null, 0L, "Order", 0, 2L, null, "1" },
                    { 1000L, null, 0L, false, null, "DefaultClient", null, 0L, "Financial", 0, 1L, null, "1" },
                    { 1001L, null, 0L, false, null, "DefaultSafe", null, 0L, "Financial", 0, 1L, null, "1" },
                    { 908L, null, 0L, false, null, "ServiceValue", null, 0L, "Order", 0, 2L, null, "" },
                    { 403L, null, 0L, false, null, "NumberLine", null, 0L, "Transaction", 0, 1L, null, "6" },
                    { 116L, null, 0L, false, null, "AutoCreateTransaction", null, 0L, "Invoice", 0, 2L, null, "0" },
                    { 401L, null, 0L, false, null, "DefaultStore", null, 0L, "Transaction", 0, 1L, null, "1" },
                    { 20L, null, 0L, false, null, "LengthQtyElectronicScale", null, 0L, "Invoice", 0, 1L, null, "5" },
                    { 101L, null, 0L, false, null, "DefaultStore", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 102L, null, 0L, false, null, "DefaultSupplier", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 103L, null, 0L, false, null, "DefaultPaymentType", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 104L, null, 0L, false, null, "DiscountValue", null, 0L, "Invoice", 0, 2L, null, "" },
                    { 105L, null, 0L, false, null, "DefaultDiscountType", null, 0L, "Invoice", 0, 2L, null, "2" },
                    { 106L, null, 0L, false, null, "ServiceValue", null, 0L, "Invoice", 0, 2L, null, "" },
                    { 107L, null, 0L, false, null, "DefaultServiceType", null, 0L, "Invoice", 0, 2L, null, "2" },
                    { 108L, null, 0L, false, null, "TaxValue", null, 0L, "Invoice", 0, 2L, null, "14" },
                    { 109L, null, 0L, false, null, "DefaultTaxType", null, 0L, "Invoice", 0, 2L, null, "2" },
                    { 110L, null, 0L, false, null, "NumberLine", null, 0L, "Invoice", 0, 2L, null, "6" },
                    { 111L, null, 0L, false, null, "OrderTabe", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 112L, null, 0L, false, null, "AutoSave", null, 0L, "Invoice", 0, 2L, null, "0" },
                    { 402L, null, 0L, false, null, "DefaultSupplier", null, 0L, "Transaction", 0, 1L, null, "1" },
                    { 114L, null, 0L, false, null, "AllowRepeated", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 19L, null, 0L, false, null, "LengthElectronicScale", null, 0L, "Invoice", 0, 1L, null, "7" },
                    { 115L, null, 0L, false, null, "SaveLastStatusSetting", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 18L, null, 0L, false, null, "CodeElectronicScale", null, 0L, "Invoice", 0, 1L, null, "009" },
                    { 16L, null, 0L, false, null, "AutoCreateTransaction", null, 0L, "Invoice", 0, 1L, null, "0" },
                    { 1L, null, 0L, false, null, "DefaultStore", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 2L, null, 0L, false, null, "DefaultCustomer", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 3L, null, 0L, false, null, "DefaultPaymentType", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 4L, null, 0L, false, null, "DiscountValue", null, 0L, "Invoice", 0, 1L, null, "" },
                    { 5L, null, 0L, false, null, "DefaultDiscountType", null, 0L, "Invoice", 0, 1L, null, "2" },
                    { 6L, null, 0L, false, null, "ServiceValue", null, 0L, "Invoice", 0, 1L, null, "" },
                    { 7L, null, 0L, false, null, "DefaultServiceType", null, 0L, "Invoice", 0, 1L, null, "2" }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "Preference",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 8L, null, 0L, false, null, "TaxValue", null, 0L, "Invoice", 0, 1L, null, "14" },
                    { 9L, null, 0L, false, null, "DefaultTaxType", null, 0L, "Invoice", 0, 1L, null, "2" },
                    { 10L, null, 0L, false, null, "NumberLine", null, 0L, "Invoice", 0, 1L, null, "6" },
                    { 11L, null, 0L, false, null, "OrderTabe", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 12L, null, 0L, false, null, "AutoSave", null, 0L, "Invoice", 0, 1L, null, "0" },
                    { 13L, null, 0L, false, null, "TypeSerial", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 14L, null, 0L, false, null, "AllowRepeated", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 15L, null, 0L, false, null, "SaveLastStatusSetting", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 17L, null, 0L, false, null, "DefaultCurrency", null, 0L, "Invoice", 0, 1L, null, "1" },
                    { 117L, null, 0L, false, null, "DefaultCurrency", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 113L, null, 0L, false, null, "TypeSerial", null, 0L, "Invoice", 0, 2L, null, "1" },
                    { 119L, null, 0L, false, null, "LengthElectronicScale", null, 0L, "Invoice", 0, 2L, null, "7" },
                    { 303L, null, 0L, false, null, "DefaultPaymentType", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 118L, null, 0L, false, null, "CodeElectronicScale", null, 0L, "Invoice", 0, 2L, null, "009" },
                    { 305L, null, 0L, false, null, "DefaultDiscountType", null, 0L, "Invoice", 0, 4L, null, "2" },
                    { 306L, null, 0L, false, null, "ServiceValue", null, 0L, "Invoice", 0, 4L, null, "" },
                    { 307L, null, 0L, false, null, "DefaultServiceType", null, 0L, "Invoice", 0, 4L, null, "2" },
                    { 308L, null, 0L, false, null, "TaxValue", null, 0L, "Invoice", 0, 4L, null, "14" },
                    { 309L, null, 0L, false, null, "DefaultTaxType", null, 0L, "Invoice", 0, 4L, null, "2" },
                    { 310L, null, 0L, false, null, "NumberLine", null, 0L, "Invoice", 0, 4L, null, "6" },
                    { 311L, null, 0L, false, null, "OrderTabe", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 312L, null, 0L, false, null, "AutoSave", null, 0L, "Invoice", 0, 4L, null, "0" },
                    { 313L, null, 0L, false, null, "TypeSerial", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 314L, null, 0L, false, null, "AllowRepeated", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 315L, null, 0L, false, null, "SaveLastStatusSetting", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 316L, null, 0L, false, null, "AutoCreateTransaction", null, 0L, "Invoice", 0, 4L, null, "0" },
                    { 317L, null, 0L, false, null, "DefaultCurrency", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 302L, null, 0L, false, null, "DefaultSupplier", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 301L, null, 0L, false, null, "DefaultStore", null, 0L, "Invoice", 0, 4L, null, "1" },
                    { 304L, null, 0L, false, null, "DiscountValue", null, 0L, "Invoice", 0, 4L, null, "" },
                    { 216L, null, 0L, false, null, "AutoCreateTransaction", null, 0L, "Invoice", 0, 3L, null, "0" },
                    { 120L, null, 0L, false, null, "LengthQtyElectronicScale", null, 0L, "Invoice", 0, 2L, null, "5" },
                    { 201L, null, 0L, false, null, "DefaultStore", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 202L, null, 0L, false, null, "DefaultCustomer", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 217L, null, 0L, false, null, "DefaultCurrency", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 204L, null, 0L, false, null, "DiscountValue", null, 0L, "Invoice", 0, 3L, null, "" },
                    { 205L, null, 0L, false, null, "DefaultDiscountType", null, 0L, "Invoice", 0, 3L, null, "2" },
                    { 206L, null, 0L, false, null, "ServiceValue", null, 0L, "Invoice", 0, 3L, null, "" },
                    { 207L, null, 0L, false, null, "DefaultServiceType", null, 0L, "Invoice", 0, 3L, null, "2" },
                    { 203L, null, 0L, false, null, "DefaultPaymentType", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 209L, null, 0L, false, null, "DefaultTaxType", null, 0L, "Invoice", 0, 3L, null, "2" },
                    { 210L, null, 0L, false, null, "NumberLine", null, 0L, "Invoice", 0, 3L, null, "6" }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "Preference",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 211L, null, 0L, false, null, "OrderTabe", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 212L, null, 0L, false, null, "AutoSave", null, 0L, "Invoice", 0, 3L, null, "0" },
                    { 213L, null, 0L, false, null, "TypeSerial", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 214L, null, 0L, false, null, "AllowRepeated", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 215L, null, 0L, false, null, "SaveLastStatusSetting", null, 0L, "Invoice", 0, 3L, null, "1" },
                    { 208L, null, 0L, false, null, "TaxValue", null, 0L, "Invoice", 0, 3L, null, "14" }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "Role",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 2L, null, 0L, false, null, null, "Admin", 0L, 0, 0L },
                    { 1L, null, 0L, true, null, null, "Owner", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "Safe",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[] { 1L, null, 0L, false, null, null, "Main Safe", 0L, 0, 0L });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "TransactionType",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "Icon", "ImgPath", "InOut", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 1L, null, 0L, false, "iconsminds-down-1", null, 1, null, "Addition", 0L, 0, 0L },
                    { 2L, null, 0L, false, "iconsminds-up-1", null, -1, null, "Issue", 0L, 0, 0L },
                    { 3L, null, 0L, false, "iconsminds-shuffle-1", null, -1, null, "Transafer", 0L, 0, 0L },
                    { 4L, null, 0L, false, "iconsminds-file-edit", null, 1, null, "Received", 0L, 0, 0L },
                    { 5L, null, 0L, false, "", null, 1, null, "Adjustment In", 0L, 0, 0L },
                    { 6L, null, 0L, false, "", null, -1, null, "Adjustment Out", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "RolePermission",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "ParentId", "PermissionId", "RoleId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 1L, null, 0L, false, null, null, 0L, 102L, 2L, 0, 0L },
                    { 2L, null, 0L, false, null, null, 0L, 10201L, 2L, 0, 0L },
                    { 3L, null, 0L, false, null, null, 0L, 1020101L, 2L, 0, 0L },
                    { 4L, null, 0L, false, null, null, 0L, 1020102L, 2L, 0, 0L },
                    { 5L, null, 0L, false, null, null, 0L, 1020103L, 2L, 0, 0L },
                    { 6L, null, 0L, false, null, null, 0L, 1020104L, 2L, 0, 0L },
                    { 7L, null, 0L, false, null, null, 0L, 10202L, 2L, 0, 0L },
                    { 8L, null, 0L, false, null, null, 0L, 1020201L, 2L, 0, 0L },
                    { 9L, null, 0L, false, null, null, 0L, 1020202L, 2L, 0, 0L },
                    { 10L, null, 0L, false, null, null, 0L, 1020203L, 2L, 0, 0L },
                    { 11L, null, 0L, false, null, null, 0L, 1020204L, 2L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "Store",
                columns: new[] { "Id", "BranchId", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[] { 1L, 1L, null, 0L, false, null, null, "Main Store", 0L, 0, 0L });

            migrationBuilder.InsertData(
                schema: _Schema,
                table: "User",
                columns: new[] { "Id", "BranchId", "Code", "CodeNumber", "Hide", "ImgPath", "LoginUserId", "MaskText", "Name", "ParentId", "Password", "RoleId", "Status", "TypeId", "UserName" },
                values: new object[,]
                {
                    { 1L, null, null, 0L, true, null, 1L, null, "Owner", 0L, "iebLM3YfOZ4fcXYL1jInxA==", 1L, 0, 0L, "Owner" },
                    { 2L, null, null, 0L, false, null, 2L, null, "Admin", 0L, "mGs8bPJNLmeH75qTfY9f9Q==", 2L, 0, 0L, "Admin" },
                    { 3L, null, null, 0L, false, null, 0L, null, "Emp", 0L, null, 2L, 0, 0L, "Admin2" }
                });

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
                name: "IX_Inventory_StoreId",
                schema: _Schema,
                table: "Inventory",
                column: "StoreId");

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
                name: "IX_Invoice_StoreId",
                schema: _Schema,
                table: "Invoice",
                column: "StoreId");

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
                name: "IX_InvoiceProduct_StoreId",
                schema: _Schema,
                table: "InvoiceProduct",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceProduct_UnitId",
                schema: _Schema,
                table: "InvoiceProduct",
                column: "UnitId");

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
                name: "IX_Store_BranchId",
                schema: _Schema,
                table: "Store",
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
                name: "IX_Transaction_StoreId",
                schema: _Schema,
                table: "Transaction",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_ToStoreId",
                schema: _Schema,
                table: "Transaction",
                column: "ToStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionProduct_ProductId",
                schema: _Schema,
                table: "TransactionProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionProduct_StoreId",
                schema: _Schema,
                table: "TransactionProduct",
                column: "StoreId");

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
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Branch_BranchId",
                schema: _Schema,
                table: "Invoice");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Branch_BranchId",
                schema: _Schema,
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Store_Branch_BranchId",
                schema: _Schema,
                table: "Store");

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
                name: "Financial",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Inventory",
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
                name: "Outlay",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Safe",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Property",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Classification",
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
                name: "Store",
                schema: _Schema);

            migrationBuilder.DropTable(
                name: "Table",
                schema: _Schema);
        }
    }
}