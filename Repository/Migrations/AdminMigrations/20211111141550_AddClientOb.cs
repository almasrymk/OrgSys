using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations.AdminMigrations
{
    public partial class AddClientOb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LoginUser_ClientId",
                schema: "admin",
                table: "LoginUser",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoginUser_Client_ClientId",
                schema: "admin",
                table: "LoginUser",
                column: "ClientId",
                principalSchema: "admin",
                principalTable: "Client",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginUser_Client_ClientId",
                schema: "admin",
                table: "LoginUser");

            migrationBuilder.DropIndex(
                name: "IX_LoginUser_ClientId",
                schema: "admin",
                table: "LoginUser");
        }
    }
}
