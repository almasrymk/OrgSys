using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class AddNetByDefaultCurrencyAndRate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Credit",
                table: "Invoice",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CreditByDefaultCurrency",
                table: "Invoice",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "NetByDefaultCurrency",
                table: "Invoice",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "Invoice",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountByDefaultCurrency",
                table: "Financial",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "Financial",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credit",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "CreditByDefaultCurrency",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "NetByDefaultCurrency",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "AmountByDefaultCurrency",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Financial");
        }
    }
}
