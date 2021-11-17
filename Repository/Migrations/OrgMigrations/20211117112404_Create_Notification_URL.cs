using Microsoft.EntityFrameworkCore.Migrations;

namespace Repository.Migrations.OrgMigrations
{
    public partial class Create_Notification_URL : Migration
    {
        public string _Schema { get; set; } = "org";
        public Create_Notification_URL() { }
        public Create_Notification_URL(string Schema) { this._Schema = Schema; }
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "URL",
                schema: _Schema,
                table: "Notification",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "URL",
                schema: _Schema,
                table: "Notification");
        }
    }
}
