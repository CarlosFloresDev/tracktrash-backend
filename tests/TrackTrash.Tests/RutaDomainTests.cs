using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Exceptions;
namespace TrackTrash.Tests;
public sealed class RutaDomainTests
{
    [Fact]public void Proximo_horario_respeta_dia_y_hora_de_Peru()
    {var r=RutaRecoleccion.Crear(Guid.NewGuid(),"Ruta de prueba","Zona de prueba",[1],new TimeOnly(19,0));var ahora=new DateTimeOffset(2026,10,26,23,0,0,TimeSpan.Zero);Assert.Equal(new DateTimeOffset(2026,10,27,0,0,0,TimeSpan.Zero),r.ProximoPaso(ahora));}
    [Fact]public void Pasado_el_horario_se_programa_la_semana_siguiente()
    {var r=RutaRecoleccion.Crear(Guid.NewGuid(),"Ruta de prueba","Zona de prueba",[1],new TimeOnly(19,0));var ahora=new DateTimeOffset(2026,10,27,1,0,0,TimeSpan.Zero);Assert.Equal(new DateTimeOffset(2026,11,3,0,0,0,TimeSpan.Zero),r.ProximoPaso(ahora));}
    [Fact]public void Suscripcion_limita_la_anticipacion()
    {var r=RutaRecoleccion.Crear(Guid.NewGuid(),"Ruta de prueba","Zona de prueba",[1],new TimeOnly(19,0));Assert.Throws<ReglaNegocioException>(()=>SuscripcionRuta.Crear(Guid.NewGuid(),r,100,DateTimeOffset.UtcNow));}
}
