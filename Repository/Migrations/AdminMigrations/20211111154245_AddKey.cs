using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations.AdminMigrations
{
    public partial class AddKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                schema: "admin",
                table: "Request",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                schema: "admin",
                table: "Request");
        }
    }
}
