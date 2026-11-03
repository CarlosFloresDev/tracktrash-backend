using TrackTrash.Domain.Entities;

namespace TrackTrash.Api.Contracts;

public sealed record CrearSolicitudRequest(
    Guid ClaveSolicitud,
    MaterialReciclable Material,
    decimal CantidadEstimadaKg,
    string Direccion,
    DateTimeOffset ProgramadaPara);
