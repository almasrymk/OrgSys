using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations.AdminMigrations
{
    public partial class AddRequestId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RequestId",
                schema: "admin",
                table: "Client",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestId",
                schema: "admin",
                table: "Client");
        }
    }
}
