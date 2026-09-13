using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrgSys.DatabaseMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountIsPostableAndArConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPostable",
                table: "Account",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPostable",
                table: "Account");
        }
    }
}
