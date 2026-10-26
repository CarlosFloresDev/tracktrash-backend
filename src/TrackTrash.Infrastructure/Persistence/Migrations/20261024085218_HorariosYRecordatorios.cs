using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackTrash.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class HorariosYRecordatorios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "rutas_recoleccion",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DistritoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Zona = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Dias = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    HoraLocal = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Demostracion = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rutas_recoleccion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_rutas_recoleccion_distritos_DistritoId",
                        column: x => x.DistritoId,
                        principalSchema: "tracktrash",
                        principalTable: "distritos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reportes_paso",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RutaId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportadoEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reportes_paso", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reportes_paso_rutas_recoleccion_RutaId",
                        column: x => x.RutaId,
                        principalSchema: "tracktrash",
                        principalTable: "rutas_recoleccion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "suscripciones_ruta",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    RutaId = table.Column<Guid>(type: "uuid", nullable: false),
                    MinutosAntes = table.Column<int>(type: "integer", nullable: false),
                    ProximoAviso = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suscripciones_ruta", x => x.Id);
                    table.ForeignKey(
                        name: "FK_suscripciones_ruta_rutas_recoleccion_RutaId",
                        column: x => x.RutaId,
                        principalSchema: "tracktrash",
                        principalTable: "rutas_recoleccion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reportes_paso_RutaId_UsuarioId_ReportadoEn",
                schema: "tracktrash",
                table: "reportes_paso",
                columns: new[] { "RutaId", "UsuarioId", "ReportadoEn" });

            migrationBuilder.CreateIndex(
                name: "IX_rutas_recoleccion_DistritoId",
                schema: "tracktrash",
                table: "rutas_recoleccion",
                column: "DistritoId");

            migrationBuilder.CreateIndex(
                name: "IX_suscripciones_ruta_ProximoAviso",
                schema: "tracktrash",
                table: "suscripciones_ruta",
                column: "ProximoAviso");

            migrationBuilder.CreateIndex(
                name: "IX_suscripciones_ruta_RutaId",
                schema: "tracktrash",
                table: "suscripciones_ruta",
                column: "RutaId");

            migrationBuilder.CreateIndex(
                name: "IX_suscripciones_ruta_UsuarioId_RutaId",
                schema: "tracktrash",
                table: "suscripciones_ruta",
                columns: new[] { "UsuarioId", "RutaId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reportes_paso",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "suscripciones_ruta",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "rutas_recoleccion",
                schema: "tracktrash");
        }
    }
}
