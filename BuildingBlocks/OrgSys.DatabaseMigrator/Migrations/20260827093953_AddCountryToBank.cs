using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrgSys.DatabaseMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryToBank : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CountryId",
                table: "Bank",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bank_CountryId",
                table: "Bank",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bank_Country_CountryId",
                table: "Bank",
                column: "CountryId",
                principalTable: "Country",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bank_Country_CountryId",
                table: "Bank");

            migrationBuilder.DropIndex(
                name: "IX_Bank_CountryId",
                table: "Bank");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Bank");
        }
    }
}
