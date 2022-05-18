using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace t9n.DAL.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArbEntryAttribute",
                columns: table => new
                {
                    AttributeInternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AttributeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttributeValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArbEntryAttribute", x => x.AttributeInternalId);
                });

            migrationBuilder.CreateTable(
                name: "ArbEntryCollection",
                columns: table => new
                {
                    CollectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Locale = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Context = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArbEntryCollection", x => x.CollectionId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserInternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserPassword = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserInternalId);
                });

            migrationBuilder.CreateTable(
                name: "ArbEntry",
                columns: table => new
                {
                    EntryInternalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntryId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ContextId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DescriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SourceTextId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ScreenId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EntryCollectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArbEntry", x => x.EntryInternalId);
                    table.ForeignKey(
                        name: "FK_ArbEntry_ArbEntryAttribute_ContextId",
                        column: x => x.ContextId,
                        principalTable: "ArbEntryAttribute",
                        principalColumn: "AttributeInternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArbEntry_ArbEntryAttribute_DescriptionId",
                        column: x => x.DescriptionId,
                        principalTable: "ArbEntryAttribute",
                        principalColumn: "AttributeInternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArbEntry_ArbEntryAttribute_ScreenId",
                        column: x => x.ScreenId,
                        principalTable: "ArbEntryAttribute",
                        principalColumn: "AttributeInternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArbEntry_ArbEntryAttribute_SourceTextId",
                        column: x => x.SourceTextId,
                        principalTable: "ArbEntryAttribute",
                        principalColumn: "AttributeInternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArbEntry_ArbEntryAttribute_TypeId",
                        column: x => x.TypeId,
                        principalTable: "ArbEntryAttribute",
                        principalColumn: "AttributeInternalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ArbEntry_ArbEntryCollection_EntryCollectionId",
                        column: x => x.EntryCollectionId,
                        principalTable: "ArbEntryCollection",
                        principalColumn: "CollectionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArbEntry_ContextId",
                table: "ArbEntry",
                column: "ContextId");

            migrationBuilder.CreateIndex(
                name: "IX_ArbEntry_DescriptionId",
                table: "ArbEntry",
                column: "DescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ArbEntry_EntryCollectionId",
                table: "ArbEntry",
                column: "EntryCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_ArbEntry_ScreenId",
                table: "ArbEntry",
                column: "ScreenId");

            migrationBuilder.CreateIndex(
                name: "IX_ArbEntry_SourceTextId",
                table: "ArbEntry",
                column: "SourceTextId");

            migrationBuilder.CreateIndex(
                name: "IX_ArbEntry_TypeId",
                table: "ArbEntry",
                column: "TypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArbEntry");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ArbEntryAttribute");

            migrationBuilder.DropTable(
                name: "ArbEntryCollection");
        }
    }
}
