using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackTrash.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CrearSolicitudesYPuntos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "tracktrash");

            migrationBuilder.CreateTable(
                name: "solicitudes_recojo",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VecinoId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecicladorId = table.Column<Guid>(type: "uuid", nullable: true),
                    ClaveSolicitud = table.Column<Guid>(type: "uuid", nullable: false),
                    Material = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CantidadEstimadaKg = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Direccion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    ProgramadaPara = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreadaEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ConfirmadaEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    CodigoConfirmacion = table.Column<string>(type: "character varying(6)", maxLength: 6, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_solicitudes_recojo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "movimientos_puntos",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    SolicitudId = table.Column<Guid>(type: "uuid", nullable: false),
                    Puntos = table.Column<int>(type: "integer", nullable: false),
                    CreadoEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movimientos_puntos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_movimientos_puntos_solicitudes_recojo_SolicitudId",
                        column: x => x.SolicitudId,
                        principalSchema: "tracktrash",
                        principalTable: "solicitudes_recojo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_movimientos_puntos_SolicitudId",
                schema: "tracktrash",
                table: "movimientos_puntos",
                column: "SolicitudId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_movimientos_puntos_UsuarioId",
                schema: "tracktrash",
                table: "movimientos_puntos",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_solicitudes_recojo_Estado_CreadaEn",
                schema: "tracktrash",
                table: "solicitudes_recojo",
                columns: new[] { "Estado", "CreadaEn" });

            migrationBuilder.CreateIndex(
                name: "IX_solicitudes_recojo_RecicladorId_CreadaEn",
                schema: "tracktrash",
                table: "solicitudes_recojo",
                columns: new[] { "RecicladorId", "CreadaEn" });

            migrationBuilder.CreateIndex(
                name: "IX_solicitudes_recojo_VecinoId_ClaveSolicitud",
                schema: "tracktrash",
                table: "solicitudes_recojo",
                columns: new[] { "VecinoId", "ClaveSolicitud" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "movimientos_puntos",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "solicitudes_recojo",
                schema: "tracktrash");
        }
    }
}
