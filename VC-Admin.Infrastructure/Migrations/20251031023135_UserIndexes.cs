using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VC_Admin.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "Users",
                newName: "IDX_User_Email_Normalized");

            migrationBuilder.CreateIndex(
                name: "IDX_User_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IDX_User_Email_TrimLower"";");
            migrationBuilder.Sql(@"CREATE UNIQUE INDEX ""IDX_User_Email_TrimLower"" ON ""Users"" (TRIM(LOWER(""Email"")));");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IDX_User_Email_TrimLower"";");

            migrationBuilder.DropIndex(
                name: "IDX_User_Username",
                table: "Users");

            migrationBuilder.RenameIndex(
                name: "IDX_User_Email_Normalized",
                table: "Users",
                newName: "IX_Users_Email");
        }
    }
}
