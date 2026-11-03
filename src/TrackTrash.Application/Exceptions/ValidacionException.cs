namespace TrackTrash.Application.Exceptions;

public sealed class ValidacionException(IDictionary<string, string[]> errores)
    : Exception("Uno o más campos no son válidos.")
{
    public IDictionary<string, string[]> Errores { get; } = errores;
}
