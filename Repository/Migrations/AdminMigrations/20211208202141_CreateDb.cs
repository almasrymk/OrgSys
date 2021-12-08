using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations.AdminMigrations
{
    public partial class CreateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "admin");

            migrationBuilder.CreateTable(
                name: "GeneralClassification",
                schema: "admin",
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
                    table.PrimaryKey("PK_GeneralClassification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralCountry",
                schema: "admin",
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
                    table.PrimaryKey("PK_GeneralCountry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralProperty",
                schema: "admin",
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
                    table.PrimaryKey("PK_GeneralProperty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralUnit",
                schema: "admin",
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
                    table.PrimaryKey("PK_GeneralUnit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Nationality",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_Nationality", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanType",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_PlanType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Request",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_Request", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeActivity",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_TypeActivity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralProduct",
                schema: "admin",
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
                    GeneralClassificationId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_GeneralProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralProduct_GeneralClassification_GeneralClassificationId",
                        column: x => x.GeneralClassificationId,
                        principalSchema: "admin",
                        principalTable: "GeneralClassification",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "GeneralCity",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GeneralCountryId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_GeneralCity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralCity_GeneralCountry_GeneralCountryId",
                        column: x => x.GeneralCountryId,
                        principalSchema: "admin",
                        principalTable: "GeneralCountry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "GeneralPropertyElement",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralPropertyId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_GeneralPropertyElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralPropertyElement_GeneralProperty_GeneralPropertyId",
                        column: x => x.GeneralPropertyId,
                        principalSchema: "admin",
                        principalTable: "GeneralProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Plan",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Offer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceAfterOffer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanTypeId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Plan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plan_PlanType_PlanTypeId",
                        column: x => x.PlanTypeId,
                        principalSchema: "admin",
                        principalTable: "PlanType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    DbSchema = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeActivityId = table.Column<long>(type: "bigint", nullable: false),
                    NationalityId = table.Column<long>(type: "bigint", nullable: false),
                    SizeOfCompany = table.Column<long>(type: "bigint", nullable: false),
                    RequestId = table.Column<long>(type: "bigint", nullable: false),
                    VersionDb = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_Client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_Nationality_NationalityId",
                        column: x => x.NationalityId,
                        principalSchema: "admin",
                        principalTable: "Nationality",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Client_TypeActivity_TypeActivityId",
                        column: x => x.TypeActivityId,
                        principalSchema: "admin",
                        principalTable: "TypeActivity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "GeneralProductRecipe",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    RecipeId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GeneralProductId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_GeneralProductRecipe", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralProductRecipe_GeneralProduct_GeneralProductId",
                        column: x => x.GeneralProductId,
                        principalSchema: "admin",
                        principalTable: "GeneralProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "GeneralProductUnit",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralProductId = table.Column<long>(type: "bigint", nullable: false),
                    GeneralUnitId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_GeneralProductUnit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralProductUnit_GeneralProduct_GeneralProductId",
                        column: x => x.GeneralProductId,
                        principalSchema: "admin",
                        principalTable: "GeneralProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GeneralProductUnit_GeneralUnit_GeneralUnitId",
                        column: x => x.GeneralUnitId,
                        principalSchema: "admin",
                        principalTable: "GeneralUnit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "GeneralDistrict",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GeneralCountryId = table.Column<long>(type: "bigint", nullable: false),
                    GeneralCityId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_GeneralDistrict", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralDistrict_GeneralCity_GeneralCityId",
                        column: x => x.GeneralCityId,
                        principalSchema: "admin",
                        principalTable: "GeneralCity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GeneralDistrict_GeneralCountry_GeneralCountryId",
                        column: x => x.GeneralCountryId,
                        principalSchema: "admin",
                        principalTable: "GeneralCountry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "GeneralProductPropertyElement",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneralProductId = table.Column<long>(type: "bigint", nullable: true),
                    GeneralPropertyId = table.Column<long>(type: "bigint", nullable: true),
                    GeneralPropertyElementId = table.Column<long>(type: "bigint", nullable: true),
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
                    table.PrimaryKey("PK_GeneralProductPropertyElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneralProductPropertyElement_GeneralProduct_GeneralProductId",
                        column: x => x.GeneralProductId,
                        principalSchema: "admin",
                        principalTable: "GeneralProduct",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GeneralProductPropertyElement_GeneralProperty_GeneralPropertyId",
                        column: x => x.GeneralPropertyId,
                        principalSchema: "admin",
                        principalTable: "GeneralProperty",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_GeneralProductPropertyElement_GeneralPropertyElement_GeneralPropertyElementId",
                        column: x => x.GeneralPropertyElementId,
                        principalSchema: "admin",
                        principalTable: "GeneralPropertyElement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "PlanElement",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_PlanElement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanElement_Plan_PlanId",
                        column: x => x.PlanId,
                        principalSchema: "admin",
                        principalTable: "Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ClientPlan",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    PlanId = table.Column<long>(type: "bigint", nullable: false),
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
                    table.PrimaryKey("PK_ClientPlan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientPlan_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "admin",
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ClientPlan_Plan_PlanId",
                        column: x => x.PlanId,
                        principalSchema: "admin",
                        principalTable: "Plan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "LoginUser",
                schema: "admin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_LoginUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginUser_Client_ClientId",
                        column: x => x.ClientId,
                        principalSchema: "admin",
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "Nationality",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 1L, null, 0L, false, null, null, "Andorran", 0L, 0, 0L },
                    { 168L, null, 0L, false, null, null, "New Caledonian", 0L, 0, 0L },
                    { 169L, null, 0L, false, null, null, "Nigerian", 0L, 0, 0L },
                    { 170L, null, 0L, false, null, null, "Norfolk Islander", 0L, 0, 0L },
                    { 171L, null, 0L, false, null, null, "Nigerian", 0L, 0, 0L },
                    { 172L, null, 0L, false, null, null, "Nicaraguan", 0L, 0, 0L },
                    { 173L, null, 0L, false, null, null, "Dutch", 0L, 0, 0L },
                    { 174L, null, 0L, false, null, null, "Norwegian", 0L, 0, 0L },
                    { 175L, null, 0L, false, null, null, "Nepalese", 0L, 0, 0L },
                    { 176L, null, 0L, false, null, null, "Dronning Maud Land", 0L, 0, 0L },
                    { 177L, null, 0L, false, null, null, "Nauruan", 0L, 0, 0L },
                    { 178L, null, 0L, false, null, null, "Neutral Zone", 0L, 0, 0L },
                    { 179L, null, 0L, false, null, null, "Niuean", 0L, 0, 0L },
                    { 180L, null, 0L, false, null, null, "New Zealander", 0L, 0, 0L },
                    { 181L, null, 0L, false, null, null, "Omani", 0L, 0, 0L },
                    { 182L, null, 0L, false, null, null, "Panamanian", 0L, 0, 0L },
                    { 183L, null, 0L, false, null, null, "Pacific Islands Trust Territory", 0L, 0, 0L },
                    { 184L, null, 0L, false, null, null, "Peruvian", 0L, 0, 0L },
                    { 185L, null, 0L, false, null, null, "French Polynesian", 0L, 0, 0L },
                    { 186L, null, 0L, false, null, null, "Papua New Guinean", 0L, 0, 0L },
                    { 187L, null, 0L, false, null, null, "Filipino", 0L, 0, 0L },
                    { 188L, null, 0L, false, null, null, "Pakistani", 0L, 0, 0L },
                    { 189L, null, 0L, false, null, null, "Polish", 0L, 0, 0L },
                    { 190L, null, 0L, false, null, null, "French", 0L, 0, 0L },
                    { 191L, null, 0L, false, null, null, "Pitcairn Islander", 0L, 0, 0L },
                    { 192L, null, 0L, false, null, null, "Puerto Rican", 0L, 0, 0L },
                    { 193L, null, 0L, false, null, null, "Palestinian", 0L, 0, 0L },
                    { 194L, null, 0L, false, null, null, "Portuguese", 0L, 0, 0L },
                    { 195L, null, 0L, false, null, null, "U.S. Miscellaneous Pacific Islands", 0L, 0, 0L },
                    { 196L, null, 0L, false, null, null, "Palauan", 0L, 0, 0L },
                    { 167L, null, 0L, false, null, null, "Namibian", 0L, 0, 0L },
                    { 197L, null, 0L, false, null, null, "Paraguayan", 0L, 0, 0L },
                    { 166L, null, 0L, false, null, null, "Mozambican", 0L, 0, 0L },
                    { 164L, null, 0L, false, null, null, "Mexican", 0L, 0, 0L },
                    { 135L, null, 0L, false, null, null, "Liechtensteiner", 0L, 0, 0L },
                    { 136L, null, 0L, false, null, null, "Sri Lankan", 0L, 0, 0L },
                    { 137L, null, 0L, false, null, null, "Liberian", 0L, 0, 0L },
                    { 138L, null, 0L, false, null, null, "Mosotho", 0L, 0, 0L },
                    { 139L, null, 0L, false, null, null, "Lithuanian", 0L, 0, 0L },
                    { 140L, null, 0L, false, null, null, "Luxembourger", 0L, 0, 0L },
                    { 141L, null, 0L, false, null, null, "Latvian", 0L, 0, 0L },
                    { 142L, null, 0L, false, null, null, "Libyan", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "Nationality",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 143L, null, 0L, false, null, null, "Moroccan", 0L, 0, 0L },
                    { 144L, null, 0L, false, null, null, "Monegasque", 0L, 0, 0L },
                    { 145L, null, 0L, false, null, null, "Moldovan", 0L, 0, 0L },
                    { 146L, null, 0L, false, null, null, "Montenegrin", 0L, 0, 0L },
                    { 147L, null, 0L, false, null, null, "Saint Martin Islander", 0L, 0, 0L },
                    { 148L, null, 0L, false, null, null, "Malagasy", 0L, 0, 0L },
                    { 149L, null, 0L, false, null, null, "Marshallese", 0L, 0, 0L },
                    { 150L, null, 0L, false, null, null, "Midway Islands", 0L, 0, 0L },
                    { 151L, null, 0L, false, null, null, "Macedonian", 0L, 0, 0L },
                    { 152L, null, 0L, false, null, null, "Malian", 0L, 0, 0L },
                    { 153L, null, 0L, false, null, null, "Myanmar", 0L, 0, 0L },
                    { 154L, null, 0L, false, null, null, "Mongolian", 0L, 0, 0L },
                    { 155L, null, 0L, false, null, null, "Chinese", 0L, 0, 0L },
                    { 156L, null, 0L, false, null, null, "American", 0L, 0, 0L },
                    { 157L, null, 0L, false, null, null, "French", 0L, 0, 0L },
                    { 158L, null, 0L, false, null, null, "Mauritanian", 0L, 0, 0L },
                    { 159L, null, 0L, false, null, null, "Montserratian", 0L, 0, 0L },
                    { 160L, null, 0L, false, null, null, "Maltese", 0L, 0, 0L },
                    { 161L, null, 0L, false, null, null, "Mauritian", 0L, 0, 0L },
                    { 162L, null, 0L, false, null, null, "Maldivan", 0L, 0, 0L },
                    { 163L, null, 0L, false, null, null, "Malawian", 0L, 0, 0L },
                    { 165L, null, 0L, false, null, null, "Malaysian", 0L, 0, 0L },
                    { 134L, null, 0L, false, null, null, "Saint Lucian", 0L, 0, 0L },
                    { 198L, null, 0L, false, null, null, "Panama Canal Zone", 0L, 0, 0L },
                    { 200L, null, 0L, false, null, null, "French", 0L, 0, 0L },
                    { 234L, null, 0L, false, null, null, "Tunisian", 0L, 0, 0L },
                    { 235L, null, 0L, false, null, null, "Tongan", 0L, 0, 0L },
                    { 236L, null, 0L, false, null, null, "Tongan", 0L, 0, 0L },
                    { 237L, null, 0L, false, null, null, "Trinidadian", 0L, 0, 0L },
                    { 238L, null, 0L, false, null, null, "Tuvaluan", 0L, 0, 0L },
                    { 239L, null, 0L, false, null, null, "Taiwanese", 0L, 0, 0L },
                    { 240L, null, 0L, false, null, null, "Tanzanian", 0L, 0, 0L },
                    { 241L, null, 0L, false, null, null, "Ukrainian", 0L, 0, 0L },
                    { 242L, null, 0L, false, null, null, "Ugandan", 0L, 0, 0L },
                    { 243L, null, 0L, false, null, null, "American", 0L, 0, 0L },
                    { 244L, null, 0L, false, null, null, "American", 0L, 0, 0L },
                    { 245L, null, 0L, false, null, null, "Uruguayan", 0L, 0, 0L },
                    { 246L, null, 0L, false, null, null, "Uzbekistani", 0L, 0, 0L },
                    { 247L, null, 0L, false, null, null, "Italian", 0L, 0, 0L },
                    { 248L, null, 0L, false, null, null, "Saint Vincentian", 0L, 0, 0L },
                    { 249L, null, 0L, false, null, null, "North Vietnam", 0L, 0, 0L },
                    { 250L, null, 0L, false, null, null, "Venezuelan", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "Nationality",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 251L, null, 0L, false, null, null, "Virgin Islander", 0L, 0, 0L },
                    { 252L, null, 0L, false, null, null, "Virgin Islander", 0L, 0, 0L },
                    { 253L, null, 0L, false, null, null, "Vietnamese", 0L, 0, 0L },
                    { 254L, null, 0L, false, null, null, "Ni-Vanuatu", 0L, 0, 0L },
                    { 255L, null, 0L, false, null, null, "Wallis and Futuna Islander", 0L, 0, 0L },
                    { 256L, null, 0L, false, null, null, "Wake Island", 0L, 0, 0L },
                    { 257L, null, 0L, false, null, null, "Samoan", 0L, 0, 0L },
                    { 258L, null, 0L, false, null, null, "Yemeni", 0L, 0, 0L },
                    { 259L, null, 0L, false, null, null, "French", 0L, 0, 0L },
                    { 260L, null, 0L, false, null, null, "South African", 0L, 0, 0L },
                    { 261L, null, 0L, false, null, null, "Zambian", 0L, 0, 0L },
                    { 262L, null, 0L, false, null, null, "Zimbabwean", 0L, 0, 0L },
                    { 233L, null, 0L, false, null, null, "Turkmen", 0L, 0, 0L },
                    { 199L, null, 0L, false, null, null, "Qatari", 0L, 0, 0L },
                    { 232L, null, 0L, false, null, null, "East Timorese", 0L, 0, 0L },
                    { 230L, null, 0L, false, null, null, "Tadzhik", 0L, 0, 0L },
                    { 201L, null, 0L, false, null, null, "Romanian", 0L, 0, 0L },
                    { 202L, null, 0L, false, null, null, "Serbian", 0L, 0, 0L },
                    { 203L, null, 0L, false, null, null, "Russian", 0L, 0, 0L },
                    { 204L, null, 0L, false, null, null, "Rwandan", 0L, 0, 0L },
                    { 205L, null, 0L, false, null, null, "Saudi Arabian", 0L, 0, 0L },
                    { 206L, null, 0L, false, null, null, "Solomon Islander", 0L, 0, 0L },
                    { 207L, null, 0L, false, null, null, "Seychellois", 0L, 0, 0L },
                    { 208L, null, 0L, false, null, null, "Sudanese", 0L, 0, 0L },
                    { 209L, null, 0L, false, null, null, "Swedish", 0L, 0, 0L },
                    { 210L, null, 0L, false, null, null, "Singaporean", 0L, 0, 0L },
                    { 211L, null, 0L, false, null, null, "Saint Helenian", 0L, 0, 0L },
                    { 212L, null, 0L, false, null, null, "Slovene", 0L, 0, 0L },
                    { 213L, null, 0L, false, null, null, "Norwegian", 0L, 0, 0L },
                    { 214L, null, 0L, false, null, null, "Slovak", 0L, 0, 0L },
                    { 215L, null, 0L, false, null, null, "Sierra Leonean", 0L, 0, 0L },
                    { 216L, null, 0L, false, null, null, "Sammarinese", 0L, 0, 0L },
                    { 217L, null, 0L, false, null, null, "Senegalese", 0L, 0, 0L },
                    { 218L, null, 0L, false, null, null, "Somali", 0L, 0, 0L },
                    { 219L, null, 0L, false, null, null, "Surinamer", 0L, 0, 0L },
                    { 220L, null, 0L, false, null, null, "Sao Tomean", 0L, 0, 0L },
                    { 221L, null, 0L, false, null, null, "Union of Soviet Socialist Republics", 0L, 0, 0L },
                    { 222L, null, 0L, false, null, null, "Salvadoran", 0L, 0, 0L },
                    { 223L, null, 0L, false, null, null, "Syrian", 0L, 0, 0L },
                    { 224L, null, 0L, false, null, null, "Swazi", 0L, 0, 0L },
                    { 225L, null, 0L, false, null, null, "Turks and Caicos Islander", 0L, 0, 0L },
                    { 226L, null, 0L, false, null, null, "Chadian", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "Nationality",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 227L, null, 0L, false, null, null, "French", 0L, 0, 0L },
                    { 228L, null, 0L, false, null, null, "Togolese", 0L, 0, 0L },
                    { 229L, null, 0L, false, null, null, "Thai", 0L, 0, 0L },
                    { 231L, null, 0L, false, null, null, "Tokelauan", 0L, 0, 0L },
                    { 132L, null, 0L, false, null, null, "Laotian", 0L, 0, 0L },
                    { 133L, null, 0L, false, null, null, "Lebanese", 0L, 0, 0L },
                    { 65L, null, 0L, false, null, null, "Algerian", 0L, 0, 0L },
                    { 35L, null, 0L, false, null, null, "Bouvet Island", 0L, 0, 0L },
                    { 36L, null, 0L, false, null, null, "Motswana", 0L, 0, 0L },
                    { 37L, null, 0L, false, null, null, "Belarusian", 0L, 0, 0L },
                    { 38L, null, 0L, false, null, null, "Belizean", 0L, 0, 0L },
                    { 39L, null, 0L, false, null, null, "Canadian", 0L, 0, 0L },
                    { 40L, null, 0L, false, null, null, "Cocos Islander", 0L, 0, 0L },
                    { 41L, null, 0L, false, null, null, "Congolese", 0L, 0, 0L },
                    { 42L, null, 0L, false, null, null, "Central African", 0L, 0, 0L },
                    { 43L, null, 0L, false, null, null, "Congolese", 0L, 0, 0L },
                    { 44L, null, 0L, false, null, null, "Swiss", 0L, 0, 0L },
                    { 45L, null, 0L, false, null, null, "Ivorian", 0L, 0, 0L },
                    { 46L, null, 0L, false, null, null, "Cook Islander", 0L, 0, 0L },
                    { 47L, null, 0L, false, null, null, "Chilean", 0L, 0, 0L },
                    { 48L, null, 0L, false, null, null, "Cameroonian", 0L, 0, 0L },
                    { 49L, null, 0L, false, null, null, "Chinese", 0L, 0, 0L },
                    { 50L, null, 0L, false, null, null, "Colombian", 0L, 0, 0L },
                    { 51L, null, 0L, false, null, null, "Costa Rican", 0L, 0, 0L },
                    { 52L, null, 0L, false, null, null, "Montenegrins, Serbs", 0L, 0, 0L },
                    { 53L, null, 0L, false, null, null, "Canton and Enderbury Islands", 0L, 0, 0L },
                    { 54L, null, 0L, false, null, null, "Cuban", 0L, 0, 0L },
                    { 55L, null, 0L, false, null, null, "Cape Verdian", 0L, 0, 0L },
                    { 56L, null, 0L, false, null, null, "Curaçaoan", 0L, 0, 0L },
                    { 57L, null, 0L, false, null, null, "Christmas Island", 0L, 0, 0L },
                    { 58L, null, 0L, false, null, null, "Cypriot", 0L, 0, 0L },
                    { 59L, null, 0L, false, null, null, "Czech", 0L, 0, 0L },
                    { 60L, null, 0L, false, null, null, "German", 0L, 0, 0L },
                    { 61L, null, 0L, false, null, null, "Djibouti", 0L, 0, 0L },
                    { 62L, null, 0L, false, null, null, "Danish", 0L, 0, 0L },
                    { 63L, null, 0L, false, null, null, "Dominican", 0L, 0, 0L },
                    { 34L, null, 0L, false, null, null, "Bhutanese", 0L, 0, 0L },
                    { 64L, null, 0L, false, null, null, "Dominican", 0L, 0, 0L },
                    { 33L, null, 0L, false, null, null, "Bahamian", 0L, 0, 0L },
                    { 31L, null, 0L, false, null, null, "Dutch", 0L, 0, 0L },
                    { 2L, null, 0L, false, null, null, "Emirati", 0L, 0, 0L },
                    { 3L, null, 0L, false, null, null, "Afghan", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "Nationality",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 4L, null, 0L, false, null, null, "Antiguan, Barbudan", 0L, 0, 0L },
                    { 5L, null, 0L, false, null, null, "Anguillian", 0L, 0, 0L },
                    { 6L, null, 0L, false, null, null, "Albanian", 0L, 0, 0L },
                    { 7L, null, 0L, false, null, null, "Armenian", 0L, 0, 0L },
                    { 8L, null, 0L, false, null, null, "Dutch", 0L, 0, 0L },
                    { 9L, null, 0L, false, null, null, "Angolan", 0L, 0, 0L },
                    { 10L, null, 0L, false, null, null, "Antarctican", 0L, 0, 0L },
                    { 11L, null, 0L, false, null, null, "Argentinean", 0L, 0, 0L },
                    { 12L, null, 0L, false, null, null, "American Samoan", 0L, 0, 0L },
                    { 13L, null, 0L, false, null, null, "Austrian", 0L, 0, 0L },
                    { 14L, null, 0L, false, null, null, "Australian", 0L, 0, 0L },
                    { 15L, null, 0L, false, null, null, "Aruban", 0L, 0, 0L },
                    { 16L, null, 0L, false, null, null, "Swedish", 0L, 0, 0L },
                    { 17L, null, 0L, false, null, null, "Azerbaijani", 0L, 0, 0L },
                    { 18L, null, 0L, false, null, null, "Bosnian, Herzegovinian", 0L, 0, 0L },
                    { 19L, null, 0L, false, null, null, "Barbadian", 0L, 0, 0L },
                    { 20L, null, 0L, false, null, null, "Bangladeshi", 0L, 0, 0L },
                    { 21L, null, 0L, false, null, null, "Belgian", 0L, 0, 0L },
                    { 22L, null, 0L, false, null, null, "Burkinabe", 0L, 0, 0L },
                    { 23L, null, 0L, false, null, null, "Bulgarian", 0L, 0, 0L },
                    { 24L, null, 0L, false, null, null, "Bahraini", 0L, 0, 0L },
                    { 25L, null, 0L, false, null, null, "Burundian", 0L, 0, 0L },
                    { 26L, null, 0L, false, null, null, "Beninese", 0L, 0, 0L },
                    { 27L, null, 0L, false, null, null, "Saint Barthélemy Islander", 0L, 0, 0L },
                    { 28L, null, 0L, false, null, null, "Bermudian", 0L, 0, 0L },
                    { 29L, null, 0L, false, null, null, "Bruneian", 0L, 0, 0L },
                    { 30L, null, 0L, false, null, null, "Bolivian", 0L, 0, 0L },
                    { 32L, null, 0L, false, null, null, "Brazilian", 0L, 0, 0L },
                    { 131L, null, 0L, false, null, null, "Kazakhstani", 0L, 0, 0L },
                    { 66L, null, 0L, false, null, null, "Ecuadorean", 0L, 0, 0L },
                    { 67L, null, 0L, false, null, null, "Estonian", 0L, 0, 0L },
                    { 102L, null, 0L, false, null, null, "Honduran", 0L, 0, 0L },
                    { 103L, null, 0L, false, null, null, "Croatian", 0L, 0, 0L },
                    { 104L, null, 0L, false, null, null, "Haitian", 0L, 0, 0L },
                    { 105L, null, 0L, false, null, null, "Hungarian", 0L, 0, 0L },
                    { 106L, null, 0L, false, null, null, "Indonesian", 0L, 0, 0L },
                    { 107L, null, 0L, false, null, null, "Irish", 0L, 0, 0L },
                    { 108L, null, 0L, false, null, null, "Israeli", 0L, 0, 0L },
                    { 109L, null, 0L, false, null, null, "Manx", 0L, 0, 0L },
                    { 110L, null, 0L, false, null, null, "Indian", 0L, 0, 0L },
                    { 111L, null, 0L, false, null, null, "Indian", 0L, 0, 0L },
                    { 112L, null, 0L, false, null, null, "Iraqi", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "Nationality",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 113L, null, 0L, false, null, null, "Iranian", 0L, 0, 0L },
                    { 114L, null, 0L, false, null, null, "Icelander", 0L, 0, 0L },
                    { 115L, null, 0L, false, null, null, "Italian", 0L, 0, 0L },
                    { 116L, null, 0L, false, null, null, "Channel Islander", 0L, 0, 0L },
                    { 117L, null, 0L, false, null, null, "Jamaican", 0L, 0, 0L },
                    { 118L, null, 0L, false, null, null, "Jordanian", 0L, 0, 0L },
                    { 119L, null, 0L, false, null, null, "Japanese", 0L, 0, 0L },
                    { 120L, null, 0L, false, null, null, "Johnston Island", 0L, 0, 0L },
                    { 121L, null, 0L, false, null, null, "Kenyan", 0L, 0, 0L },
                    { 122L, null, 0L, false, null, null, "Kirghiz", 0L, 0, 0L },
                    { 123L, null, 0L, false, null, null, "Cambodian", 0L, 0, 0L },
                    { 124L, null, 0L, false, null, null, "I-Kiribati", 0L, 0, 0L },
                    { 125L, null, 0L, false, null, null, "Comoran", 0L, 0, 0L },
                    { 126L, null, 0L, false, null, null, "Kittian and Nevisian", 0L, 0, 0L },
                    { 127L, null, 0L, false, null, null, "North Korean", 0L, 0, 0L },
                    { 128L, null, 0L, false, null, null, "South Korean", 0L, 0, 0L },
                    { 129L, null, 0L, false, null, null, "Kuwaiti", 0L, 0, 0L },
                    { 130L, null, 0L, false, null, null, "Caymanian", 0L, 0, 0L },
                    { 101L, null, 0L, false, null, null, "Heard and McDonald Islander", 0L, 0, 0L },
                    { 100L, null, 0L, false, null, null, "Chinese", 0L, 0, 0L },
                    { 99L, null, 0L, false, null, null, "Guyanese", 0L, 0, 0L },
                    { 98L, null, 0L, false, null, null, "Guinea-Bissauan", 0L, 0, 0L },
                    { 68L, null, 0L, false, null, null, "Egyptian", 0L, 0, 0L },
                    { 69L, null, 0L, false, null, null, "Sahrawi", 0L, 0, 0L },
                    { 70L, null, 0L, false, null, null, "Eritrean", 0L, 0, 0L },
                    { 71L, null, 0L, false, null, null, "Spanish", 0L, 0, 0L },
                    { 72L, null, 0L, false, null, null, "Ethiopian", 0L, 0, 0L },
                    { 73L, null, 0L, false, null, null, "Finnish", 0L, 0, 0L },
                    { 74L, null, 0L, false, null, null, "Fijian", 0L, 0, 0L },
                    { 75L, null, 0L, false, null, null, "Falkland Islander", 0L, 0, 0L },
                    { 76L, null, 0L, false, null, null, "Micronesian", 0L, 0, 0L },
                    { 77L, null, 0L, false, null, null, "Faroese", 0L, 0, 0L },
                    { 78L, null, 0L, false, null, null, "French Southern and Antarctic Territories", 0L, 0, 0L },
                    { 79L, null, 0L, false, null, null, "French", 0L, 0, 0L },
                    { 80L, null, 0L, false, null, null, "Metropolitan France", 0L, 0, 0L },
                    { 81L, null, 0L, false, null, null, "Gabonese", 0L, 0, 0L },
                    { 82L, null, 0L, false, null, null, "British", 0L, 0, 0L },
                    { 91L, null, 0L, false, null, null, "Guinean", 0L, 0, 0L },
                    { 84L, null, 0L, false, null, null, "Georgian", 0L, 0, 0L },
                    { 85L, null, 0L, false, null, null, "French Guiana", 0L, 0, 0L },
                    { 86L, null, 0L, false, null, null, "Channel Islander", 0L, 0, 0L },
                    { 87L, null, 0L, false, null, null, "Ghanaian", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "Nationality",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 88L, null, 0L, false, null, null, "Gibraltar", 0L, 0, 0L },
                    { 89L, null, 0L, false, null, null, "Greenlandic", 0L, 0, 0L },
                    { 90L, null, 0L, false, null, null, "Gambian", 0L, 0, 0L },
                    { 83L, null, 0L, false, null, null, "Grenadian", 0L, 0, 0L },
                    { 92L, null, 0L, false, null, null, "Guadeloupian", 0L, 0, 0L },
                    { 93L, null, 0L, false, null, null, "Equatorial Guinean", 0L, 0, 0L },
                    { 94L, null, 0L, false, null, null, "Greek", 0L, 0, 0L },
                    { 95L, null, 0L, false, null, null, "South Georgia and the South Sandwich Islander", 0L, 0, 0L },
                    { 96L, null, 0L, false, null, null, "Guatemalan", 0L, 0, 0L },
                    { 97L, null, 0L, false, null, null, "Guamanian", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "PlanType",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 1L, null, 0L, false, null, null, "Always", 0L, 0, 0L },
                    { 2L, null, 0L, false, null, null, "Limited in time", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "Request",
                columns: new[] { "Id", "Code", "CodeNumber", "CompanyName", "Email", "ExpireDate", "Hide", "ImgPath", "Key", "MaskText", "Name", "ParentId", "Phone", "Status", "TypeId", "URL" },
                values: new object[] { 1L, "1", 1L, "org", "info@org.com", new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, null, "Mohammed Khaled", 0L, "0201111105784", 0, 0L, "" });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "TypeActivity",
                columns: new[] { "Id", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[] { 1L, null, 0L, false, null, null, "Supermarket", 0L, 0, 0L });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "Client",
                columns: new[] { "Id", "Code", "CodeNumber", "CompanyName", "DbSchema", "Description", "Email", "Fax", "Hide", "ImgPath", "MaskText", "Mobile", "Name", "NationalityId", "ParentId", "Phone", "RequestId", "SizeOfCompany", "Status", "TypeActivityId", "TypeId", "VersionDb" },
                values: new object[] { 1L, "1", 1L, null, "org", null, "info@org.com", null, false, null, null, "0201111105784", "Org", 68L, 0L, "0201111105784", 0L, 1L, 0, 1L, 0L, 1L });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "Plan",
                columns: new[] { "Id", "Code", "CodeNumber", "Description", "EndDate", "Hide", "ImgPath", "MaskText", "Name", "Offer", "ParentId", "PlanTypeId", "Price", "PriceAfterOffer", "StartDate", "Status", "TypeId" },
                values: new object[] { 2L, "1", 1L, "Two-week trial", null, false, null, null, "Basic", "First year discount offer 50%", 0L, 1L, "400$", "200$", null, 0, 0L });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "Plan",
                columns: new[] { "Id", "Code", "CodeNumber", "Description", "EndDate", "Hide", "ImgPath", "MaskText", "Name", "Offer", "ParentId", "PlanTypeId", "Price", "PriceAfterOffer", "StartDate", "Status", "TypeId" },
                values: new object[] { 1L, "1", 1L, "Two-week trial", null, false, null, null, "Trial", "0", 0L, 1L, "5$", "5$", null, 0, 0L });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "ClientPlan",
                columns: new[] { "Id", "ClientId", "Code", "CodeNumber", "EndDate", "Hide", "ImgPath", "MaskText", "ParentId", "PlanId", "StartDate", "Status", "TypeId" },
                values: new object[] { 1L, 1L, "1", 1L, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, 0L, 1L, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, 0L });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "LoginUser",
                columns: new[] { "Id", "ClientId", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "ParentId", "Password", "Status", "TypeId", "UserName" },
                values: new object[] { 1L, 1L, "1", 1L, false, null, null, 0L, "8wxxrb+Qv++WfvIH95KL1g==", 0, 0L, "Owner" });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "LoginUser",
                columns: new[] { "Id", "ClientId", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "ParentId", "Password", "Status", "TypeId", "UserName" },
                values: new object[] { 2L, 1L, "1", 1L, false, null, null, 0L, "8wxxrb+Qv++WfvIH95KL1g==", 0, 0L, "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Client_NationalityId",
                schema: "admin",
                table: "Client",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Client_TypeActivityId",
                schema: "admin",
                table: "Client",
                column: "TypeActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPlan_ClientId",
                schema: "admin",
                table: "ClientPlan",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientPlan_PlanId",
                schema: "admin",
                table: "ClientPlan",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralCity_GeneralCountryId",
                schema: "admin",
                table: "GeneralCity",
                column: "GeneralCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralDistrict_GeneralCityId",
                schema: "admin",
                table: "GeneralDistrict",
                column: "GeneralCityId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralDistrict_GeneralCountryId",
                schema: "admin",
                table: "GeneralDistrict",
                column: "GeneralCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralProduct_GeneralClassificationId",
                schema: "admin",
                table: "GeneralProduct",
                column: "GeneralClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralProductPropertyElement_GeneralProductId",
                schema: "admin",
                table: "GeneralProductPropertyElement",
                column: "GeneralProductId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralProductPropertyElement_GeneralPropertyElementId",
                schema: "admin",
                table: "GeneralProductPropertyElement",
                column: "GeneralPropertyElementId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralProductPropertyElement_GeneralPropertyId",
                schema: "admin",
                table: "GeneralProductPropertyElement",
                column: "GeneralPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralProductRecipe_GeneralProductId",
                schema: "admin",
                table: "GeneralProductRecipe",
                column: "GeneralProductId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralProductUnit_GeneralProductId",
                schema: "admin",
                table: "GeneralProductUnit",
                column: "GeneralProductId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralProductUnit_GeneralUnitId",
                schema: "admin",
                table: "GeneralProductUnit",
                column: "GeneralUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_GeneralPropertyElement_GeneralPropertyId",
                schema: "admin",
                table: "GeneralPropertyElement",
                column: "GeneralPropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginUser_ClientId",
                schema: "admin",
                table: "LoginUser",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Plan_PlanTypeId",
                schema: "admin",
                table: "Plan",
                column: "PlanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanElement_PlanId",
                schema: "admin",
                table: "PlanElement",
                column: "PlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientPlan",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "GeneralDistrict",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "GeneralProductPropertyElement",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "GeneralProductRecipe",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "GeneralProductUnit",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "LoginUser",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "PlanElement",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "Request",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "GeneralCity",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "GeneralPropertyElement",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "GeneralProduct",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "GeneralUnit",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "Client",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "Plan",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "GeneralCountry",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "GeneralProperty",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "GeneralClassification",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "Nationality",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "TypeActivity",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "PlanType",
                schema: "admin");
        }
    }
}
