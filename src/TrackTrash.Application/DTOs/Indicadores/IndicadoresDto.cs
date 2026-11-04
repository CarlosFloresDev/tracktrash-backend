namespace TrackTrash.Application.DTOs.Indicadores;

public sealed record IndicadoresDto(int Contenedores, int Saturados, int IncidenciasAbiertas, int RecojosConfirmados, decimal KgEstimadosConfirmados);
