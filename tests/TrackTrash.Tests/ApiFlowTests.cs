using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Exceptions;
using TrackTrash.Domain.Entities;
using TrackTrash.Infrastructure.Persistence;

namespace TrackTrash.Tests;

public sealed partial class ApiFlowTests : IAsyncLifetime
{
    private WebApplicationFactory<Program> _factory = null!;
    private string _connection = "";
    private string _admin = "";
    private readonly string _database = "tracktrash_test_" + Guid.NewGuid().ToString("N");

    public async Task InitializeAsync()
    {
        var value = Environment.GetEnvironmentVariable("TRACKTRASH_TEST_CONNECTION")
            ?? throw new InvalidOperationException("Configura TRACKTRASH_TEST_CONNECTION o ejecuta scripts/test.ps1. Las pruebas usan una BD temporal propia.");
        var b = new NpgsqlConnectionStringBuilder(value) { Database = "postgres" };
        _admin = b.ConnectionString;
        await using (var admin = new NpgsqlConnection(_admin))
        {
            await admin.OpenAsync();
            await using var cmd = new NpgsqlCommand($"CREATE DATABASE \"{_database}\"", admin);
            await cmd.ExecuteNonQueryAsync();
        }
        b.Database = _database;
        _connection = b.ConnectionString;
        _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(web =>
        {
            web.UseEnvironment("Development");
            web.ConfigureAppConfiguration((_, config) => config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:TrackTrash"] = _connection,
                ["Authentication:Demo"] = "true",
                ["Jobs:Enabled"] = "false",
                ["Logging:LogLevel:Default"] = "Warning"
            }));
            web.ConfigureServices(services =>
            {
                services.RemoveAll<TrackTrashDbContext>();
                services.RemoveAll<DbContextOptions<TrackTrashDbContext>>();
                services.RemoveAll<IDbContextOptionsConfiguration<TrackTrashDbContext>>();
                services.AddDbContext<TrackTrashDbContext>(o => o.UseNpgsql(_connection,
                    npg => npg.UseNetTopologySuite().MigrationsHistoryTable("__EFMigrationsHistory", "tracktrash")));
            });
        });
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TrackTrashDbContext>();
        await db.Database.MigrateAsync();
        await DemoSeed.InicializarAsync(db);
    }

    public async Task DisposeAsync()
    {
        if (_factory is not null) await _factory.DisposeAsync();
        if (_admin == "") return;
        NpgsqlConnection.ClearAllPools();
        await using var admin = new NpgsqlConnection(_admin);
        await admin.OpenAsync();
        await using var cmd = new NpgsqlCommand($"DROP DATABASE IF EXISTS \"{_database}\" WITH (FORCE)", admin);
        await cmd.ExecuteNonQueryAsync();
    }

    private HttpClient Client(string? token)
    {
        var client = _factory.CreateClient();
        if (token is not null) client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return client;
    }

    private async Task<SolicitudDto> Crear(HttpClient client)
    {
        var response = await client.PostAsJsonAsync("/api/v1/solicitudes", new CrearSolicitudRequest(
            Guid.NewGuid(), MaterialReciclable.PapelCarton, 2.5m, "Av. Arequipa 123", DateTimeOffset.UtcNow.AddDays(1)));
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<SolicitudDto>())!;
    }

    [Fact]
    public async Task Recorrido_completo_guarda_puntos_una_sola_vez_y_oculta_codigo_al_reciclador()
    {
        using var vecino = Client("demo-vecino"); using var reciclador = Client("demo-reciclador");
        var s = await Crear(vecino);
        Assert.NotNull(s.CodigoConfirmacion);
        var disponible = await reciclador.GetFromJsonAsync<SolicitudDto>($"/api/v1/solicitudes/{s.Id}");
        Assert.Null(disponible!.Direccion);
        Assert.Null(disponible.CodigoConfirmacion);
        (await reciclador.PostAsync($"/api/v1/solicitudes/{s.Id}/aceptacion", null)).EnsureSuccessStatusCode();
        var payload = new ConfirmarEntregaRequest(s.CodigoConfirmacion!);
        (await reciclador.PostAsJsonAsync($"/api/v1/solicitudes/{s.Id}/confirmacion", payload)).EnsureSuccessStatusCode();
        (await reciclador.PostAsJsonAsync($"/api/v1/solicitudes/{s.Id}/confirmacion", payload)).EnsureSuccessStatusCode();
        Assert.Equal(20, (await vecino.GetFromJsonAsync<ImpactoDto>("/api/v1/perfil/impacto"))!.Puntos);
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TrackTrashDbContext>();
        Assert.Equal(1, await db.MovimientosPuntos.CountAsync());
        Assert.Equal(EstadoSolicitud.Confirmada, (await db.Solicitudes.SingleAsync()).Estado);
    }

    [Fact]
    public async Task Sin_sesion_no_hay_acceso_y_otro_vecino_no_puede_leer_la_solicitud()
    {
        using var anonimo = Client(null); using var vecino = Client("demo-vecino"); using var otro = Client("demo-vecino-2");
        Assert.Equal(HttpStatusCode.Unauthorized, (await anonimo.GetAsync("/api/v1/solicitudes")).StatusCode);
        var s = await Crear(vecino);
        Assert.Equal(HttpStatusCode.NotFound, (await otro.GetAsync($"/api/v1/solicitudes/{s.Id}")).StatusCode);
        Assert.Empty((await otro.GetFromJsonAsync<List<SolicitudDto>>("/api/v1/solicitudes"))!);
        Assert.Equal(HttpStatusCode.Forbidden, (await vecino.PostAsync($"/api/v1/solicitudes/{s.Id}/aceptacion", null)).StatusCode);
    }

    [Fact]
    public async Task Reenvio_de_creacion_devuelve_la_misma_solicitud()
    {
        using var vecino = Client("demo-vecino");
        var request = new CrearSolicitudRequest(Guid.NewGuid(), MaterialReciclable.Vidrio, 2, "Av. Arequipa 123", DateTimeOffset.UtcNow.AddDays(1));
        var a = await (await vecino.PostAsJsonAsync("/api/v1/solicitudes", request)).Content.ReadFromJsonAsync<SolicitudDto>();
        var b = await (await vecino.PostAsJsonAsync("/api/v1/solicitudes", request)).Content.ReadFromJsonAsync<SolicitudDto>();
        Assert.Equal(a!.Id, b!.Id);
        Assert.Equal(HttpStatusCode.UnprocessableEntity,
            (await vecino.PostAsJsonAsync("/api/v1/solicitudes", request with { CantidadEstimadaKg = 3 })).StatusCode);
    }

    [Fact]
    public async Task Codigo_incorrecto_no_modifica_estado_ni_otorga_puntos()
    {
        using var vecino = Client("demo-vecino"); using var r = Client("demo-reciclador"); using var otro = Client("demo-reciclador-2");
        var s = await Crear(vecino);
        (await r.PostAsync($"/api/v1/solicitudes/{s.Id}/aceptacion", null)).EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Forbidden,
            (await otro.PostAsJsonAsync($"/api/v1/solicitudes/{s.Id}/confirmacion", new ConfirmarEntregaRequest(s.CodigoConfirmacion!))).StatusCode);
        var incorrecto = s.CodigoConfirmacion == "000000" ? "111111" : "000000";
        Assert.Equal(HttpStatusCode.UnprocessableEntity,
            (await r.PostAsJsonAsync($"/api/v1/solicitudes/{s.Id}/confirmacion", new ConfirmarEntregaRequest(incorrecto))).StatusCode);
        Assert.Equal(0, (await vecino.GetFromJsonAsync<ImpactoDto>("/api/v1/perfil/impacto"))!.Puntos);
        Assert.Equal("Aceptada", (await vecino.GetFromJsonAsync<SolicitudDto>($"/api/v1/solicitudes/{s.Id}"))!.Estado);
    }

    [Fact]
    public async Task Dos_contextos_no_pueden_aceptar_la_misma_solicitud()
    {
        using var vecino = Client("demo-vecino"); var s = await Crear(vecino);
        var options = new DbContextOptionsBuilder<TrackTrashDbContext>().UseNpgsql(_connection,o=>o.UseNetTopologySuite()).Options;
        await using var a = new TrackTrashDbContext(options); await using var b = new TrackTrashDbContext(options);
        var primero = await a.Solicitudes.SingleAsync(); var segundo = await b.Solicitudes.SingleAsync();
        primero.Aceptar(Guid.NewGuid()); segundo.Aceptar(Guid.NewGuid());
        await new UnitOfWork(a).GuardarCambiosAsync(default);
        await Assert.ThrowsAsync<ConflictoPersistenciaException>(() => new UnitOfWork(b).GuardarCambiosAsync(default));
    }

    [Fact]
    public async Task Fallo_al_guardar_puntos_revierte_tambien_la_confirmacion()
    {
        using var vecino = Client("demo-vecino"); using var r = Client("demo-reciclador");
        var dto = await Crear(vecino);
        (await r.PostAsync($"/api/v1/solicitudes/{dto.Id}/aceptacion", null)).EnsureSuccessStatusCode();
        var options = new DbContextOptionsBuilder<TrackTrashDbContext>().UseNpgsql(_connection,o=>o.UseNetTopologySuite()).Options;
        await using (var db = new TrackTrashDbContext(options))
        {
            var s = await db.Solicitudes.SingleAsync();
            s.Confirmar(s.RecicladorId!.Value, dto.CodigoConfirmacion!, DateTimeOffset.UtcNow);
            db.MovimientosPuntos.Add(MovimientoPuntos.PorEntrega(s, DateTimeOffset.UtcNow));
            db.MovimientosPuntos.Add(MovimientoPuntos.PorEntrega(s, DateTimeOffset.UtcNow));
            await Assert.ThrowsAsync<ConflictoPersistenciaException>(() => new UnitOfWork(db).GuardarCambiosAsync(default));
        }
        await using var check = new TrackTrashDbContext(options);
        Assert.Equal(EstadoSolicitud.Aceptada, (await check.Solicitudes.SingleAsync()).Estado);
        Assert.Empty(await check.MovimientosPuntos.ToListAsync());
    }
}
