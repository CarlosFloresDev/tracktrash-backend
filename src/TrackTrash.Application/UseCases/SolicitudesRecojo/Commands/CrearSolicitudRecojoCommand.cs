using System.ComponentModel.DataAnnotations;
using MediatR;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Ports.Persistence;
using TrackTrash.Domain.Ports.Repositories;

namespace TrackTrash.Application.UseCases.SolicitudesRecojo.Commands;

public sealed record CrearSolicitudRecojoCommand : IRequest<SolicitudDto>
{
    [Required]
    public Guid ClaveSolicitud { get; set; }

    [EnumDataType(typeof(MaterialReciclable))]
    public MaterialReciclable Material { get; set; }

    [Range(typeof(decimal), "0.01", "500")]
    public decimal CantidadEstimadaKg { get; set; }

    [Required, StringLength(250, MinimumLength = 8)]
    public string Direccion { get; set; } = null!;

    public DateTimeOffset ProgramadaPara { get; set; }

    public double? Latitud { get; set; }

    public double? Longitud { get; set; }

    public bool AutoAsignar { get; set; }
}

internal sealed class CrearSolicitudRecojoCommandHandler(
    ISolicitudRecojoRepository solicitudes,
    IUnitOfWork unitOfWork,
    IUsuarioActualService usuario,
    ICodigoEntregaService codigos,
    TimeProvider reloj,
    IHistoricoRepository historial,
    ICatalogoRepository catalogo) : IRequestHandler<CrearSolicitudRecojoCommand, SolicitudDto>
{
    public async Task<SolicitudDto> Handle(CrearSolicitudRecojoCommand request, CancellationToken cancellationToken)
    {
        if (usuario.EsReciclador) throw new AccesoDenegadoException();
        var existente = await solicitudes.ObtenerPorClaveAsync(usuario.Id, request.ClaveSolicitud, cancellationToken);
        if (existente is not null)
        {
            if (existente.Material != request.Material || existente.CantidadEstimadaKg != request.CantidadEstimadaKg
                || existente.Direccion != request.Direccion.Trim()
                || existente.ProgramadaPara.UtcTicks / 10 != request.ProgramadaPara.UtcTicks / 10
                || existente.Latitud != request.Latitud || existente.Longitud != request.Longitud)
                throw new Domain.Exceptions.ReglaNegocioException("Esta clave de solicitud ya se utilizó con otros datos.");
            return SolicitudDto.Desde(existente, usuario.Id);
        }
        var solicitud = SolicitudRecojo.Crear(usuario.Id, request.ClaveSolicitud, request.Material,
            request.CantidadEstimadaKg, request.Direccion, request.ProgramadaPara, reloj.GetUtcNow(), codigos.Generar(), request.Latitud, request.Longitud);
        solicitudes.Agregar(solicitud);
        historial.Agregar(Domain.Entities.HistoricoSolicitud.Crear(solicitud.Id, usuario.Id, "Solicitud creada", reloj.GetUtcNow()));
        if (request.AutoAsignar)
        {
            if (!solicitud.Latitud.HasValue) throw new Domain.Exceptions.ReglaNegocioException("La asignación requiere ubicación del recojo.");
            var a = (await catalogo.CandidatosAsync(solicitud.Material, solicitud.CantidadEstimadaKg, solicitud.Latitud.Value, solicitud.Longitud!.Value, cancellationToken)).FirstOrDefault();
            if (a is null) throw new Domain.Exceptions.ReglaNegocioException("No hay un aliado elegible a menos de 20 km para estos materiales.");
            solicitud.Asignar(a.Id); historial.Agregar(HistoricoSolicitud.Crear(solicitud.Id, usuario.Id, "Asignada a " + a.Nombre, reloj.GetUtcNow()));
        }
        await unitOfWork.GuardarCambiosAsync(cancellationToken);
        return SolicitudDto.Desde(solicitud, usuario.Id);
    }
}
