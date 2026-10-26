using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.Rutas;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Infrastructure.Persistence;

namespace TrackTrash.Tests;

public sealed partial class ApiFlowTests
{
    [Fact]
    public async Task Migracion_conserva_avisos_anteriores_y_su_estado_de_lectura()
    {
        // Solo se cambia la base temporal exclusiva de esta prueba.
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TrackTrashDbContext>();
        var usuario = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var previo = Notificacion.Crear(usuario, "aceptada:anterior", "Aviso anterior", "Contenido conservado", DateTimeOffset.UtcNow);
        previo.Leer();
        db.Add(previo);
        var perfil = await db.Perfiles.SingleAsync(p => p.Id == usuario);
        perfil.Actualizar("Vecino", "", "", null, null, false);
        await db.SaveChangesAsync();
        var migrador = db.GetService<IMigrator>();
        await migrador.MigrateAsync("20261024090251_AuditoriaOperaciones");
        await migrador.MigrateAsync();
        db.ChangeTracker.Clear();
        var conservado = await db.Notificaciones.SingleAsync(n => n.Id == previo.Id);
        Assert.True(conservado.Leida);
        Assert.Equal(TipoAviso.Recojos, conservado.Tipo);
        Assert.Equal("Contenido conservado", conservado.Mensaje);
        perfil = await db.Perfiles.SingleAsync(p => p.Id == usuario);
        Assert.False(perfil.RecibirAvisos);
        Assert.True(perfil.AvisosHorarios);
        Assert.True(perfil.AvisosRecojos);
    }

    [Fact]
    public async Task Preferencias_se_guardan_por_usuario_y_clientes_antiguos_no_las_reinician()
    {
        using var vecino = Client("demo-vecino");
        using var otro = Client("demo-vecino-2");
        var opciones = new PreferenciasAvisosDto(Horarios: false, Recojos: false);
        (await vecino.PutAsJsonAsync("/api/v1/perfil", new ActualizarPerfilRequest("Vecino prueba", "", "", null, null, true, opciones))).EnsureSuccessStatusCode();
        var guardado = (await vecino.GetFromJsonAsync<PerfilDto>("/api/v1/perfil"))!;
        Assert.Equal(opciones, guardado.PreferenciasAvisos);
        Assert.Equal(new PreferenciasAvisosDto(), (await otro.GetFromJsonAsync<PerfilDto>("/api/v1/perfil"))!.PreferenciasAvisos);

        // Cliente 0.2 que todavía no envía preferenciasAvisos.
        (await vecino.PutAsJsonAsync("/api/v1/perfil", new { nombre = "Nombre actualizado", telefono = "", direccion = "", recibirAvisos = true })).EnsureSuccessStatusCode();
        Assert.Equal(opciones, (await vecino.GetFromJsonAsync<PerfilDto>("/api/v1/perfil"))!.PreferenciasAvisos);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task Silenciar_avisos_no_bloquea_entregas_puntos_canjes_ni_reportes(bool maestro)
    {
        using var vecino = Client("demo-vecino");
        using var reciclador = Client("demo-reciclador");
        var opciones = maestro ? new PreferenciasAvisosDto(false, false, false, false) : new PreferenciasAvisosDto();
        (await vecino.PutAsJsonAsync("/api/v1/perfil", new ActualizarPerfilRequest("Vecino prueba", "", "", null, null, maestro, opciones))).EnsureSuccessStatusCode();
        var solicitud = await Crear(vecino);
        (await reciclador.PostAsync($"/api/v1/solicitudes/{solicitud.Id}/aceptacion", null)).EnsureSuccessStatusCode();
        (await reciclador.PostAsync($"/api/v1/solicitudes/{solicitud.Id}/viaje", null)).EnsureSuccessStatusCode();
        (await reciclador.PostAsJsonAsync($"/api/v1/solicitudes/{solicitud.Id}/confirmacion", new ConfirmarEntregaRequest(solicitud.CodigoConfirmacion!))).EnsureSuccessStatusCode();
        Assert.Equal(20, (await vecino.GetFromJsonAsync<ImpactoDto>("/api/v1/perfil/impacto"))!.Puntos);
        var beneficio = (await vecino.GetFromJsonAsync<List<BeneficioDto>>("/api/v1/beneficios"))!.First(b => b.CostoPuntos == 20);
        (await vecino.PostAsJsonAsync("/api/v1/canjes", new CanjearRequest(beneficio.Id, Guid.NewGuid()))).EnsureSuccessStatusCode();
        var mapa = (await vecino.GetFromJsonAsync<MapaDto>("/api/v1/mapa"))!;
        (await vecino.PostAsJsonAsync("/api/v1/incidencias", new CrearIncidenciaRequest(mapa.Contenedores[0].Id, "Tacho lleno", "Reporte de prueba sin notificaciones"))).EnsureSuccessStatusCode();
        var avisos = (await vecino.GetFromJsonAsync<List<NotificacionDto>>("/api/v1/notificaciones"))!;
        Assert.All(avisos, n => Assert.Equal("Sistema", n.Tipo)); // Bienvenida previa conservada.
        Assert.Equal(0, (await vecino.GetFromJsonAsync<ImpactoDto>("/api/v1/perfil/impacto"))!.Puntos);
        Assert.NotEmpty((await vecino.GetFromJsonAsync<List<IncidenciaDto>>("/api/v1/incidencias"))!);
    }

    [Fact]
    public async Task Tipos_habilitados_siguen_llegando_y_el_historial_previo_no_se_oculta()
    {
        using var vecino = Client("demo-vecino");
        using var reciclador = Client("demo-reciclador");
        var solicitud = await Crear(vecino);
        (await reciclador.PostAsync($"/api/v1/solicitudes/{solicitud.Id}/aceptacion", null)).EnsureSuccessStatusCode();
        (await vecino.PutAsJsonAsync("/api/v1/perfil", new ActualizarPerfilRequest("Vecino prueba", "", "", null, null, true, new(Recojos: false)))).EnsureSuccessStatusCode();
        (await reciclador.PostAsync($"/api/v1/solicitudes/{solicitud.Id}/viaje", null)).EnsureSuccessStatusCode();
        var mapa = (await vecino.GetFromJsonAsync<MapaDto>("/api/v1/mapa"))!;
        (await vecino.PostAsJsonAsync("/api/v1/incidencias", new CrearIncidenciaRequest(mapa.Contenedores[0].Id, "Tacho lleno", "Otro tipo de aviso debe seguir habilitado"))).EnsureSuccessStatusCode();
        var avisos = (await vecino.GetFromJsonAsync<List<NotificacionDto>>("/api/v1/notificaciones"))!;
        Assert.Single(avisos, n => n.Tipo == "Recojos");
        Assert.Single(avisos, n => n.Tipo == "Incidencias");
        Assert.DoesNotContain(avisos, n => n.Titulo == "El reciclador está en camino");
    }

    [Fact]
    public async Task Antelacion_se_actualiza_sin_duplicar_y_valida_incluso_una_suscripcion_existente()
    {
        using var vecino = Client("demo-vecino");
        using var otro = Client("demo-vecino-2");
        var ruta = (await vecino.GetFromJsonAsync<List<RutaDto>>("/api/v1/rutas"))![0];
        foreach (var minutos in new[] { 20, 35, 35 })
            (await vecino.PostAsJsonAsync($"/api/v1/rutas/{ruta.Id}/suscripcion", new SuscribirRutaRequest(minutos))).EnsureSuccessStatusCode();
        Assert.Equal(35, (await vecino.GetFromJsonAsync<List<RutaDto>>("/api/v1/rutas"))!.Single(r => r.Id == ruta.Id).MinutosAntes);
        Assert.Null((await otro.GetFromJsonAsync<List<RutaDto>>("/api/v1/rutas"))!.Single(r => r.Id == ruta.Id).MinutosAntes);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, (await vecino.PostAsJsonAsync($"/api/v1/rutas/{ruta.Id}/suscripcion", new SuscribirRutaRequest(100))).StatusCode);
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TrackTrashDbContext>();
        var sub = await db.Set<SuscripcionRuta>().SingleAsync();
        Assert.Equal(35, sub.MinutosAntes);
        Assert.Equal(2, sub.Version); // Crear y cambiar; reintentar no reprograma.
    }

