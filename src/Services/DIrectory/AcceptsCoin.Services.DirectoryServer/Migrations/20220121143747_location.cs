using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

namespace AcceptsCoin.Services.DirectoryServer.Migrations
{
    public partial class location : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Businesses",
                type: "geography (point)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Businesses");
        }
    }
}
