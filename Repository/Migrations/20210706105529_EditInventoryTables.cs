using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class EditInventoryTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryProduct_InventoryStore_InventoryStoreId",
                table: "InventoryProduct");

            migrationBuilder.DropTable(
                name: "InventoryStore");

            migrationBuilder.RenameColumn(
                name: "InventoryStoreId",
                table: "InventoryProduct",
                newName: "InventoryId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryProduct_InventoryStoreId",
                table: "InventoryProduct",
                newName: "IX_InventoryProduct_InventoryId");

            migrationBuilder.AddColumn<long>(
                name: "StoreId",
                table: "Inventory",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_StoreId",
                table: "Inventory",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Store_StoreId",
                table: "Inventory",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryProduct_Inventory_InventoryId",
                table: "InventoryProduct",
                column: "InventoryId",
                principalTable: "Inventory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Store_StoreId",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryProduct_Inventory_InventoryId",
                table: "InventoryProduct");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_StoreId",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Inventory");

            migrationBuilder.RenameColumn(
                name: "InventoryId",
                table: "InventoryProduct",
                newName: "InventoryStoreId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryProduct_InventoryId",
                table: "InventoryProduct",
                newName: "IX_InventoryProduct_InventoryStoreId");

            migrationBuilder.CreateTable(
                name: "InventoryStore",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    CreateTransaction = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InventoryId = table.Column<long>(type: "bigint", nullable: false),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    Review = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryStore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryStore_Inventory_InventoryId",
                        column: x => x.InventoryId,
                        principalTable: "Inventory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryStore_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryStore_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryStore_InventoryId",
                table: "InventoryStore",
                column: "InventoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryStore_StoreId",
                table: "InventoryStore",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryStore_UserId",
                table: "InventoryStore",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryProduct_InventoryStore_InventoryStoreId",
                table: "InventoryProduct",
                column: "InventoryStoreId",
                principalTable: "InventoryStore",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
