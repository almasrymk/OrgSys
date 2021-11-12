using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations.AdminMigrations
{
    public partial class AddCompanyName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                schema: "admin",
                table: "Client",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                schema: "admin",
                table: "Client");
        }
    }
}
