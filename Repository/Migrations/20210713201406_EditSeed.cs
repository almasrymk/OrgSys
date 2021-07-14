using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class EditSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                keyValue: 10204L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10304L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10403L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10404L);

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

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Permission",
                newName: "Name2");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Permission",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RolePermission",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    PermissionId = table.Column<long>(type: "bigint", nullable: false),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Organizer", "Organizer", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Setting.All", "Setting", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 101L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Organization", "Organization", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 102L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Security", "Security", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 103L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Products", "Products", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 104L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Dealers", "Dealers", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10101L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Branchs.All", "Branchs", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10102L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Store.All", "Store", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10103L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Tables.All", "Tables", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10104L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Safes.All", "Safes", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10201L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Roles.All", "Roles", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10202L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Users.All", "Users", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10203L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Sfilts.All", "Sfilts", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10301L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Products.All", "Products", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10302L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Classifications.All", "Classifications", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10303L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "UnitsMeasure.All", "Units Measure", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10401L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Clients.All", "Clients", null });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10402L,
                columns: new[] { "Key", "Name", "Name2" },
                values: new object[] { "Suppliers.All", "Suppliers", null });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "Name", "Name2", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 1040103L, false, null, "Clients.Edit", null, "Edit", null, 10401L, 0, 1L },
                    { 1040104L, false, null, "Clients.Delete", null, "Delete", null, 10401L, 0, 1L },
                    { 1040201L, false, null, "Suppliers.View", null, "View", null, 10402L, 0, 1L },
                    { 1040202L, false, null, "Suppliers.Add", null, "Add", null, 10402L, 0, 1L },
                    { 1040203L, false, null, "Suppliers.Edit", null, "Edit", null, 10402L, 0, 1L },
                    { 1040204L, false, null, "Suppliers.Delete", null, "Delete", null, 10402L, 0, 1L },
                    { 105L, false, null, "Financials", null, "Financials", null, 10L, 0, 0L },
                    { 10501L, false, null, "OutlayTerms.All", null, "Outlay Terms", null, 105L, 0, 0L },
                    { 1050101L, false, null, "OutlayTerms.View", null, "View", null, 10501L, 0, 1L },
                    { 1010101L, false, null, "Branchs.View", null, "View", null, 10101L, 0, 1L },
                    { 1050103L, false, null, "OutlayTerms.Edit", null, "Edit", null, 10501L, 0, 1L },
                    { 1050104L, false, null, "OutlayTerms.Delete", null, "Delete", null, 10501L, 0, 1L },
                    { 10502L, false, null, "Currencies.All", null, "Currencies", null, 105L, 0, 0L },
                    { 1050201L, false, null, "Currencies.View", null, "View", null, 10502L, 0, 1L },
                    { 1050202L, false, null, "Currencies.Add", null, "Add", null, 10502L, 0, 1L },
                    { 1050203L, false, null, "Currencies.Edit", null, "Edit", null, 10502L, 0, 1L },
                    { 1050204L, false, null, "Currencies.Delete", null, "Delete", null, 10502L, 0, 1L },
                    { 1040102L, false, null, "Clients.Add", null, "Add", null, 10401L, 0, 1L },
                    { 1050102L, false, null, "OutlayTerms.Add", null, "Add", null, 10501L, 0, 1L },
                    { 1040101L, false, null, "Clients.View", null, "View", null, 10401L, 0, 1L },
                    { 1030304L, false, null, "UnitsMeasure.Delete", null, "Delete", null, 10303L, 0, 1L },
                    { 1020104L, false, null, "Roles.Delete", null, "Delete", null, 10201L, 0, 1L },
                    { 1020102L, false, null, "Roles.Add", null, "Add", null, 10201L, 0, 1L },
                    { 1020101L, false, null, "Roles.View", null, "View", null, 10201L, 0, 1L }
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "Name", "Name2", "ParentId", "Status", "TypeId" },
                values: new object[,]
                {
                    { 1010404L, false, null, "Safes.Delete", null, "Delete", null, 10104L, 0, 1L },
                    { 1010403L, false, null, "Safes.Edit", null, "Edit", null, 10104L, 0, 1L },
                    { 1010402L, false, null, "Safes.Add", null, "Add", null, 10104L, 0, 1L },
                    { 1010401L, false, null, "Safes.View", null, "View", null, 10104L, 0, 1L },
                    { 1010304L, false, null, "Tables.Delete", null, "Delete", null, 10103L, 0, 1L },
                    { 1010303L, false, null, "Tables.Edit", null, "Edit", null, 10103L, 0, 1L },
                    { 1010302L, false, null, "Tables.Add", null, "Add", null, 10103L, 0, 1L },
                    { 1010301L, false, null, "Tables.View", null, "View", null, 10103L, 0, 1L },
                    { 1010204L, false, null, "Store.Delete", null, "Delete", null, 10102L, 0, 1L },
                    { 1010203L, false, null, "Store.Edit", null, "Edit", null, 10102L, 0, 1L },
                    { 1010202L, false, null, "Store.Add", null, "Add", null, 10102L, 0, 1L },
                    { 1010201L, false, null, "Store.View", null, "View", null, 10102L, 0, 1L },
                    { 1010104L, false, null, "Branchs.Delete", null, "Delete", null, 10101L, 0, 1L },
                    { 1020103L, false, null, "Roles.Edit", null, "Edit", null, 10201L, 0, 1L },
                    { 1030303L, false, null, "UnitsMeasure.Edit", null, "Edit", null, 10303L, 0, 1L },
                    { 1020201L, false, null, "Users.View", null, "View", null, 10202L, 0, 1L },
                    { 1020202L, false, null, "Users.Add", null, "Add", null, 10202L, 0, 1L },
                    { 1030302L, false, null, "UnitsMeasure.Add", null, "Add", null, 10303L, 0, 1L },
                    { 1030301L, false, null, "UnitsMeasure.View", null, "View", null, 10303L, 0, 1L },
                    { 1030204L, false, null, "Classifications.Delete", null, "Delete", null, 10302L, 0, 1L },
                    { 1030203L, false, null, "Classifications.Edit", null, "Edit", null, 10302L, 0, 1L },
                    { 1030202L, false, null, "Classifications.Add", null, "Add", null, 10302L, 0, 1L },
                    { 1030201L, false, null, "Classifications.View", null, "View", null, 10302L, 0, 1L },
                    { 1030104L, false, null, "Products.Delete", null, "Delete", null, 10301L, 0, 1L },
                    { 1010103L, false, null, "Branchs.Edit", null, "Edit", null, 10101L, 0, 1L },
                    { 1030103L, false, null, "Products.Edit", null, "Edit", null, 10301L, 0, 1L },
                    { 1030101L, false, null, "Products.View", null, "View", null, 10301L, 0, 1L },
                    { 1020304L, false, null, "Sfilts.Delete", null, "Delete", null, 10203L, 0, 1L },
                    { 1020303L, false, null, "Sfilts.Edit", null, "Edit", null, 10203L, 0, 1L },
                    { 1020302L, false, null, "Sfilts.Add", null, "Add", null, 10203L, 0, 1L },
                    { 1020301L, false, null, "Sfilts.View", null, "View", null, 10203L, 0, 1L },
                    { 1020204L, false, null, "Users.Delete", null, "Delete", null, 10202L, 0, 1L },
                    { 1020203L, false, null, "Users.Edit", null, "Edit", null, 10202L, 0, 1L },
                    { 1030102L, false, null, "Products.Add", null, "Add", null, 10301L, 0, 1L },
                    { 1010102L, false, null, "Branchs.Add", null, "Add", null, 10101L, 0, 1L }
                });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Hide",
                value: true);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Hide",
                value: true);

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_PermissionId",
                table: "RolePermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                table: "RolePermission",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermission");

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 105L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10501L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10502L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010103L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010104L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010201L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010202L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010203L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010204L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010301L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010302L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010303L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010304L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010401L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010402L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010403L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1010404L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020103L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020104L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020201L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020202L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020203L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020204L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020301L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020302L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020303L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1020304L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1030101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1030102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1030103L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1030104L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1030201L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1030202L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1030203L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1030204L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1030301L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1030302L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1030303L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1030304L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1040101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1040102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1040103L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1040104L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1040201L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1040202L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1040203L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1040204L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050101L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050102L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050103L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050104L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050201L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050202L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050203L);

            migrationBuilder.DeleteData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1050204L);

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Permission");

            migrationBuilder.RenameColumn(
                name: "Name2",
                table: "Permission",
                newName: "Value");

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "AllPage", "AllPage.All" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "Setting", "Setting.All" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 101L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "Branchs", "Branchs.All" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 102L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "Roles", "Roles.All" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 103L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "Users", "Users.All" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 104L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "Shifts", "Shifts.All" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10101L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "View", "Branchs.View" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10102L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "Add", "Branchs.Add" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10103L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "Edit", "Branchs.Edit" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10104L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "Delete", "Branchs.Delete" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10201L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "View", "Roles.View" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10202L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "Add", "Roles.Add" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10203L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "Edit", "Roles.Edit" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10301L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "View", "Users.View" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10302L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "Add", "Users.Add" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10303L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "Edit", "Users.Edit" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10401L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "View", "Shifts.View" });

            migrationBuilder.UpdateData(
                table: "Permission",
                keyColumn: "Id",
                keyValue: 10402L,
                columns: new[] { "Key", "Value" },
                values: new object[] { "Add", "Shifts.Add" });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Status", "TypeId", "Value" },
                values: new object[,]
                {
                    { 10204L, false, null, "Delete", null, 102L, 0, 0L, "Roles.Delete" },
                    { 20404L, false, null, "Delete", null, 204L, 0, 0L, "Clients.Delete" },
                    { 20403L, false, null, "Edit", null, 204L, 0, 0L, "Clients.Edit" },
                    { 20402L, false, null, "Add", null, 204L, 0, 0L, "Clients.Add" },
                    { 202L, false, null, "Classifications", null, 20L, 0, 0L, "Classifications.All" },
                    { 10404L, false, null, "Delete", null, 104L, 0, 0L, "Shifts.Delete" },
                    { 20L, false, null, "Sales", null, 1L, 0, 0L, "Sales.All" },
                    { 201L, false, null, "Products", null, 20L, 0, 0L, "Products.All" },
                    { 20101L, false, null, "View", null, 201L, 0, 0L, "Products.View" },
                    { 20102L, false, null, "Add", null, 201L, 0, 0L, "Products.Add" },
                    { 20103L, false, null, "Edit", null, 201L, 0, 0L, "Products.Edit" },
                    { 20104L, false, null, "Delete", null, 201L, 0, 0L, "Products.Delete" },
                    { 20401L, false, null, "View", null, 204L, 0, 0L, "Clients.View" },
                    { 10403L, false, null, "Edit", null, 104L, 0, 0L, "Shifts.Edit" },
                    { 20201L, false, null, "View", null, 202L, 0, 0L, "Classifications.View" },
                    { 20203L, false, null, "Edit", null, 202L, 0, 0L, "Classifications.Edit" },
                    { 20204L, false, null, "Delete", null, 202L, 0, 0L, "Classifications.Delete" },
                    { 203L, false, null, "UnitsMeasure", null, 20L, 0, 0L, "UnitsMeasure.All" },
                    { 20301L, false, null, "View", null, 203L, 0, 0L, "UnitsMeasure.View" },
                    { 20302L, false, null, "Add", null, 203L, 0, 0L, "UnitsMeasure.Add" },
                    { 20303L, false, null, "Edit", null, 203L, 0, 0L, "UnitsMeasure.Edit" },
                    { 20304L, false, null, "Delete", null, 203L, 0, 0L, "UnitsMeasure.Delete" },
                    { 204L, false, null, "Clients", null, 20L, 0, 0L, "Clients.All" },
                    { 20202L, false, null, "Add", null, 202L, 0, 0L, "Classifications.Add" }
                });

            migrationBuilder.InsertData(
                table: "Permission",
                columns: new[] { "Id", "Hide", "ImgPath", "Key", "MaskText", "ParentId", "Status", "TypeId", "Value" },
                values: new object[] { 10304L, false, null, "Delete", null, 103L, 0, 0L, "Users.Delete" });

            migrationBuilder.UpdateData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Hide",
                value: false);

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1L,
                column: "Hide",
                value: false);
        }
    }
}
