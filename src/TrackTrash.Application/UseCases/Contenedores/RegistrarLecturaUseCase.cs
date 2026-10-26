using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Repositories;
using TrackTrash.Domain.Ports.Persistence;
namespace TrackTrash.Application.UseCases.Contenedores;

public sealed class RegistrarLecturaUseCase(IContenedorRepository repo, IIncidenciaRepository incidencias, IUnitOfWork uow, ISeguridadDispositivoService seguridad, TimeProvider reloj)
{
    public async Task<LecturaResultadoDto> ExecuteAsync(Guid id, string clave, LecturaRequest r, CancellationToken ct)
    {
        var d = await repo.DispositivoAsync(id, ct);
        if (d is null || !seguridad.Validar(clave, d.ClaveHash)) throw new AccesoDenegadoException();
        var c = await repo.ObtenerAsync(d.ContenedorId, ct) ?? throw new RecursoNoEncontradoException();
        if (await repo.LecturaExisteAsync(id, r.Secuencia, ct)) return new(true, c.Porcentaje, c.Estado(reloj.GetUtcNow()));
        var ahora = reloj.GetUtcNow(); var l = LecturaNivel.Crear(id, r.Secuencia, r.DistanciasCm ?? [], d.AlturaCm, r.Bateria, r.MedidaEn, ahora);
        repo.Agregar(l); d.Contactar(ahora, r.Bateria); c.ActualizarNivel(l.Porcentaje, l.MedidaEn, "Sensor IoT");
        if (c.Estado(ahora) == "Saturado" && !await incidencias.SaturacionAbiertaAsync(c.Id, ct))
            incidencias.Agregar(Incidencia.Crear(c.Id, Guid.Empty, "Saturación", "Alerta automática de llenado superior al 80% según sensor.", ahora));
        await uow.GuardarCambiosAsync(ct); return new(false, c.Porcentaje, c.Estado(ahora));
    }
}
