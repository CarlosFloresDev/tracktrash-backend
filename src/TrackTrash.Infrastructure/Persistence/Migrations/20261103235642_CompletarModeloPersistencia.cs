using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TrackTrash.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CompletarModeloPersistencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_movimientos_puntos_SolicitudId",
                schema: "tracktrash",
                table: "movimientos_puntos");

            migrationBuilder.DropIndex(
                name: "IX_movimientos_puntos_UsuarioId",
                schema: "tracktrash",
                table: "movimientos_puntos");

            migrationBuilder.AddColumn<Guid>(
                name: "AsociacionId",
                schema: "tracktrash",
                table: "solicitudes_recojo",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitud",
                schema: "tracktrash",
                table: "solicitudes_recojo",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitud",
                schema: "tracktrash",
                table: "solicitudes_recojo",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotivoCancelacion",
                schema: "tracktrash",
                table: "solicitudes_recojo",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true);

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
                    Nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
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
                    Nombre = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_distritos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "eventos_auditoria",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Actor = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Entidad = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Referencia = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Accion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Campos = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreadoEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eventos_auditoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "historico_solicitudes",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SolicitudId = table.Column<Guid>(type: "uuid", nullable: false),
                    ActorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Accion = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreadoEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_historico_solicitudes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_historico_solicitudes_solicitudes_recojo_SolicitudId",
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
                    ClaveUnica = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Titulo = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Mensaje = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreadaEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Leida = table.Column<bool>(type: "boolean", nullable: false),
                    Tipo = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notificaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "perfiles_usuario",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Telefono = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Direccion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Latitud = table.Column<double>(type: "double precision", nullable: true),
                    Longitud = table.Column<double>(type: "double precision", nullable: true),
                    AsociacionId = table.Column<Guid>(type: "uuid", nullable: true),
                    RecibirAvisos = table.Column<bool>(type: "boolean", nullable: false),
                    AvisosHorarios = table.Column<bool>(type: "boolean", nullable: false),
                    AvisosRecojos = table.Column<bool>(type: "boolean", nullable: false),
                    AvisosIncidencias = table.Column<bool>(type: "boolean", nullable: false),
                    AvisosRecompensas = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    UltimoDiaEntrega = table.Column<DateOnly>(type: "date", nullable: true),
                    RachaActual = table.Column<int>(type: "integer", nullable: false),
                    RachaMaxima = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_perfiles_usuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "puntos_acopio",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Direccion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Latitud = table.Column<double>(type: "double precision", nullable: false),
                    Longitud = table.Column<double>(type: "double precision", nullable: false),
                    Demostracion = table.Column<bool>(type: "boolean", nullable: false)
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
                    Codigo = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
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
                name: "asociaciones_reciclaje",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DistritoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Direccion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Horario = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Latitud = table.Column<double>(type: "double precision", nullable: false),
                    Longitud = table.Column<double>(type: "double precision", nullable: false),
                    CapacidadKg = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    ReservadoKg = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    Verificada = table.Column<bool>(type: "boolean", nullable: false),
                    Demostracion = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_asociaciones_reciclaje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_asociaciones_reciclaje_distritos_DistritoId",
                        column: x => x.DistritoId,
                        principalSchema: "tracktrash",
                        principalTable: "distritos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "contenedores",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DistritoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Latitud = table.Column<double>(type: "double precision", nullable: false),
                    Longitud = table.Column<double>(type: "double precision", nullable: false),
                    Porcentaje = table.Column<int>(type: "integer", nullable: true),
                    ActualizadoEn = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Fuente = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Demostracion = table.Column<bool>(type: "boolean", nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false)
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rutas_recoleccion",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DistritoId = table.Column<Guid>(type: "uuid", nullable: false),
                    Nombre = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    Zona = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Dias = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
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
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "materiales_aceptados",
                schema: "tracktrash",
                columns: table => new
                {
                    AsociacionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Material = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materiales_aceptados", x => new { x.AsociacionId, x.Material });
                    table.ForeignKey(
                        name: "FK_materiales_aceptados_asociaciones_reciclaje_AsociacionId",
                        column: x => x.AsociacionId,
                        principalSchema: "tracktrash",
                        principalTable: "asociaciones_reciclaje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dispositivos_iot",
                schema: "tracktrash",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContenedorId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaveHash = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
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
                    Tipo = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Estado = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ResponsableId = table.Column<Guid>(type: "uuid", nullable: true),
                    Resolucion = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
                });

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

            migrationBuilder.CreateTable(
                name: "reportes_paso_camion",
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
                    table.PrimaryKey("PK_reportes_paso_camion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_reportes_paso_camion_rutas_recoleccion_RutaId",
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
                unique: true,
                filter: "\"CanjeId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_movimientos_puntos_SolicitudId",
                schema: "tracktrash",
                table: "movimientos_puntos",
                column: "SolicitudId",
                unique: true,
                filter: "\"SolicitudId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_movimientos_puntos_UsuarioId_CreadoEn",
                schema: "tracktrash",
                table: "movimientos_puntos",
                columns: new[] { "UsuarioId", "CreadoEn" });

            migrationBuilder.CreateIndex(
                name: "IX_asociaciones_reciclaje_DistritoId",
                schema: "tracktrash",
                table: "asociaciones_reciclaje",
                column: "DistritoId");

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
                name: "IX_canjes_UsuarioId_CreadoEn",
                schema: "tracktrash",
                table: "canjes",
                columns: new[] { "UsuarioId", "CreadoEn" });

            migrationBuilder.CreateIndex(
                name: "IX_contenedores_DistritoId",
                schema: "tracktrash",
                table: "contenedores",
                column: "DistritoId");

            migrationBuilder.CreateIndex(
                name: "IX_dispositivos_iot_ContenedorId",
                schema: "tracktrash",
                table: "dispositivos_iot",
                column: "ContenedorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_distritos_Nombre",
                schema: "tracktrash",
                table: "distritos",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_eventos_auditoria_Entidad_Referencia_CreadoEn",
                schema: "tracktrash",
                table: "eventos_auditoria",
                columns: new[] { "Entidad", "Referencia", "CreadoEn" });

            migrationBuilder.CreateIndex(
                name: "IX_historico_solicitudes_SolicitudId_CreadoEn",
                schema: "tracktrash",
                table: "historico_solicitudes",
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
                name: "IX_reportes_ciudadanos_ContenedorId_UsuarioId_CreadoEn",
                schema: "tracktrash",
                table: "reportes_ciudadanos",
                columns: new[] { "ContenedorId", "UsuarioId", "CreadoEn" });

            migrationBuilder.CreateIndex(
                name: "IX_reportes_paso_camion_RutaId_UsuarioId_ReportadoEn",
                schema: "tracktrash",
                table: "reportes_paso_camion",
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
                name: "IX_suscripciones_ruta_RutaId_UsuarioId",
                schema: "tracktrash",
                table: "suscripciones_ruta",
                columns: new[] { "RutaId", "UsuarioId" },
                unique: true);

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
                name: "eventos_auditoria",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "historico_solicitudes",
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
                name: "perfiles_usuario",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "puntos_acopio",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "reportes_ciudadanos",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "reportes_paso_camion",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "suscripciones_ruta",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "beneficios",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "dispositivos_iot",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "asociaciones_reciclaje",
                schema: "tracktrash");

            migrationBuilder.DropTable(
                name: "rutas_recoleccion",
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

            migrationBuilder.DropIndex(
                name: "IX_movimientos_puntos_SolicitudId",
                schema: "tracktrash",
                table: "movimientos_puntos");

            migrationBuilder.DropIndex(
                name: "IX_movimientos_puntos_UsuarioId_CreadoEn",
                schema: "tracktrash",
                table: "movimientos_puntos");

            migrationBuilder.DropColumn(
                name: "AsociacionId",
                schema: "tracktrash",
                table: "solicitudes_recojo");

            migrationBuilder.DropColumn(
                name: "Latitud",
                schema: "tracktrash",
                table: "solicitudes_recojo");

            migrationBuilder.DropColumn(
                name: "Longitud",
                schema: "tracktrash",
                table: "solicitudes_recojo");

            migrationBuilder.DropColumn(
                name: "MotivoCancelacion",
                schema: "tracktrash",
                table: "solicitudes_recojo");

            migrationBuilder.DropColumn(
                name: "CanjeId",
                schema: "tracktrash",
                table: "movimientos_puntos");

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
        }
    }
}
