using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace TrackTrash.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EcosistemaCircular : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.AlterColumn<Guid>(
                name: "SolicitudId",
                schema: "tracktrash",
                table: "movimientos_puntos",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "CanjeId",
                schema: "tracktrash",
                table: "movimientos_puntos",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "beneficios",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    CostoPuntos = table.Column<int>(type: "integer", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    Activo = table.Column<bool>(type: "boolean", nullable: false),
                    Demostracion = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_beneficios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "distritos",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_distritos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "historial_solicitudes",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SolicitudId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Accion = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    CreadoEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historial_solicitudes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_historial_solicitudes_solicitudes_recojo_SolicitudId",
                        column: x => x.SolicitudId,
                        principalSchema: "tracktrash",
                        principalTable: "solicitudes_recojo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notificaciones",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaveUnica = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Mensaje = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    CreadaEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Leida = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notificaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "puntos_acopio",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Direccion = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Latitud = table.Column<double>(type: "double precision", nullable: false),
                    Longitud = table.Column<double>(type: "double precision", nullable: false),
                    Demostracion = table.Column<bool>(type: "boolean", nullable: false),
                    Ubicacion = table.Column<Point>(type: "geography (point,4326)", nullable: true, computedColumnSql: "ST_SetSRID(ST_MakePoint(\"Longitud\", \"Latitud\"),4326)::geography", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_puntos_acopio", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "canjes",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UsuarioId = table.Column<Guid>(type: "uuid", nullable: false),
                    BeneficioId = table.Column<Guid>(type: "uuid", nullable: false),
                    Clave = table.Column<Guid>(type: "uuid", nullable: false),
                    PuntosUsados = table.Column<int>(type: "integer", nullable: false),
                    Codigo = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    CreadoEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_canjes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_canjes_beneficios_BeneficioId",
                        column: x => x.BeneficioId,
                        principalSchema: "tracktrash",
                        principalTable: "beneficios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "asociaciones",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DistritoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Direccion = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Horario = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Latitud = table.Column<double>(type: "double precision", nullable: false),
                    Longitud = table.Column<double>(type: "double precision", nullable: false),
                    CapacidadKg = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    Verificada = table.Column<bool>(type: "boolean", nullable: false),
                    Demostracion = table.Column<bool>(type: "boolean", nullable: false),
                    Ubicacion = table.Column<Point>(type: "geography (point,4326)", nullable: true, computedColumnSql: "ST_SetSRID(ST_MakePoint(\"Longitud\", \"Latitud\"),4326)::geography", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asociaciones", x => x.Id);
                    table.ForeignKey(
                        name: "FK_asociaciones_distritos_DistritoId",
                        column: x => x.DistritoId,
                        principalSchema: "tracktrash",
                        principalTable: "distritos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "contenedores",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DistritoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Latitud = table.Column<double>(type: "double precision", nullable: false),
                    Longitud = table.Column<double>(type: "double precision", nullable: false),
                    Porcentaje = table.Column<int>(type: "integer", nullable: true),
                    ActualizadoEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Fuente = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Demostracion = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Ubicacion = table.Column<Point>(type: "geography (point,4326)", nullable: true, computedColumnSql: "ST_SetSRID(ST_MakePoint(\"Longitud\", \"Latitud\"),4326)::geography", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contenedores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_contenedores_distritos_DistritoId",
                        column: x => x.DistritoId,
                        principalSchema: "tracktrash",
                        principalTable: "distritos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "materiales_aceptados",
                schema: "tracktrash",
                columns: table => new
                {
                    AsociacionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Material = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materiales_aceptados", x => new { x.AsociacionId, x.Material });
                    table.ForeignKey(
                        name: "FK_materiales_aceptados_asociaciones_AsociacionId",
                        column: x => x.AsociacionId,
                        principalSchema: "tracktrash",
                        principalTable: "asociaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "perfiles",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Telefono = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Direccion = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Latitud = table.Column<double>(type: "double precision", nullable: true),
                    Longitud = table.Column<double>(type: "double precision", nullable: true),
                    AsociacionId = table.Column<Guid>(type: "uuid", nullable: true),
                    RecibirAvisos = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_perfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_perfiles_asociaciones_AsociacionId",
                        column: x => x.AsociacionId,
                        principalSchema: "tracktrash",
                        principalTable: "asociaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "dispositivos_iot",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContenedorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaveHash = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    AlturaCm = table.Column<double>(type: "double precision", nullable: false),
                    UltimoContacto = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Bateria = table.Column<int>(type: "integer", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dispositivos_iot", x => x.Id);
                    table.ForeignKey(
                        name: "FK_dispositivos_iot_contenedores_ContenedorId",
                        column: x => x.ContenedorId,
                        principalSchema: "tracktrash",
                        principalTable: "contenedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "incidencias",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContenedorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReportadaPor = table.Column<Guid>(type: "uuid", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    ResponsableId = table.Column<Guid>(type: "uuid", nullable: true),
                    Resolucion = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    CreadaEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ResueltaEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Version = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_incidencias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_incidencias_contenedores_ContenedorId",
                        column: x => x.ContenedorId,
                        principalSchema: "tracktrash",
                        principalTable: "contenedores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lecturas_nivel",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DispositivoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Secuencia = table.Column<long>(type: "bigint", nullable: false),
                    Porcentaje = table.Column<int>(type: "integer", nullable: false),
                    Bateria = table.Column<int>(type: "integer", nullable: false),
                    MedidaEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    RecibidaEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lecturas_nivel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lecturas_nivel_dispositivos_iot_DispositivoId",
                        column: x => x.DispositivoId,
                        principalSchema: "tracktrash",
                        principalTable: "dispositivos_iot",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_movimientos_puntos_CanjeId",
                schema: "tracktrash",
                table: "movimientos_puntos",
                column: "CanjeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_asociaciones_DistritoId",
                schema: "tracktrash",
                table: "asociaciones",
                column: "DistritoId");

            migrationBuilder.CreateIndex(
                name: "IX_asociaciones_Ubicacion",
                schema: "tracktrash",
                table: "asociaciones",
                column: "Ubicacion")
                .Annotation("Npgsql:IndexMethod", "gist");

            migrationBuilder.CreateIndex(
                name: "IX_canjes_BeneficioId",
                schema: "tracktrash",
                table: "canjes",
                column: "BeneficioId");

            migrationBuilder.CreateIndex(
                name: "IX_canjes_UsuarioId_Clave",
                schema: "tracktrash",
                table: "canjes",
                columns: new[] { "UsuarioId", "Clave" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_contenedores_DistritoId",
                schema: "tracktrash",
                table: "contenedores",
                column: "DistritoId");

            migrationBuilder.CreateIndex(
                name: "IX_contenedores_Ubicacion",
                schema: "tracktrash",
                table: "contenedores",
                column: "Ubicacion")
                .Annotation("Npgsql:IndexMethod", "gist");

            migrationBuilder.CreateIndex(
                name: "IX_dispositivos_iot_ContenedorId",
                schema: "tracktrash",
                table: "dispositivos_iot",
                column: "ContenedorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_historial_solicitudes_SolicitudId_CreadoEn",
                schema: "tracktrash",
                table: "historial_solicitudes",
                columns: new[] { "SolicitudId", "CreadoEn" });

            migrationBuilder.CreateIndex(
                name: "IX_incidencias_ContenedorId_Estado",
                schema: "tracktrash",
                table: "incidencias",
                columns: new[] { "ContenedorId", "Estado" });

            migrationBuilder.CreateIndex(
                name: "IX_lecturas_nivel_DispositivoId_MedidaEn",
                schema: "tracktrash",
                table: "lecturas_nivel",
                columns: new[] { "DispositivoId", "MedidaEn" });

            migrationBuilder.CreateIndex(
                name: "IX_lecturas_nivel_DispositivoId_Secuencia",
                schema: "tracktrash",
                table: "lecturas_nivel",
                columns: new[] { "DispositivoId", "Secuencia" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notificaciones_UsuarioId_ClaveUnica",
                schema: "tracktrash",
                table: "notificaciones",
                columns: new[] { "UsuarioId", "ClaveUnica" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notificaciones_UsuarioId_CreadaEn",
                schema: "tracktrash",
                table: "notificaciones",
                columns: new[] { "UsuarioId", "CreadaEn" });

            migrationBuilder.CreateIndex(
                name: "IX_perfiles_AsociacionId",
                schema: "tracktrash",
                table: "perfiles",
                column: "AsociacionId");

            migrationBuilder.CreateIndex(
                name: "IX_puntos_acopio_Ubicacion",
                schema: "tracktrash",
                table: "puntos_acopio",
                column: "Ubicacion")
                .Annotation("Npgsql:IndexMethod", "gist");

            migrationBuilder.AddForeignKey(
                name: "FK_movimientos_puntos_canjes_CanjeId",
                schema: "tracktrash",
                table: "movimientos_puntos",
                column: "CanjeId",
                principalSchema: "tracktrash",
                principalTable: "canjes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_movimientos_puntos_canjes_CanjeId",
                schema: "tracktrash",
                table: "movimientos_puntos");

            migrationBuilder.DropTable(
                name: "canjes",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "historial_solicitudes",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "incidencias",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "lecturas_nivel",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "materiales_aceptados",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "notificaciones",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "perfiles",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "puntos_acopio",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "beneficios",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "dispositivos_iot",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "asociaciones",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "contenedores",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "distritos",
                schema: "tracktrash");

            migrationBuilder.DropIndex(
                name: "IX_movimientos_puntos_CanjeId",
                schema: "tracktrash",
                table: "movimientos_puntos");

            migrationBuilder.DropColumn(
                name: "CanjeId",
                schema: "tracktrash",
                table: "movimientos_puntos");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.AlterColumn<Guid>(
                name: "SolicitudId",
                schema: "tracktrash",
                table: "movimientos_puntos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
