namespace TrackTrash.Domain.Exceptions;

public sealed class ReglaNegocioException(string message) : Exception(message);
