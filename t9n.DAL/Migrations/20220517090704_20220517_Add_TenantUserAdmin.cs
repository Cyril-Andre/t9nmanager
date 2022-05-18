using Microsoft.EntityFrameworkCore.Migrations;

namespace t9n.DAL.Migrations
{
    public partial class _20220517_Add_TenantUserAdmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminUserName",
                table: "Tenants",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminUserName",
                table: "Tenants");
        }
    }
}
