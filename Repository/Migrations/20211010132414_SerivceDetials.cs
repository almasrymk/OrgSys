using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class SerivceDetials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Service",
                table: "OrderProduct",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Service",
                table: "InvoiceProduct",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Service",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "Service",
                table: "InvoiceProduct");
        }
    }
}
