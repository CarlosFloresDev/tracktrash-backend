using MediatR;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.Contenedores.Commands;

public sealed record RegistrarLecturaCommand : IRequest<LecturaResultadoDto>
{
    public Guid DispositivoId { get; set; }

    public string ClaveDispositivo { get; set; } = null!;

    public long Secuencia { get; set; }

    public double[]? DistanciasCm { get; set; }

    public int Bateria { get; set; }

    public DateTimeOffset MedidaEn { get; set; }
}

internal sealed class RegistrarLecturaCommandHandler(
    IContenedorRepository repo,
    IIncidenciaRepository incidencias,
    IUnitOfWork unitOfWork,
    ISeguridadDispositivoService seguridad,
    TimeProvider reloj) : IRequestHandler<RegistrarLecturaCommand, LecturaResultadoDto>
{
    public async Task<LecturaResultadoDto> Handle(RegistrarLecturaCommand request, CancellationToken cancellationToken)
    {
        var d = await repo.DispositivoAsync(request.DispositivoId, cancellationToken);
        if (d is null || !seguridad.Validar(request.ClaveDispositivo, d.ClaveHash)) throw new AccesoDenegadoException();
        var c = await repo.ObtenerAsync(d.ContenedorId, cancellationToken) ?? throw new RecursoNoEncontradoException();
        if (await repo.LecturaExisteAsync(request.DispositivoId, request.Secuencia, cancellationToken)) return new(true, c.Porcentaje, c.Estado(reloj.GetUtcNow()));
        var ahora = reloj.GetUtcNow(); var l = LecturaNivel.Crear(request.DispositivoId, request.Secuencia, request.DistanciasCm ?? [], d.AlturaCm, request.Bateria, request.MedidaEn, ahora);
        repo.Agregar(l); d.Contactar(ahora, request.Bateria); c.ActualizarNivel(l.Porcentaje, l.MedidaEn, "Sensor IoT");
        if (c.Estado(ahora) == "Saturado" && !await incidencias.SaturacionAbiertaAsync(c.Id, cancellationToken))
            incidencias.Agregar(Incidencia.Crear(c.Id, Guid.Empty, "Saturación", "Alerta automática de llenado superior al 80% según sensor.", ahora));
        await unitOfWork.GuardarCambiosAsync(cancellationToken); return new(false, c.Porcentaje, c.Estado(ahora));
    }
}
