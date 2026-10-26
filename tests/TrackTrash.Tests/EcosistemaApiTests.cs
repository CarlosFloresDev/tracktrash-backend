using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Domain.Entities;
using TrackTrash.Infrastructure.Persistence;
namespace TrackTrash.Tests;

public sealed partial class ApiFlowTests
{
    [Fact]
    public async Task Mapa_filtra_por_distancia_y_no_simula_ubicaciones_de_usuarios()
    {
        using var vecino = Client("demo-vecino");
        var mapa = await vecino.GetFromJsonAsync<MapaDto>("/api/v1/mapa?lat=-16.3988&lng=-71.5369&radioKm=10");
        Assert.Equal(4, mapa!.Contenedores.Count); Assert.Equal(2, mapa.Asociaciones.Count);
        Assert.All(mapa.Contenedores, c => Assert.True(c.Demostracion));
        var lejos = await vecino.GetFromJsonAsync<MapaDto>("/api/v1/mapa?lat=0&lng=0&radioKm=1"); Assert.Empty(lejos!.Contenedores);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, (await vecino.GetAsync("/api/v1/mapa?radioKm=1000")).StatusCode);
    }
    [Fact]
    public async Task Asignacion_busca_material_y_capacidad_y_reserva_hasta_cancelar()
    {
        using var vecino = Client("demo-vecino"); using var r = Client("demo-reciclador");
        var req = new CrearSolicitudRequest(Guid.NewGuid(), MaterialReciclable.Vidrio, 2, "Dirección de prueba", DateTimeOffset.UtcNow.AddDays(1), -16.3988, -71.5369);
        var res = await vecino.PostAsJsonAsync("/api/v1/solicitudes", req); res.EnsureSuccessStatusCode(); var s = (await res.Content.ReadFromJsonAsync<SolicitudDto>())!;
        (await vecino.PostAsync($"/api/v1/solicitudes/{s.Id}/asignacion", null)).EnsureSuccessStatusCode();
        (await r.PostAsync($"/api/v1/solicitudes/{s.Id}/aceptacion", null)).EnsureSuccessStatusCode();
        using (var scope = _factory.Services.CreateScope()) Assert.Equal(2, (await scope.ServiceProvider.GetRequiredService<TrackTrashDbContext>().Asociaciones.SingleAsync(a => a.Id == DemoSeed.AsociacionPrincipal)).ReservadoKg);
        (await vecino.PostAsJsonAsync($"/api/v1/solicitudes/{s.Id}/cancelacion", new CancelarSolicitudRequest("Cambio de disponibilidad"))).EnsureSuccessStatusCode();
        using (var scope = _factory.Services.CreateScope()) Assert.Equal(0, (await scope.ServiceProvider.GetRequiredService<TrackTrashDbContext>().Asociaciones.SingleAsync(a => a.Id == DemoSeed.AsociacionPrincipal)).ReservadoKg);
        var h = await vecino.GetFromJsonAsync<List<HistoricoDto>>($"/api/v1/solicitudes/{s.Id}/historial"); Assert.Equal(4, h!.Count);
    }
    [Fact]
    public async Task Incidencia_solo_puede_ser_resuelta_por_operador_y_notifica_al_vecino()
    {
        using var vecino = Client("demo-vecino"); using var admin = Client("demo-admin"); using var otro = Client("demo-vecino-2");
        var mapa = (await vecino.GetFromJsonAsync<MapaDto>("/api/v1/mapa"))!;
        var result = await vecino.PostAsJsonAsync("/api/v1/incidencias", new CrearIncidenciaRequest(mapa.Contenedores[0].Id, "Tacho dañado", "La tapa está rota y requiere reparación.")); result.EnsureSuccessStatusCode();
        var i = (await result.Content.ReadFromJsonAsync<IncidenciaDto>())!;
        Assert.Empty((await otro.GetFromJsonAsync<List<IncidenciaDto>>("/api/v1/incidencias"))!);
        Assert.Equal(HttpStatusCode.Forbidden, (await vecino.PostAsync($"/api/v1/incidencias/{i.Id}/atencion", null)).StatusCode);
        (await admin.PostAsync($"/api/v1/incidencias/{i.Id}/atencion", null)).EnsureSuccessStatusCode();
        (await admin.PostAsJsonAsync($"/api/v1/incidencias/{i.Id}/resolucion", new ResolverIncidenciaRequest("Tapa reparada por el equipo de prueba."))).EnsureSuccessStatusCode();
        Assert.Equal("Resuelta", (await vecino.GetFromJsonAsync<List<IncidenciaDto>>("/api/v1/incidencias"))!.Single().Estado);
        Assert.Contains((await vecino.GetFromJsonAsync<List<NotificacionDto>>("/api/v1/notificaciones"))!, n => n.Titulo == "Tu reporte fue atendido");
    }
    [Fact]
    public async Task Telemetria_autentica_dispositivo_usa_mediana_y_deduplica_alertas()
    {
        using var admin = Client("demo-admin"); using var sensor = Client(null);
        var c = (await admin.GetFromJsonAsync<MapaDto>("/api/v1/mapa"))!.Contenedores[0];
        var registered = await admin.PostAsJsonAsync("/api/v1/iot/dispositivos", new RegistrarDispositivoRequest(c.Id, 100)); registered.EnsureSuccessStatusCode();
        var d = (await registered.Content.ReadFromJsonAsync<DispositivoRegistradoDto>())!;
        var url = $"/api/v1/iot/dispositivos/{d.Id}/lecturas"; var lectura = new LecturaRequest(1, [2, 3, 90], 75, DateTimeOffset.UtcNow.AddSeconds(1));
        Assert.Equal(HttpStatusCode.Forbidden, (await sensor.PostAsJsonAsync(url, lectura)).StatusCode);
        sensor.DefaultRequestHeaders.Add("X-Device-Key", d.Clave);
        var first = await sensor.PostAsJsonAsync(url, lectura); first.EnsureSuccessStatusCode(); Assert.Equal(97, (await first.Content.ReadFromJsonAsync<LecturaResultadoDto>())!.Porcentaje);
        var duplicate = await sensor.PostAsJsonAsync(url, lectura); Assert.True((await duplicate.Content.ReadFromJsonAsync<LecturaResultadoDto>())!.Duplicada);
        (await sensor.PostAsJsonAsync(url, lectura with { Secuencia = 2, MedidaEn = DateTimeOffset.UtcNow.AddSeconds(2) })).EnsureSuccessStatusCode();
        using var scope = _factory.Services.CreateScope(); var db = scope.ServiceProvider.GetRequiredService<TrackTrashDbContext>();
        Assert.Equal(2, await db.Lecturas.CountAsync()); Assert.Equal(1, await db.Incidencias.CountAsync(x => x.Tipo == "Saturación"));
    }
    [Fact]
    public async Task Canje_descontado_una_vez_y_sin_saldo_negativo()
    {
        using var vecino = Client("demo-vecino"); using var r = Client("demo-reciclador"); var s = await Crear(vecino);
        (await r.PostAsync($"/api/v1/solicitudes/{s.Id}/aceptacion", null)).EnsureSuccessStatusCode();
        (await r.PostAsJsonAsync($"/api/v1/solicitudes/{s.Id}/confirmacion", new ConfirmarEntregaRequest(s.CodigoConfirmacion!))).EnsureSuccessStatusCode();
        var b = (await vecino.GetFromJsonAsync<List<BeneficioDto>>("/api/v1/beneficios"))!.First(x => x.CostoPuntos == 20);
        var req = new CanjearRequest(b.Id, Guid.NewGuid());
        var first = await vecino.PostAsJsonAsync("/api/v1/canjes", req); first.EnsureSuccessStatusCode();
        var repeated = await vecino.PostAsJsonAsync("/api/v1/canjes", req); repeated.EnsureSuccessStatusCode();
        Assert.Equal((await first.Content.ReadFromJsonAsync<CanjeDto>())!.Id, (await repeated.Content.ReadFromJsonAsync<CanjeDto>())!.Id);
        Assert.Equal(0, (await vecino.GetFromJsonAsync<ImpactoDto>("/api/v1/perfil/impacto"))!.Puntos);
        Assert.Equal(HttpStatusCode.UnprocessableEntity, (await vecino.PostAsJsonAsync("/api/v1/canjes", req with { Clave = Guid.NewGuid() })).StatusCode);
        Assert.Equal(b.Stock - 1, (await vecino.GetFromJsonAsync<List<BeneficioDto>>("/api/v1/beneficios"))!.Single(x => x.Id == b.Id).Stock);
    }
    [Fact]
    public async Task Perfil_no_permite_elevar_permisos_y_bandeja_es_privada()
    {
        using var vecino = Client("demo-vecino"); using var otro = Client("demo-vecino-2");
        var n = (await vecino.GetFromJsonAsync<List<NotificacionDto>>("/api/v1/notificaciones"))!.First();
        Assert.Equal(HttpStatusCode.NotFound, (await otro.PostAsync($"/api/v1/notificaciones/{n.Id}/lectura", null)).StatusCode);
        (await vecino.PutAsJsonAsync("/api/v1/perfil", new ActualizarPerfilRequest("Nuevo nombre", "999111222", "Calle de prueba", -16.4, -71.5, false))).EnsureSuccessStatusCode();
        var perfil = (await vecino.GetFromJsonAsync<PerfilDto>("/api/v1/perfil"))!; Assert.Equal("vecino", perfil.Rol); Assert.False(perfil.RecibirAvisos);
        Assert.Equal(HttpStatusCode.Forbidden, (await vecino.GetAsync("/api/v1/operacion/indicadores")).StatusCode);
    }
}
