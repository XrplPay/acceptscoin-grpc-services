using Microsoft.EntityFrameworkCore.Migrations;

namespace AcceptsCoin.Services.TokenServer.Migrations
{
    public partial class updateTokenIcon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Tokens",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Tokens");
        }
    }
}
