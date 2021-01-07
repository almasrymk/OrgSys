using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class DealerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MaskText",
                table: "Unit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Dealer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeNumber = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DealerType = table.Column<int>(type: "int", nullable: false),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dealer", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dealer");

            migrationBuilder.DropColumn(
                name: "MaskText",
                table: "Unit");
        }
    }
}
