using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace t9n.DAL.Migrations
{
    public partial class _20220510_Change_TenantStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbTenantDbUser");

            migrationBuilder.AddColumn<Guid>(
                name: "DbUserUserInternalId",
                table: "Tenants",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_DbUserUserInternalId",
                table: "Tenants",
                column: "DbUserUserInternalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_Users_DbUserUserInternalId",
                table: "Tenants",
                column: "DbUserUserInternalId",
                principalTable: "Users",
                principalColumn: "UserInternalId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_Users_DbUserUserInternalId",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_DbUserUserInternalId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "DbUserUserInternalId",
                table: "Tenants");

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
    }
}
