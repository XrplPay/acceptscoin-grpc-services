using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AcceptsCoin.Services.IdentityServer.Migrations
{
    public partial class Partner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PartnerId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Partners",
                columns: table => new
                {
                    PartnerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partners", x => x.PartnerId);
                });

            migrationBuilder.InsertData(
                table: "Partners",
                column: "PartnerId",
                value: new Guid("bff3b2dd-e89d-46fc-a868-aab93a3efbbe"));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("25b3c861-2fd3-4b6e-a623-d24010a9500f"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 17, 2, 42, 29, 581, DateTimeKind.Local).AddTicks(9300));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("27fc6d20-b661-43c5-b48d-93eca8185ece"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 17, 2, 42, 29, 581, DateTimeKind.Local).AddTicks(6630));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("553ca261-6db1-4413-af03-4c1549a4d1de"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 17, 2, 42, 29, 581, DateTimeKind.Local).AddTicks(9290));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("90000f34-509f-4a81-877c-0c0cafadb573"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 17, 2, 42, 29, 581, DateTimeKind.Local).AddTicks(9270));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 17, 2, 42, 29, 547, DateTimeKind.Local).AddTicks(5660));

            migrationBuilder.CreateIndex(
                name: "IX_Users_PartnerId",
                table: "Users",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Partners_PartnerId",
                table: "Users",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "PartnerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Partners_PartnerId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Partners");

            migrationBuilder.DropIndex(
                name: "IX_Users_PartnerId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PartnerId",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("25b3c861-2fd3-4b6e-a623-d24010a9500f"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 13, 0, 40, 47, 87, DateTimeKind.Local).AddTicks(2900));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("27fc6d20-b661-43c5-b48d-93eca8185ece"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 13, 0, 40, 47, 86, DateTimeKind.Local).AddTicks(6790));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("553ca261-6db1-4413-af03-4c1549a4d1de"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 13, 0, 40, 47, 87, DateTimeKind.Local).AddTicks(2880));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: new Guid("90000f34-509f-4a81-877c-0c0cafadb573"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 13, 0, 40, 47, 87, DateTimeKind.Local).AddTicks(2830));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("999bb90f-3167-4f81-83bb-0c76d1d3ace5"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 13, 0, 40, 47, 48, DateTimeKind.Local).AddTicks(8030));
        }
    }
}
