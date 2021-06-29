using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class AddOutlayIdFinancial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OutlayId",
                table: "Financial",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Financial_OutlayId",
                table: "Financial",
                column: "OutlayId");

            migrationBuilder.AddForeignKey(
                name: "FK_Financial_Outlay_OutlayId",
                table: "Financial",
                column: "OutlayId",
                principalTable: "Outlay",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Financial_Outlay_OutlayId",
                table: "Financial");

            migrationBuilder.DropIndex(
                name: "IX_Financial_OutlayId",
                table: "Financial");

            migrationBuilder.DropColumn(
                name: "OutlayId",
                table: "Financial");
        }
    }
}
