using Microsoft.EntityFrameworkCore.Migrations;

namespace t9n.DAL.Migrations
{
    public partial class _202204071_AddUserEmailValidation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UserEmailValidated",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserEmailValidated",
                table: "Users");
        }
    }
}
