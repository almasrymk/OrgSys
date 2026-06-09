using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations.OrgMigrations
{
    /// <inheritdoc />
    public partial class AddAccountToSafeTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "AccountId",
                schema: "org",
                table: "Safe",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Safe_AccountId",
                schema: "org",
                table: "Safe",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Safe_Account_AccountId",
                schema: "org",
                table: "Safe",
                column: "AccountId",
                principalSchema: "org",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Safe_Account_AccountId",
                schema: "org",
                table: "Safe");

            migrationBuilder.DropIndex(
                name: "IX_Safe_AccountId",
                schema: "org",
                table: "Safe");

            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "org",
                table: "Safe");
        }
    }
}
