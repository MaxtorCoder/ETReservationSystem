using Microsoft.EntityFrameworkCore.Migrations;

namespace ReserveringSysteem.Database.Migrations
{
    public partial class AddTelefoonBedrijf : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TelefoonNummer",
                table: "bedrijven",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelefoonNummer",
                table: "bedrijven");
        }
    }
}