    [Fact]
    public async Task Worker_respeta_tipo_horarios_y_avanza_vencimientos_aunque_el_usuario_lo_silencie()
    {
        var ahora = new DateTimeOffset(2026, 10, 26, 23, 45, 0, TimeSpan.Zero);
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TrackTrashDbContext>();
        var distrito = await db.Distritos.FirstAsync();
        var ruta = RutaRecoleccion.Crear(distrito.Id, "Prueba de recordatorio", "Zona prueba", [1], new TimeOnly(19, 0));
        var usuarios = new[] { Guid.Parse("11111111-1111-1111-1111-111111111111"), Guid.Parse("33333333-3333-3333-3333-333333333333") };
        db.Add(ruta);
        var perfil = await db.Perfiles.SingleAsync(p => p.Id == usuarios[0]);
        perfil.ActualizarPreferenciasAvisos(false, true, true, true);
        foreach (var id in usuarios) db.Add(SuscripcionRuta.Crear(id, ruta, 20, ahora.AddHours(-1)));
        await db.SaveChangesAsync();
        var worker = new ProcesarRecordatoriosUseCase(scope.ServiceProvider.GetRequiredService<IRutaRepository>(),
            scope.ServiceProvider.GetRequiredService<INotificadorUsuarioService>(), scope.ServiceProvider.GetRequiredService<IUnitOfWork>(), new RelojAvisos(ahora));
        await worker.ExecuteAsync(default);
        await worker.ExecuteAsync(default);
        var aviso = Assert.Single(await db.Notificaciones.Where(n => n.Tipo == TipoAviso.Horarios).ToListAsync());
        Assert.Equal(usuarios[1], aviso.UsuarioId);
        Assert.All(await db.Set<SuscripcionRuta>().ToListAsync(), s => Assert.True(s.ProximoAviso > ahora));
    }

    private sealed class RelojAvisos(DateTimeOffset ahora) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => ahora;
    }
}

public sealed class PreferenciasAvisosDomainTests
{
    [Theory]
    [InlineData(TipoAviso.Horarios)]
    [InlineData(TipoAviso.Recojos)]
    [InlineData(TipoAviso.Incidencias)]
    [InlineData(TipoAviso.Recompensas)]
    public void Pausa_global_preserva_seleccion_individual(TipoAviso tipo)
    {
        var p = PerfilUsuario.Crear(Guid.NewGuid());
        Assert.True(p.PermiteAviso(tipo));
        p.Actualizar("Vecino", "", "", null, null, false);
        Assert.False(p.PermiteAviso(tipo));
        p.Actualizar("Vecino", "", "", null, null, true);
        Assert.True(p.PermiteAviso(tipo));
        p.ActualizarPreferenciasAvisos(false, false, false, false);
        Assert.False(p.PermiteAviso(tipo));
    }
}
