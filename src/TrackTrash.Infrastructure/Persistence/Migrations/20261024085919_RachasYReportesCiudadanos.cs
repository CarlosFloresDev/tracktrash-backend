using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackTrash.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RachasYReportesCiudadanos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RachaActual",
                schema: "tracktrash",
                table: "perfiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RachaMaxima",
                schema: "tracktrash",
                table: "perfiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "UltimoDiaEntrega",
                schema: "tracktrash",
                table: "perfiles",
                type: "date",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "reportes_ciudadanos",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContenedorId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    NivelEstimado = table.Column<int>(type: "integer", nullable: false),
                    CreadoEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reportes_ciudadanos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reportes_ciudadanos_contenedores_ContenedorId",
                        column: x => x.ContenedorId,
                        principalSchema: "tracktrash",
                        principalTable: "contenedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reportes_ciudadanos_ContenedorId_UsuarioId_CreadoEn",
                schema: "tracktrash",
                table: "reportes_ciudadanos",
                columns: new[] { "ContenedorId", "UsuarioId", "CreadoEn" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reportes_ciudadanos",
                schema: "tracktrash");

            migrationBuilder.DropColumn(
                name: "RachaActual",
                schema: "tracktrash",
                table: "perfiles");

            migrationBuilder.DropColumn(
                name: "RachaMaxima",
                schema: "tracktrash",
                table: "perfiles");

            migrationBuilder.DropColumn(
                name: "UltimoDiaEntrega",
                schema: "tracktrash",
                table: "perfiles");
        }
    }
}
