using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class CreateFinancialTypeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PaymentTypeId",
                table: "Financial",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "FinancialType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InOut = table.Column<int>(type: "int", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "FinancialType",
                columns: new[] { "Id", "Hide", "Icon", "ImgPath", "InOut", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 1L, false, "iconsminds-financial", null, 1, null, "Collection", 0L, 0, 0L },
                    { 2L, false, "iconsminds-handshake", null, -1, null, "Payment", 0L, 0, 0L },
                    { 3L, false, "iconsminds-wallet", null, -1, null, "Outlay", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                table: "Safe",
                columns: new[] { "Id", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[] { 1L, false, null, null, "Main Safe", 0L, 0, 0L });

            migrationBuilder.CreateIndex(
                name: "IX_Financial_PaymentTypeId",
                table: "Financial",
                column: "PaymentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Financial_PaymentType_PaymentTypeId",
                table: "Financial",
                column: "PaymentTypeId",
                principalTable: "PaymentType",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Financial_PaymentType_PaymentTypeId",
                table: "Financial");

            migrationBuilder.DropTable(
                name: "FinancialType");

            migrationBuilder.DropIndex(
                name: "IX_Financial_PaymentTypeId",
                table: "Financial");

            migrationBuilder.DeleteData(
                table: "Safe",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DropColumn(
                name: "PaymentTypeId",
                table: "Financial");
        }
    }
}
