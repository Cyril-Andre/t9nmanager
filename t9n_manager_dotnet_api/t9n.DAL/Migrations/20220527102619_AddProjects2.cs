using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace t9n.DAL.Migrations
{
    public partial class AddProjects2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tenants",
                columns: new[] { "InternalId", "AdminUserName", "Name" },
                values: new object[] { new Guid("1af0e668-086f-4a3c-8196-3231f21f6636"), "t9nAdmin", "Public" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tenants",
                keyColumn: "InternalId",
                keyValue: new Guid("1af0e668-086f-4a3c-8196-3231f21f6636"));
        }
    }
}
