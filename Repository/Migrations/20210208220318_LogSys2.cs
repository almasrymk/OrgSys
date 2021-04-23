using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations
{
    public partial class LogSys2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogSys",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    ResourceId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScreenName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    EstimateBySecond = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Line = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogType = table.Column<int>(type: "int", nullable: false),
                    LogStatus = table.Column<int>(type: "int", nullable: false),
                    LogAccessLevel = table.Column<int>(type: "int", nullable: false),
                    Sent = table.Column<bool>(type: "bit", nullable: false),
                    MaskText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<long>(type: "bigint", nullable: false),
                    TypeId = table.Column<long>(type: "bigint", nullable: false),
                    Hide = table.Column<bool>(type: "bit", nullable: false),
                    ImgPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogSys", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogSys");
        }
    }
}
