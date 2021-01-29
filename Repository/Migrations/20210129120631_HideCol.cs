using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class HideCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Hide",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Hide",
                table: "Unit",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Hide",
                table: "Store",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Hide",
                table: "Shift",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Hide",
                table: "Role",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Hide",
                table: "ProductUnit",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Hide",
                table: "Product",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Hide",
                table: "Permission",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Hide",
                table: "Dealer",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Hide",
                table: "Classification",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Hide",
                table: "Branch",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Value",
                value: "AllPage.All");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10L,
                column: "Value",
                value: "Setting.All");

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Status", "TypeId", "Value" },
                values: new object[,]
                {
                    { 20404L, false, null, "Delete", null, 204L, 0, 0L, "Clients.Delete" },
                    { 20403L, false, null, "Edit", null, 204L, 0, 0L, "Clients.Edit" },
                    { 20402L, false, null, "Add", null, 204L, 0, 0L, "Clients.Add" },
                    { 20401L, false, null, "View", null, 204L, 0, 0L, "Clients.View" },
                    { 204L, false, null, "Clients", null, 20L, 0, 0L, "Clients.All" },
                    { 20304L, false, null, "Delete", null, 203L, 0, 0L, "UnitsMeasure.Delete" },
                    { 20303L, false, null, "Edit", null, 203L, 0, 0L, "UnitsMeasure.Edit" },
                    { 20302L, false, null, "Add", null, 203L, 0, 0L, "UnitsMeasure.Add" },
                    { 20301L, false, null, "View", null, 203L, 0, 0L, "UnitsMeasure.View" },
                    { 203L, false, null, "UnitsMeasure", null, 20L, 0, 0L, "UnitsMeasure.All" },
                    { 20203L, false, null, "Edit", null, 202L, 0, 0L, "Classifications.Edit" },
                    { 20202L, false, null, "Add", null, 202L, 0, 0L, "Classifications.Add" },
                    { 20201L, false, null, "View", null, 202L, 0, 0L, "Classifications.View" },
                    { 202L, false, null, "Classifications", null, 20L, 0, 0L, "Classifications.All" },
                    { 20104L, false, null, "Delete", null, 201L, 0, 0L, "Products.Delete" },
                    { 20103L, false, null, "Edit", null, 201L, 0, 0L, "Products.Edit" },
                    { 20102L, false, null, "Add", null, 201L, 0, 0L, "Products.Add" },
                    { 20101L, false, null, "View", null, 201L, 0, 0L, "Products.View" },
                    { 201L, false, null, "Products", null, 20L, 0, 0L, "Products.All" },
                    { 20204L, false, null, "Delete", null, 202L, 0, 0L, "Classifications.Delete" },
                    { 20L, false, null, "Sales", null, 1L, 0, 0L, "Sales.All" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 201L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 202L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 203L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 204L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20103L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20104L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20201L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20202L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20203L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20204L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20301L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20302L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20303L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20304L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20401L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20402L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20403L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 20404L);

            migrationBuilder.DropColumn(
                name: "Hide",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Hide",
                table: "Unit");

            migrationBuilder.DropColumn(
                name: "Hide",
                table: "Store");

            migrationBuilder.DropColumn(
                name: "Hide",
                table: "Shift");

            migrationBuilder.DropColumn(
                name: "Hide",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "Hide",
                table: "ProductUnit");

            migrationBuilder.DropColumn(
                name: "Hide",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Hide",
                table: "Permission");

            migrationBuilder.DropColumn(
                name: "Hide",
                table: "Dealer");

            migrationBuilder.DropColumn(
                name: "Hide",
                table: "Classification");

            migrationBuilder.DropColumn(
                name: "Hide",
                table: "Branch");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Value",
                value: "AllPage");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10L,
                column: "Value",
                value: "Setting");
        }
    }
}
