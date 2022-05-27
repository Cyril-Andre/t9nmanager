using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace t9n.DAL.Migrations
{
    public partial class AddProjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbTenantDbUser_Tenants_TenantsTenantInternalId",
                table: "DbTenantDbUser");

            migrationBuilder.DropForeignKey(
                name: "FK_DbTenantDbUser_Users_UsersUserInternalId",
                table: "DbTenantDbUser");

            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "UserBirthdate",
                table: "Users",
                newName: "Birthdate");

            migrationBuilder.RenameColumn(
                name: "UserInternalId",
                table: "Users",
                newName: "InternalId");

            migrationBuilder.RenameColumn(
                name: "TenantName",
                table: "Tenants",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "TenantInternalId",
                table: "Tenants",
                newName: "InternalId");

            migrationBuilder.RenameColumn(
                name: "UsersUserInternalId",
                table: "DbTenantDbUser",
                newName: "UsersInternalId");

            migrationBuilder.RenameColumn(
                name: "TenantsTenantInternalId",
                table: "DbTenantDbUser",
                newName: "TenantsInternalId");

            migrationBuilder.RenameIndex(
                name: "IX_DbTenantDbUser_UsersUserInternalId",
                table: "DbTenantDbUser",
                newName: "IX_DbTenantDbUser_UsersInternalId");

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    InternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantInternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.InternalId);
                    table.ForeignKey(
                        name: "FK_Projects_Tenants_TenantInternalId",
                        column: x => x.TenantInternalId,
                        principalTable: "Tenants",
                        principalColumn: "InternalId");
                });

            migrationBuilder.CreateTable(
                name: "DbProjectDbUser",
                columns: table => new
                {
                    ProjectsInternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersInternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbProjectDbUser", x => new { x.ProjectsInternalId, x.UsersInternalId });
                    table.ForeignKey(
                        name: "FK_DbProjectDbUser_Projects_ProjectsInternalId",
                        column: x => x.ProjectsInternalId,
                        principalTable: "Projects",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbProjectDbUser_Users_UsersInternalId",
                        column: x => x.UsersInternalId,
                        principalTable: "Users",
                        principalColumn: "InternalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbProjectDbUser_UsersInternalId",
                table: "DbProjectDbUser",
                column: "UsersInternalId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TenantInternalId",
                table: "Projects",
                column: "TenantInternalId");

            migrationBuilder.AddForeignKey(
                name: "FK_DbTenantDbUser_Tenants_TenantsInternalId",
                table: "DbTenantDbUser",
                column: "TenantsInternalId",
                principalTable: "Tenants",
                principalColumn: "InternalId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DbTenantDbUser_Users_UsersInternalId",
                table: "DbTenantDbUser",
                column: "UsersInternalId",
                principalTable: "Users",
                principalColumn: "InternalId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbTenantDbUser_Tenants_TenantsInternalId",
                table: "DbTenantDbUser");

            migrationBuilder.DropForeignKey(
                name: "FK_DbTenantDbUser_Users_UsersInternalId",
                table: "DbTenantDbUser");

            migrationBuilder.DropTable(
                name: "DbProjectDbUser");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Users",
                newName: "UserEmail");

            migrationBuilder.RenameColumn(
                name: "Birthdate",
                table: "Users",
                newName: "UserBirthdate");

            migrationBuilder.RenameColumn(
                name: "InternalId",
                table: "Users",
                newName: "UserInternalId");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tenants",
                newName: "TenantName");

            migrationBuilder.RenameColumn(
                name: "InternalId",
                table: "Tenants",
                newName: "TenantInternalId");

            migrationBuilder.RenameColumn(
                name: "UsersInternalId",
                table: "DbTenantDbUser",
                newName: "UsersUserInternalId");

            migrationBuilder.RenameColumn(
                name: "TenantsInternalId",
                table: "DbTenantDbUser",
                newName: "TenantsTenantInternalId");

            migrationBuilder.RenameIndex(
                name: "IX_DbTenantDbUser_UsersInternalId",
                table: "DbTenantDbUser",
                newName: "IX_DbTenantDbUser_UsersUserInternalId");

            migrationBuilder.AddForeignKey(
                name: "FK_DbTenantDbUser_Tenants_TenantsTenantInternalId",
                table: "DbTenantDbUser",
                column: "TenantsTenantInternalId",
                principalTable: "Tenants",
                principalColumn: "TenantInternalId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DbTenantDbUser_Users_UsersUserInternalId",
                table: "DbTenantDbUser",
                column: "UsersUserInternalId",
                principalTable: "Users",
                principalColumn: "UserInternalId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
