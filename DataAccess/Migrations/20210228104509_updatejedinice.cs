using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class updatejedinice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artikli_JediniceMjere_FkJedinicaMjereId",
                table: "Artikli");

            migrationBuilder.AlterColumn<long>(
                name: "FkJedinicaMjereId",
                table: "Artikli",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Artikli_JediniceMjere_FkJedinicaMjereId",
                table: "Artikli",
                column: "FkJedinicaMjereId",
                principalTable: "JediniceMjere",
                principalColumn: "PkJedinicaMjereId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Artikli_JediniceMjere_FkJedinicaMjereId",
                table: "Artikli");

            migrationBuilder.AlterColumn<long>(
                name: "FkJedinicaMjereId",
                table: "Artikli",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Artikli_JediniceMjere_FkJedinicaMjereId",
                table: "Artikli",
                column: "FkJedinicaMjereId",
                principalTable: "JediniceMjere",
                principalColumn: "PkJedinicaMjereId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
