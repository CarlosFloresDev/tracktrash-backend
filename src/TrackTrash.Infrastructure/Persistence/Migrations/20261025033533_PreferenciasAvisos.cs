using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackTrash.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PreferenciasAvisos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AvisosHorarios",
                schema: "tracktrash",
                table: "perfiles",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "AvisosIncidencias",
                schema: "tracktrash",
                table: "perfiles",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "AvisosRecojos",
                schema: "tracktrash",
                table: "perfiles",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "AvisosRecompensas",
                schema: "tracktrash",
                table: "perfiles",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                schema: "tracktrash",
                table: "notificaciones",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "Sistema");

            // Clasificar avisos anteriores sin borrarlos ni cambiar su lectura.
            migrationBuilder.Sql("""
                UPDATE tracktrash.notificaciones
                SET "Tipo" = CASE
                    WHEN "ClaveUnica" LIKE 'horario:%' THEN 'Horarios'
                    WHEN "ClaveUnica" LIKE 'aceptada:%' OR "ClaveUnica" LIKE 'camino:%'
                      OR "ClaveUnica" LIKE 'cancelada:%' OR "ClaveUnica" LIKE 'confirmada:%' THEN 'Recojos'
                    WHEN "ClaveUnica" LIKE 'incidencia:%' OR "ClaveUnica" LIKE 'resuelta:%' THEN 'Incidencias'
                    WHEN "ClaveUnica" LIKE 'canje:%' THEN 'Recompensas'
                    ELSE 'Sistema'
                END;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvisosHorarios",
                schema: "tracktrash",
                table: "perfiles");

            migrationBuilder.DropColumn(
                name: "AvisosIncidencias",
                schema: "tracktrash",
                table: "perfiles");

            migrationBuilder.DropColumn(
                name: "AvisosRecojos",
                schema: "tracktrash",
                table: "perfiles");

            migrationBuilder.DropColumn(
                name: "AvisosRecompensas",
                schema: "tracktrash",
                table: "perfiles");

            migrationBuilder.DropColumn(
                name: "Tipo",
                schema: "tracktrash",
                table: "notificaciones");
        }
    }
}
