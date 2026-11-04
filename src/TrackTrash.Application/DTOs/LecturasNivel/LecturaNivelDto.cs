namespace TrackTrash.Application.DTOs.LecturasNivel;

public sealed record LecturaNivelDto(Guid Id, Guid DispositivoId, long Secuencia, int Porcentaje, int Bateria, DateTimeOffset MedidaEn, DateTimeOffset RecibidaEn);
