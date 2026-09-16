using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrgSys.DatabaseMigrator.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPasswordHashing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Existing User.Password values are AES ciphertext and cannot be verified against
            // the new Identity hasher. Backfill MustResetPassword = 1 so every pre-existing
            // account is forced through a password reset; the seeder then clears the flag
            // for Owner/Admin after writing Identity hashes.
            migrationBuilder.AddColumn<bool>(
                name: "MustResetPassword",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "MustResetPassword",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MustResetPassword",
                table: "User");
        }
    }
}
