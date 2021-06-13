using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class CreateOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrderId",
                table: "OrderProduct",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "TableId",
                table: "Order",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "OrderTypeId",
                table: "Order",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "Service",
                table: "Order",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ServiceType",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaxType",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "OrderType",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
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
                    table.PrimaryKey("PK_OrderType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Table",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfPeople = table.Column<int>(type: "int", nullable: false),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Table", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "OrderType",
                columns: new[] { "Id", "Hide", "Icon", "ImgPath", "MaskText", "Name", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 1L, false, "simple-icon-basket-loaded", null, null, "Internal", 0L, 0, 0L },
                    { 2L, false, "simple-icon-basket-loaded", null, null, "External", 0L, 0, 0L }
                });

            migrationBuilder.InsertData(
                table: "Preference",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Reference", "Status", "TypeId", "UserId", "Value" },
                values: new object[,]
                {
                    { 91L, false, null, "NumberLine", null, 0L, "Order", 0, 1L, null, "6" },
                    { 92L, false, null, "OrderTabe", null, 0L, "Order", 0, 1L, null, "2" },
                    { 93L, false, null, "AutoSave", null, 0L, "Order", 0, 1L, null, "0" },
                    { 94L, false, null, "TypeSerial", null, 0L, "Order", 0, 1L, null, "1" },
                    { 95L, false, null, "AllowRepeated", null, 0L, "Order", 0, 1L, null, "1" },
                    { 96L, false, null, "NumberLine", null, 0L, "Order", 0, 2L, null, "6" },
                    { 97L, false, null, "OrderTabe", null, 0L, "Order", 0, 2L, null, "2" },
                    { 98L, false, null, "AutoSave", null, 0L, "Order", 0, 2L, null, "0" },
                    { 99L, false, null, "TypeSerial", null, 0L, "Order", 0, 2L, null, "1" },
                    { 100L, false, null, "AllowRepeated", null, 0L, "Order", 0, 2L, null, "1" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_OrderId",
                table: "OrderProduct",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderTypeId",
                table: "Order",
                column: "OrderTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_TableId",
                table: "Order",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_OrderType_OrderTypeId",
                table: "Order",
                column: "OrderTypeId",
                principalTable: "OrderType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Table_TableId",
                table: "Order",
                column: "TableId",
                principalTable: "Table",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduct_Order_OrderId",
                table: "OrderProduct",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_OrderType_OrderTypeId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Table_TableId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduct_Order_OrderId",
                table: "OrderProduct");

            migrationBuilder.DropTable(
                name: "OrderType");

            migrationBuilder.DropTable(
                name: "Table");

            migrationBuilder.DropIndex(
                name: "IX_OrderProduct_OrderId",
                table: "OrderProduct");

            migrationBuilder.DropIndex(
                name: "IX_Order_OrderTypeId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_TableId",
                table: "Order");

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 91L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 92L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 93L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 94L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 95L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 96L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 97L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 98L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 99L);

            migrationBuilder.DeleteData(
                table: "Preference",
                keyColumn: "Id",
                keyValue: 100L);

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderTypeId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Service",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "TaxType",
                table: "Order");

            migrationBuilder.AlterColumn<long>(
                name: "TableId",
                table: "Order",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);
        }
    }
}
