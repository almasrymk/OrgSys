using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class RemainingPaid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Paid",
                table: "Invoice",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Remaining",
                table: "Invoice",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Remaining",
                table: "Invoice");
        }
    }
}
