using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Infrastructure.Persistence;

namespace TrackTrash.Tests;

public sealed class PersistenceArchitectureTests
{
    private static TrackTrashDbContext CrearContexto()
    {
        var options = new DbContextOptionsBuilder<TrackTrashDbContext>()
            .UseNpgsql("Host=localhost;Database=tracktrash_model;Username=model;Password=model")
            .Options;
        return new TrackTrashDbContext(options);
    }

    [Fact]
    public void Todas_las_entidades_persistentes_forman_parte_del_modelo()
    {
        using var db = CrearContexto();
        var esperadas = new[]
        {
            typeof(SolicitudRecojo), typeof(MovimientoPuntos), typeof(PerfilUsuario), typeof(Distrito),
            typeof(AsociacionReciclaje), typeof(MaterialAceptado), typeof(PuntoAcopio), typeof(Contenedor),
            typeof(DispositivoIoT), typeof(LecturaNivel), typeof(ReporteCiudadano), typeof(RutaRecoleccion),
            typeof(SuscripcionRuta), typeof(ReportePasoCamion), typeof(Incidencia), typeof(Notificacion),
            typeof(Beneficio), typeof(Canje), typeof(HistoricoSolicitud), typeof(EventoAuditoria)
        };

        foreach (var tipo in esperadas)
            Assert.NotNull(db.Model.FindEntityType(tipo));
    }

    [Fact]
    public void UnitOfWork_expone_repositorios_especificos_y_genericos()
    {
        using var db = CrearContexto();
        var uow = new UnitOfWork(db);

        Assert.IsAssignableFrom<ISolicitudRecojoRepository>(uow.Solicitudes);
        Assert.IsAssignableFrom<IMovimientoPuntosRepository>(uow.MovimientosPuntos);
        Assert.Same(uow.Repositorio<EventoAuditoria>(), uow.Repositorio<EventoAuditoria>());
    }
}
