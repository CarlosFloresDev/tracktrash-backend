using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrackTrash.Infrastructure.Configuration;
using TrackTrash.Infrastructure.Persistence;

namespace TrackTrash.Tests;

public sealed partial class ApiFlowTests
{
    [Fact]
    public async Task Piloto_requiere_habilitacion_y_vincula_identidades_reales_sin_duplicarlas()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TrackTrashDbContext>();
        await Assert.ThrowsAsync<InvalidOperationException>(() => PilotSeed.InicializarAsync(db, new ConfigurationBuilder().Build()));
        var vecino = Guid.NewGuid(); var reciclador = Guid.NewGuid(); var admin = Guid.NewGuid();
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["Pilot:Enabled"] = "true", ["Pilot:VecinoId"] = vecino.ToString(),
            ["Pilot:RecicladorId"] = reciclador.ToString(), ["Pilot:AdminId"] = admin.ToString()
        }).Build();
        await PilotSeed.InicializarAsync(db, config);
        await PilotSeed.InicializarAsync(db, config);
        var perfil = await db.Perfiles.SingleAsync(p => p.Id == reciclador);
        Assert.Equal(DemoSeed.AsociacionPrincipal, perfil.AsociacionId);
        Assert.Equal(3, await db.Perfiles.CountAsync(p => p.Id == vecino || p.Id == reciclador || p.Id == admin));
    }

    [Fact]
    public async Task Readiness_rechaza_un_esquema_con_migraciones_pendientes()
    {
        using var cliente = Client(null);
        Assert.Equal(HttpStatusCode.OK, (await cliente.GetAsync("/health/ready")).StatusCode);
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TrackTrashDbContext>();
        var migrator = db.GetService<IMigrator>();
        try
        {
            await migrator.MigrateAsync("20261024090251_AuditoriaOperaciones");
            Assert.Equal(HttpStatusCode.ServiceUnavailable, (await cliente.GetAsync("/health/ready")).StatusCode);
        }
        finally { await migrator.MigrateAsync(); }
    }

    [Fact]
    public async Task Render_genera_location_https_sin_confiar_en_cabeceras_de_cliente()
    {
        using var render = _factory.WithWebHostBuilder(web => web.ConfigureAppConfiguration((_, config) =>
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["RENDER"] = "true", ["RENDER_EXTERNAL_URL"] = "https://tracktrash-api.example"
            })));
        using var cliente = render.CreateClient();
        cliente.DefaultRequestHeaders.Authorization = new("Bearer", "demo-vecino");
        cliente.DefaultRequestHeaders.Add("X-Forwarded-Proto", "http");
        var response = await cliente.PostAsJsonAsync("/api/v1/solicitudes", new TrackTrash.Application.DTOs.SolicitudesRecojo.CrearSolicitudRequest(
            Guid.NewGuid(), TrackTrash.Domain.Entities.MaterialReciclable.Vidrio, 1, "Referencia de prueba", DateTimeOffset.UtcNow.AddDays(1)));
        response.EnsureSuccessStatusCode();
        Assert.Equal("https", response.Headers.Location!.Scheme);
    }
}

public sealed class RenderHostingTests
{
    [Fact]
    public void Hosting_no_se_activa_por_una_url_sin_la_marca_del_proveedor()
    {
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            { ["RENDER_EXTERNAL_URL"] = "https://example.com" }).Build();
        Assert.False(RenderHosting.HasManagedHttps(config));
    }

    [Theory]
    [InlineData("http://example.com")]
    [InlineData("")]
    public void Render_exige_origen_https_declarado(string origin)
    {
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>
            { ["RENDER"] = "true", ["RENDER_EXTERNAL_URL"] = origin }).Build();
        Assert.Throws<InvalidOperationException>(() => RenderHosting.HasManagedHttps(config));
    }
}
