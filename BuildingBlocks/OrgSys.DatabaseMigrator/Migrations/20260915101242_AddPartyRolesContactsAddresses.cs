using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrgSys.DatabaseMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddPartyRolesContactsAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommercialRegistrationNumber",
                table: "Dealer",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PartyType",
                table: "Dealer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxRegistrationNumber",
                table: "Dealer",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CustomerProfile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerId = table.Column<long>(type: "bigint", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    CreditLimit = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsCreditAllowed = table.Column<bool>(type: "bit", nullable: false),
                    IsOnHold = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_CustomerProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerProfile_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerProfile_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartyAddress",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerId = table.Column<long>(type: "bigint", nullable: false),
                    AddressType = table.Column<int>(type: "int", nullable: false),
                    Line1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Line2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    DistrictId = table.Column<long>(type: "bigint", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_PartyAddress", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyAddress_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PartyAddress_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PartyAddress_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartyAddress_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "District",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PartyContact",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_PartyContact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartyContact_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierProfile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealerId = table.Column<long>(type: "bigint", nullable: false),
                    AccountId = table.Column<long>(type: "bigint", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    IsOnHold = table.Column<bool>(type: "bit", nullable: false),
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
                    table.PrimaryKey("PK_SupplierProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierProfile_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SupplierProfile_Dealer_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfile_AccountId",
                table: "CustomerProfile",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerProfile_DealerId",
                table: "CustomerProfile",
                column: "DealerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartyAddress_CityId",
                table: "PartyAddress",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyAddress_CountryId",
                table: "PartyAddress",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyAddress_DealerId",
                table: "PartyAddress",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyAddress_DistrictId",
                table: "PartyAddress",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_PartyContact_DealerId",
                table: "PartyContact",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierProfile_AccountId",
                table: "SupplierProfile",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierProfile_DealerId",
                table: "SupplierProfile",
                column: "DealerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerProfile");

            migrationBuilder.DropTable(
                name: "PartyAddress");

            migrationBuilder.DropTable(
                name: "PartyContact");

            migrationBuilder.DropTable(
                name: "SupplierProfile");

            migrationBuilder.DropColumn(
                name: "CommercialRegistrationNumber",
                table: "Dealer");

            migrationBuilder.DropColumn(
                name: "PartyType",
                table: "Dealer");

            migrationBuilder.DropColumn(
                name: "TaxRegistrationNumber",
                table: "Dealer");
        }
    }
}
