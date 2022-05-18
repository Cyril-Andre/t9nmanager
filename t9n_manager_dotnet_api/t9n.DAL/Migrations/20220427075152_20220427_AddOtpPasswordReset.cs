using Microsoft.EntityFrameworkCore.Migrations;

namespace t9n.DAL.Migrations
{
    public partial class _20220427_AddOtpPasswordReset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordOtp",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordOtp",
                table: "Users");
        }
    }
}
