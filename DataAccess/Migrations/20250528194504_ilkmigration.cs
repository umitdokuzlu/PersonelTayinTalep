using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class ilkmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adliyeler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ad = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adliyeler", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Personeller",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SicilNo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Adi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Soyadi = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Unvan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personeller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TayinTalepleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonelId = table.Column<int>(type: "int", nullable: false),
                    TalepTuru = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Aciklama = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    BasvuruTarihi = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TayinTalepleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TayinTalepleri_Personeller_PersonelId",
                        column: x => x.PersonelId,
                        principalTable: "Personeller",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TayinTalepTercihleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TayinTalepId = table.Column<int>(type: "int", nullable: false),
                    AdliyeId = table.Column<int>(type: "int", nullable: false),
                    Sira = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TayinTalepTercihleri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TayinTalepTercihleri_Adliyeler_AdliyeId",
                        column: x => x.AdliyeId,
                        principalTable: "Adliyeler",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TayinTalepTercihleri_TayinTalepleri_TayinTalepId",
                        column: x => x.TayinTalepId,
                        principalTable: "TayinTalepleri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TayinTalepleri_PersonelId",
                table: "TayinTalepleri",
                column: "PersonelId");

            migrationBuilder.CreateIndex(
                name: "IX_TayinTalepTercihleri_AdliyeId",
                table: "TayinTalepTercihleri",
                column: "AdliyeId");

            migrationBuilder.CreateIndex(
                name: "IX_TayinTalepTercihleri_TayinTalepId",
                table: "TayinTalepTercihleri",
                column: "TayinTalepId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TayinTalepTercihleri");

            migrationBuilder.DropTable(
                name: "Adliyeler");

            migrationBuilder.DropTable(
                name: "TayinTalepleri");

            migrationBuilder.DropTable(
                name: "Personeller");
        }
    }
}
