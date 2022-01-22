using Microsoft.EntityFrameworkCore.Migrations;

namespace AcceptsCoin.Services.DirectoryServer.Migrations
{
    public partial class ReviewPublishd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Publishd",
                table: "Reviews",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Publishd",
                table: "Reviews");
        }
    }
}
