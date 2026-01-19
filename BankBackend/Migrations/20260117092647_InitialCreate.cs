using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BankBackend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenelIstatistikler",
                columns: table => new
                {
                    toplam_musteri = table.Column<int>(type: "integer", nullable: false),
                    toplam_para = table.Column<decimal>(type: "numeric", nullable: false),
                    toplam_islem = table.Column<int>(type: "integer", nullable: false),
                    toplam_risk = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "musteriler",
                columns: table => new
                {
                    musteri_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    soyad = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    tc_kimlik_no = table.Column<string>(type: "character(11)", fixedLength: true, maxLength: 11, nullable: false),
                    sifre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    rol = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValueSql: "'MUSTERI'::character varying"),
                    kayit_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("musteriler_pkey", x => x.musteri_id);
                });

            migrationBuilder.CreateTable(
                name: "risk_masasi",
                columns: table => new
                {
                    risk_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    hesap_id = table.Column<int>(type: "integer", nullable: true),
                    supheli_olay = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    olay_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("risk_masasi_pkey", x => x.risk_id);
                });

            migrationBuilder.CreateTable(
                name: "hesaplar",
                columns: table => new
                {
                    hesap_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    musteri_id = table.Column<int>(type: "integer", nullable: false),
                    hesap_no = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    hesap_turu = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, defaultValue: "VADESIZ"),
                    bakiye = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: true, defaultValueSql: "0.00"),
                    aktif_mi = table.Column<bool>(type: "boolean", nullable: true, defaultValue: true),
                    olusturma_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("hesaplar_pkey", x => x.hesap_id);
                    table.ForeignKey(
                        name: "fk_musteri",
                        column: x => x.musteri_id,
                        principalTable: "musteriler",
                        principalColumn: "musteri_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "islem_hareketleri",
                columns: table => new
                {
                    islem_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    gonderen_hesap_id = table.Column<int>(type: "integer", nullable: true),
                    alici_hesap_id = table.Column<int>(type: "integer", nullable: true),
                    miktar = table.Column<decimal>(type: "numeric(15,2)", precision: 15, scale: 2, nullable: false),
                    aciklama = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    islem_tarihi = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("islem_hareketleri_pkey", x => x.islem_id);
                    table.ForeignKey(
                        name: "fk_alici",
                        column: x => x.alici_hesap_id,
                        principalTable: "hesaplar",
                        principalColumn: "hesap_id");
                    table.ForeignKey(
                        name: "fk_gonderen",
                        column: x => x.gonderen_hesap_id,
                        principalTable: "hesaplar",
                        principalColumn: "hesap_id");
                });

            migrationBuilder.CreateIndex(
                name: "hesaplar_hesap_no_key",
                table: "hesaplar",
                column: "hesap_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_hesaplar_musteri_id",
                table: "hesaplar",
                column: "musteri_id");

            migrationBuilder.CreateIndex(
                name: "IX_islem_hareketleri_alici_hesap_id",
                table: "islem_hareketleri",
                column: "alici_hesap_id");

            migrationBuilder.CreateIndex(
                name: "IX_islem_hareketleri_gonderen_hesap_id",
                table: "islem_hareketleri",
                column: "gonderen_hesap_id");

            migrationBuilder.CreateIndex(
                name: "musteriler_tc_kimlik_no_key",
                table: "musteriler",
                column: "tc_kimlik_no",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenelIstatistikler");

            migrationBuilder.DropTable(
                name: "islem_hareketleri");

            migrationBuilder.DropTable(
                name: "risk_masasi");

            migrationBuilder.DropTable(
                name: "hesaplar");

            migrationBuilder.DropTable(
                name: "musteriler");
        }
    }
}
