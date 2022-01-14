using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AcceptsCoin.Services.CoreServer.Migrations
{
    public partial class UpdateCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("711425a4-07b9-4396-bb33-942a73ba6354"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 13, 3, 59, 41, 808, DateTimeKind.Local).AddTicks(6700));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("ba626277-b49c-4e2b-9410-16ca496f278d"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 13, 3, 59, 41, 817, DateTimeKind.Local).AddTicks(3610));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "LanguageId",
                keyValue: new Guid("9934b846-e3f1-406a-9207-04926e553d1b"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 13, 3, 59, 41, 817, DateTimeKind.Local).AddTicks(8930));

            migrationBuilder.UpdateData(
                table: "Partners",
                keyColumn: "PartnerId",
                keyValue: new Guid("bff3b2dd-e89d-46fc-a868-aab93a3efbbe"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 13, 3, 59, 41, 818, DateTimeKind.Local).AddTicks(2500));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("711425a4-07b9-4396-bb33-942a73ba6354"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 13, 0, 33, 36, 156, DateTimeKind.Local).AddTicks(4690));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: new Guid("ba626277-b49c-4e2b-9410-16ca496f278d"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 13, 0, 33, 36, 167, DateTimeKind.Local).AddTicks(9800));

            migrationBuilder.UpdateData(
                table: "Languages",
                keyColumn: "LanguageId",
                keyValue: new Guid("9934b846-e3f1-406a-9207-04926e553d1b"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 13, 0, 33, 36, 168, DateTimeKind.Local).AddTicks(8000));

            migrationBuilder.UpdateData(
                table: "Partners",
                keyColumn: "PartnerId",
                keyValue: new Guid("bff3b2dd-e89d-46fc-a868-aab93a3efbbe"),
                column: "CreatedDate",
                value: new DateTime(2022, 1, 13, 0, 33, 36, 169, DateTimeKind.Local).AddTicks(3600));
        }
    }
}
