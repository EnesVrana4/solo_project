using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PeopleForPeople.Migrations
{
    public partial class PeopleForPeopleMigration8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Myimage",
                table: "Cases",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Myimage",
                table: "Cases");
        }
    }
}
