using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReserveringSysteem.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "vestigingen",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    MaxTafels = table.Column<int>(type: "int", nullable: false),
                    MaxPersonen = table.Column<int>(type: "int", nullable: false),
                    OpeningsTijd = table.Column<TimeSpan>(type: "time", nullable: false),
                    SluitingsTijd = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vestiging", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "reserveringen",
                columns: table => new
                {
                    ReserveringID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID = table.Column<int>(type: "int", nullable: false),
                    Tafel = table.Column<int>(type: "int", nullable: false),
                    AantalPersonen = table.Column<int>(type: "int", nullable: false),
                    NaamReserverende = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    TelefoonNummer = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Tijd = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserveringen", x => x.ReserveringID);
                    table.ForeignKey(
                        name: "FK__reservering_id__vestiging_id",
                        column: x => x.ID,
                        principalTable: "vestigingen",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bedrijven",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naam = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Adress = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    BTWNummer = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    KVKNummer = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    ReserveringID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bedrijven", x => x.ID);
                    table.ForeignKey(
                        name: "FK_bedrijven_reserveringen_ReserveringID",
                        column: x => x.ReserveringID,
                        principalTable: "reserveringen",
                        principalColumn: "ReserveringID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bedrijven_ReserveringID",
                table: "bedrijven",
                column: "ReserveringID");

            migrationBuilder.CreateIndex(
                name: "IX_reserveringen_ID",
                table: "reserveringen",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bedrijven");

            migrationBuilder.DropTable(
                name: "reserveringen");

            migrationBuilder.DropTable(
                name: "vestigingen");
        }
    }
}
