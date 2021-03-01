using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "JediniceMjere",
                columns: new[] { "PkJedinicaMjereId", "Naziv" },
                values: new object[,]
                {
                    { 1L, "kg" },
                    { 2L, "l" },
                    { 3L, "m" },
                    { 4L, "cm" },
                    { 5L, "dm" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "JediniceMjere",
                keyColumn: "PkJedinicaMjereId",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "JediniceMjere",
                keyColumn: "PkJedinicaMjereId",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "JediniceMjere",
                keyColumn: "PkJedinicaMjereId",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "JediniceMjere",
                keyColumn: "PkJedinicaMjereId",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "JediniceMjere",
                keyColumn: "PkJedinicaMjereId",
                keyValue: 5L);
        }
    }
}
