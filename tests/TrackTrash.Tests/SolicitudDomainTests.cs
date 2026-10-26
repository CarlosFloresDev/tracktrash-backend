using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Tests;

public sealed class SolicitudDomainTests
{
    private static SolicitudRecojo Nueva() => SolicitudRecojo.Crear(Guid.NewGuid(), Guid.NewGuid(),
        MaterialReciclable.PapelCarton, 2, "Av. Arequipa 123", DateTimeOffset.UtcNow.AddDays(1),
        DateTimeOffset.UtcNow, "123456");

    [Fact]
    public void Confirmar_repetidamente_no_genera_otra_entrega()
    {
        var s = Nueva(); var reciclador = Guid.NewGuid();
        s.Aceptar(reciclador);
        Assert.True(s.Confirmar(reciclador, "123456", DateTimeOffset.UtcNow));
        Assert.False(s.Confirmar(reciclador, "123456", DateTimeOffset.UtcNow));
        Assert.Equal(20, MovimientoPuntos.PorEntrega(s, DateTimeOffset.UtcNow).Puntos);
    }

    [Fact]
    public void No_se_acepta_una_solicitud_propia()
    {
        var s = Nueva();
        Assert.Throws<ReglaNegocioException>(() => s.Aceptar(s.VecinoId));
    }

    [Fact]
    public void Codigo_incorrecto_conserva_estado_aceptada()
    {
        var s = Nueva(); var r = Guid.NewGuid(); s.Aceptar(r);
        Assert.Throws<ReglaNegocioException>(() => s.Confirmar(r, "999999", DateTimeOffset.UtcNow));
        Assert.Equal(EstadoSolicitud.Aceptada, s.Estado);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(501)]
    public void Cantidad_fuera_de_rango_es_rechazada(int kg) =>
        Assert.Throws<ReglaNegocioException>(() => SolicitudRecojo.Crear(Guid.NewGuid(), Guid.NewGuid(),
            MaterialReciclable.Vidrio, kg, "Av. Arequipa 123", DateTimeOffset.UtcNow.AddDays(1), DateTimeOffset.UtcNow, "123456"));

    [Fact]
    public void No_se_otorgan_puntos_por_solicitud_pendiente() =>
        Assert.Throws<ReglaNegocioException>(() => MovimientoPuntos.PorEntrega(Nueva(), DateTimeOffset.UtcNow));
}
