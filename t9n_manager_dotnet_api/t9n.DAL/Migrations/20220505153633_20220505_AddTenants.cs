using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace t9n.DAL.Migrations
{
    public partial class _20220505_AddTenants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantInternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantInternalId);
                });

            migrationBuilder.CreateTable(
                name: "DbTenantDbUser",
                columns: table => new
                {
                    TenantUsersUserInternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserTenantsTenantInternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbTenantDbUser", x => new { x.TenantUsersUserInternalId, x.UserTenantsTenantInternalId });
                    table.ForeignKey(
                        name: "FK_DbTenantDbUser_Tenants_UserTenantsTenantInternalId",
                        column: x => x.UserTenantsTenantInternalId,
                        principalTable: "Tenants",
                        principalColumn: "TenantInternalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbTenantDbUser_Users_TenantUsersUserInternalId",
                        column: x => x.TenantUsersUserInternalId,
                        principalTable: "Users",
                        principalColumn: "UserInternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbTenantDbUser_UserTenantsTenantInternalId",
                table: "DbTenantDbUser",
                column: "UserTenantsTenantInternalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbTenantDbUser");

            migrationBuilder.DropTable(
                name: "Tenants");
        }
    }
}
