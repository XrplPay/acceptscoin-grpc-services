using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AcceptsCoin.Services.DirectoryServer.Migrations
{
    public partial class BusinessGallery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BusinessGalleries",
                columns: table => new
                {
                    BusinessGalleryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Extension = table.Column<string>(type: "text", nullable: false),
                    BusinessId = table.Column<Guid>(type: "uuid", nullable: false),
                    Published = table.Column<bool>(type: "boolean", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BusinessGalleries", x => x.BusinessGalleryId);
                    table.ForeignKey(
                        name: "FK_BusinessGalleries_Businesses_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "Businesses",
                        principalColumn: "BusinessId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessGalleries_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BusinessGalleries_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessGalleries_BusinessId",
                table: "BusinessGalleries",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessGalleries_CreatedById",
                table: "BusinessGalleries",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessGalleries_UpdatedById",
                table: "BusinessGalleries",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BusinessGalleries");
        }
    }
}
