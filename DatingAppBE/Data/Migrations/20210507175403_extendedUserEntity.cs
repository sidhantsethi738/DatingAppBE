using Microsoft.EntityFrameworkCore.Migrations;

namespace DatingAppBE.Data.Migrations
{
    public partial class extendedUserEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "country",
                table: "Users",
                newName: "Country");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Users",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "Users",
                newName: "country");
        }
    }
}
