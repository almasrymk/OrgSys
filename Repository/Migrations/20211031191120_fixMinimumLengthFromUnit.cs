using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class fixMinimumLengthFromUnit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "BranchId", "Code", "CodeNumber", "Hide", "ImgPath", "MaskText", "Name", "ParentId", "Password", "RoleId", "Status", "TypeId", "UserName" },
                values: new object[] { 3L, null, null, 0L, false, null, null, "Emp", 0L, null, 2L, 0, 0L, "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "Id",
                keyValue: 3L);
        }
    }
}
