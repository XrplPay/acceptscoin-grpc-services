using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AcceptsCoin.Services.IdentityServer.Migrations
{
    public partial class initialDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Activated = table.Column<bool>(type: "boolean", nullable: false),
                    SubscribedNewsLetter = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatePassword = table.Column<bool>(type: "boolean", nullable: false),
                    Published = table.Column<bool>(type: "boolean", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_Users_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Published = table.Column<bool>(type: "boolean", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    CreatedById = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedById = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_Roles_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Roles_Users_UpdatedById",
                        column: x => x.UpdatedById,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Activated", "CreatedById", "CreatedDate", "Deleted", "Email", "Name", "Password", "Published", "SubscribedNewsLetter", "UpdatePassword", "UpdatedById", "UpdatedDate", "UserName" },
                values: new object[] { new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5"), true, new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5"), new DateTime(2021, 12, 5, 21, 2, 18, 271, DateTimeKind.Local).AddTicks(609), false, "info@acceptscoin.com", "Super Admin", "superAdmin@123", true, true, true, null, null, "info@acceptscoin.com" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "CreatedById", "CreatedDate", "Deleted", "Name", "Published", "UpdatedById", "UpdatedDate" },
                values: new object[,]
                {
                    { new Guid("27fc6d20-b661-43c5-b48d-93eca8185ece"), new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5"), new DateTime(2021, 12, 5, 21, 2, 18, 293, DateTimeKind.Local).AddTicks(3410), false, "Administrator", true, null, null },
                    { new Guid("90000f34-509f-4a81-877c-0c0cafadb573"), new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5"), new DateTime(2021, 12, 5, 21, 2, 18, 293, DateTimeKind.Local).AddTicks(4885), false, "Business", true, null, null },
                    { new Guid("553ca261-6db1-4413-af03-4c1549a4d1de"), new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5"), new DateTime(2021, 12, 5, 21, 2, 18, 293, DateTimeKind.Local).AddTicks(4901), false, "Partner", true, null, null },
                    { new Guid("25b3c861-2fd3-4b6e-a623-d24010a9500f"), new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5"), new DateTime(2021, 12, 5, 21, 2, 18, 293, DateTimeKind.Local).AddTicks(4911), false, "User", true, null, null }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("27fc6d20-b661-43c5-b48d-93eca8185ece"), new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5") },
                    { new Guid("90000f34-509f-4a81-877c-0c0cafadb573"), new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5") },
                    { new Guid("553ca261-6db1-4413-af03-4c1549a4d1de"), new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5") },
                    { new Guid("25b3c861-2fd3-4b6e-a623-d24010a9500f"), new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedById",
                table: "Roles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UpdatedById",
                table: "Roles",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedById",
                table: "Users",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UpdatedById",
                table: "Users",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
