using Microsoft.EntityFrameworkCore.Migrations;

namespace t9n.DAL.Migrations
{
    public partial class _202204062_AddUserPasswordHash_Salt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserPassword",
                table: "Users",
                newName: "UserPasswordHash");

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "UserPasswordHash",
                table: "Users",
                newName: "UserPassword");
        }
    }
}
