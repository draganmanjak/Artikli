using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Atributi",
                columns: table => new
                {
                    PkAtributId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FkJedinicaMjereId = table.Column<long>(type: "bigint", nullable: true),
                    JedinicaMjere = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atributi", x => x.PkAtributId);
                });

            migrationBuilder.CreateTable(
                name: "JediniceMjere",
                columns: table => new
                {
                    PkJedinicaMjereId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JediniceMjere", x => x.PkJedinicaMjereId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    PkUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrojTelefona = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.PkUserId);
                });

            migrationBuilder.CreateTable(
                name: "Artikli",
                columns: table => new
                {
                    PkArtikliId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sifra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FkJedinicaMjereId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artikli", x => x.PkArtikliId);
                    table.ForeignKey(
                        name: "FK_Artikli_JediniceMjere_FkJedinicaMjereId",
                        column: x => x.FkJedinicaMjereId,
                        principalTable: "JediniceMjere",
                        principalColumn: "PkJedinicaMjereId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AtributiArtikla",
                columns: table => new
                {
                    PkFkArtikalId = table.Column<long>(type: "bigint", nullable: false),
                    PkFkAtributId = table.Column<long>(type: "bigint", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtributiArtikla", x => new { x.PkFkArtikalId, x.PkFkAtributId });
                    table.ForeignKey(
                        name: "FK_AtributiArtikla_Artikli_PkFkArtikalId",
                        column: x => x.PkFkArtikalId,
                        principalTable: "Artikli",
                        principalColumn: "PkArtikliId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AtributiArtikla_Atributi_PkFkAtributId",
                        column: x => x.PkFkAtributId,
                        principalTable: "Atributi",
                        principalColumn: "PkAtributId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Artikli_FkJedinicaMjereId",
                table: "Artikli",
                column: "FkJedinicaMjereId");

            migrationBuilder.CreateIndex(
                name: "IX_AtributiArtikla_PkFkAtributId",
                table: "AtributiArtikla",
                column: "PkFkAtributId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AtributiArtikla");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Artikli");

            migrationBuilder.DropTable(
                name: "Atributi");

            migrationBuilder.DropTable(
                name: "JediniceMjere");
        }
    }
}
