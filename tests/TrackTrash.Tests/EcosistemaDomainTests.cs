using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Exceptions;
namespace TrackTrash.Tests;

public sealed class EcosistemaDomainTests
{
    [Fact]
    public void Dato_antiguo_no_sustituye_estado_y_dato_vencido_no_aparece_vacio()
    { var ahora = DateTimeOffset.UtcNow; var c = Contenedor.Crear(Guid.NewGuid(), "Tacho de prueba", -16.4, -71.5); c.ActualizarNivel(95, ahora, "sensor"); c.ActualizarNivel(10, ahora.AddMinutes(-1), "sensor"); Assert.Equal(95, c.Porcentaje); Assert.Equal("SinDatosRecientes", c.Estado(ahora.AddHours(1))); }
    [Fact]
    public void Mediana_descarta_valor_atipico()
    { var l = LecturaNivel.Crear(Guid.NewGuid(), 1, [5, 6, 90], 100, 90, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow); Assert.Equal(94, l.Porcentaje); }
    [Fact]
    public void Sin_stock_o_saldo_no_se_reserva_un_beneficio()
    { var b = Beneficio.Crear("Prueba", "Beneficio para probar", 20, 1); Assert.Throws<ReglaNegocioException>(() => b.Reservar(19)); b.Reservar(20); Assert.Throws<ReglaNegocioException>(() => b.Reservar(100)); Assert.Equal(0, b.Stock); }
    [Fact]
    public void Distancia_geografica_usa_coordenadas_en_grados()
    { Assert.InRange(Ubicacion.DistanciaKm(-16.3988, -71.5369, -16.4012, -71.5373), 0.2, 0.4); }
}
