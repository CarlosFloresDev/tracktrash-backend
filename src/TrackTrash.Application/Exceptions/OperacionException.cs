namespace TrackTrash.Application.Exceptions;

public sealed class RecursoNoEncontradoException() : Exception("No se encontró el recurso solicitado.");
public sealed class AccesoDenegadoException() : Exception("No tienes permiso para esta operación.");
public sealed class ConflictoPersistenciaException() : Exception("La solicitud cambió mientras operabas. Actualiza y vuelve a intentarlo.");
