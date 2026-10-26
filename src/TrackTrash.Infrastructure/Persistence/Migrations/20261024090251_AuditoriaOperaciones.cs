using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackTrash.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AuditoriaOperaciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "eventos_auditoria",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Actor = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Entidad = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Referencia = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Accion = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Campos = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    CreadoEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eventos_auditoria", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_eventos_auditoria_Entidad_CreadoEn",
                schema: "tracktrash",
                table: "eventos_auditoria",
                columns: new[] { "Entidad", "CreadoEn" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "eventos_auditoria",
                schema: "tracktrash");
        }
    }
}
