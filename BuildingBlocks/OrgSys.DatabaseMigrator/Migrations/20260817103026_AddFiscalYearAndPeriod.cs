using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrgSys.DatabaseMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddFiscalYearAndPeriod : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FiscalPeriodId",
                table: "Journal",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "FiscalYearId",
                table: "Journal",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "FiscalYear",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    FiscalYearStatus = table.Column<int>(type: "int", nullable: false),
                    CodeNumber = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalYear", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FiscalPeriod",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FiscalYearId = table.Column<long>(type: "bigint", nullable: false),
                    PeriodNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FiscalPeriodStatus = table.Column<int>(type: "int", nullable: false),
                    CodeNumber = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FiscalPeriod", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FiscalPeriod_FiscalYear_FiscalYearId",
                        column: x => x.FiscalYearId,
                        principalTable: "FiscalYear",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Journal_FiscalPeriodId",
                table: "Journal",
                column: "FiscalPeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_Journal_FiscalYearId",
                table: "Journal",
                column: "FiscalYearId");

            migrationBuilder.CreateIndex(
                name: "IX_FiscalPeriod_FiscalYearId",
                table: "FiscalPeriod",
                column: "FiscalYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Journal_FiscalPeriod_FiscalPeriodId",
                table: "Journal",
                column: "FiscalPeriodId",
                principalTable: "FiscalPeriod",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Journal_FiscalYear_FiscalYearId",
                table: "Journal",
                column: "FiscalYearId",
                principalTable: "FiscalYear",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Journal_FiscalPeriod_FiscalPeriodId",
                table: "Journal");

            migrationBuilder.DropForeignKey(
                name: "FK_Journal_FiscalYear_FiscalYearId",
                table: "Journal");

            migrationBuilder.DropTable(
                name: "FiscalPeriod");

            migrationBuilder.DropTable(
                name: "FiscalYear");

            migrationBuilder.DropIndex(
                name: "IX_Journal_FiscalPeriodId",
                table: "Journal");

            migrationBuilder.DropIndex(
                name: "IX_Journal_FiscalYearId",
                table: "Journal");

            migrationBuilder.DropColumn(
                name: "FiscalPeriodId",
                table: "Journal");

            migrationBuilder.DropColumn(
                name: "FiscalYearId",
                table: "Journal");
        }
    }
}
