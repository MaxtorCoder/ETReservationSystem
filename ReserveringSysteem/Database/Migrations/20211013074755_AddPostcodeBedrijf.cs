using Microsoft.EntityFrameworkCore.Migrations;

namespace ReserveringSysteem.Database.Migrations
{
    public partial class AddPostcodeBedrijf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostCode",
                table: "bedrijven",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostCode",
                table: "bedrijven");
        }
    }
}
