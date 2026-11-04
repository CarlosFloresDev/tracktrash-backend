using TrackTrash.Application.DTOs.Asociaciones;
using TrackTrash.Application.DTOs.Contenedores;

namespace TrackTrash.Application.DTOs.Contenedores;

public sealed record MapaDto(IReadOnlyList<ContenedorDto> Contenedores, IReadOnlyList<AsociacionDto> Asociaciones, IReadOnlyList<PuntoAcopioDto> PuntosAcopio);
public sealed record PuntoAcopioDto(Guid Id, string Nombre, string Direccion, double Latitud, double Longitud, bool Demostracion);
