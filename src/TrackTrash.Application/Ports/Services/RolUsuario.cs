namespace TrackTrash.Application.Ports.Services;

public enum RolUsuario
{
    Vecino,
    Reciclador,
    Administrador
}

public static class RolesTrackTrash
{
    public const string Vecino = "vecino";
    public const string Reciclador = "reciclador";
    public const string Administrador = "administrador";

    public static string ComoClaim(this RolUsuario rol) => rol switch
    {
        RolUsuario.Vecino => Vecino,
        RolUsuario.Reciclador => Reciclador,
        RolUsuario.Administrador => Administrador,
        _ => throw new ArgumentOutOfRangeException(nameof(rol))
    };

    public static bool IntentarObtener(string? valor, out RolUsuario rol)
    {
        rol = valor?.Trim().ToLowerInvariant() switch
        {
            Vecino => RolUsuario.Vecino,
            Reciclador => RolUsuario.Reciclador,
            Administrador => RolUsuario.Administrador,
            _ => (RolUsuario)(-1)
        };
        return Enum.IsDefined(rol);
    }
}

public static class ClaimsTrackTrash
{
    public const string Sujeto = "sub";
    public const string Rol = "tracktrash_role";
}

public static class PoliticasTrackTrash
{
    public const string SoloVecinos = nameof(SoloVecinos);
    public const string SoloRecicladores = nameof(SoloRecicladores);
    public const string SoloAdministradores = nameof(SoloAdministradores);
    public const string Operadores = nameof(Operadores);
}
