using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddJournalReversalLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OriginalJournalId",
                table: "Journal",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Journal_OriginalJournalId",
                table: "Journal",
                column: "OriginalJournalId",
                unique: true,
                filter: "[OriginalJournalId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Journal_Journal_OriginalJournalId",
                table: "Journal",
                column: "OriginalJournalId",
                principalTable: "Journal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Journal_Journal_OriginalJournalId",
                table: "Journal");

            migrationBuilder.DropIndex(
                name: "IX_Journal_OriginalJournalId",
                table: "Journal");

            migrationBuilder.DropColumn(
                name: "OriginalJournalId",
                table: "Journal");
        }
    }
}
