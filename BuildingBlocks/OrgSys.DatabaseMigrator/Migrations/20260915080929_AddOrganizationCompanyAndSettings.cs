using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrgSys.DatabaseMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddOrganizationCompanyAndSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Hand-edited from the EF-generated scaffold (brief's "no destructive EF migration
            // without explicit data migration" rule — OrgConnection is a live remote database with
            // existing Branch rows; a NOT NULL CompanyId with defaultValue:0 would leave every
            // existing Branch pointing at a Company.Id that doesn't exist, and the AddForeignKey
            // below would fail outright). Sequence: create Company -> seed exactly one default
            // Company row -> add CompanyId as nullable -> backfill every existing Branch to that
            // Company -> tighten the column to NOT NULL -> only then add the FK constraint.
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LegalName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TradeName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    TaxRegistrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CommercialRegistrationNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DefaultCurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Company_Currency_DefaultCurrencyId",
                        column: x => x.DefaultCurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id");
                });

            // Exactly one default Company (Id = 1, guaranteed on a fresh IDENTITY table) so every
            // pre-existing Branch row has somewhere to point. Mirrors OrganizationDataSeeder's
            // InitialCompany, which only ever runs against a fresh database (the seeder's
            // idempotency check is "no Company rows exist yet" — this migration is what makes that
            // true be false on the live database, same relationship InitialBranch already had with
            // its own idempotency check).
            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "LegalName", "Code", "CodeNumber", "ParentId", "TypeId", "Hide", "Status" },
                values: new object[] { 1L, "Main Company", "MAIN", 1L, 0L, 0L, false, 0 });

            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "Branch",
                type: "bigint",
                nullable: true);

            migrationBuilder.Sql("UPDATE [Branch] SET [CompanyId] = 1 WHERE [CompanyId] IS NULL;");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                table: "Branch",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "OrganizationSettings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    DefaultCurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    DefaultCountryId = table.Column<long>(type: "bigint", nullable: true),
                    DefaultTimeZone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FiscalYearStartMonth = table.Column<int>(type: "int", nullable: true),
                    FiscalYearStartDay = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_OrganizationSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrganizationSettings_Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrganizationSettings_Country_DefaultCountryId",
                        column: x => x.DefaultCountryId,
                        principalTable: "Country",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrganizationSettings_Currency_DefaultCurrencyId",
                        column: x => x.DefaultCurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Branch_CompanyId",
                table: "Branch",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_CountryId",
                table: "Company",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Company_DefaultCurrencyId",
                table: "Company",
                column: "DefaultCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationSettings_CompanyId",
                table: "OrganizationSettings",
                column: "CompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationSettings_DefaultCountryId",
                table: "OrganizationSettings",
                column: "DefaultCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationSettings_DefaultCurrencyId",
                table: "OrganizationSettings",
                column: "DefaultCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Branch_Company_CompanyId",
                table: "Branch",
                column: "CompanyId",
                principalTable: "Company",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Branch_Company_CompanyId",
                table: "Branch");

            migrationBuilder.DropTable(
                name: "OrganizationSettings");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropIndex(
                name: "IX_Branch_CompanyId",
                table: "Branch");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Branch");
        }
    }
}
