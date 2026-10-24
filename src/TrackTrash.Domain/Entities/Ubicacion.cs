using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Domain.Entities;

public static class Ubicacion
{
    public static void Validar(double lat, double lng)
    {
        if (!double.IsFinite(lat) || !double.IsFinite(lng) || lat is < -90 or > 90 || lng is < -180 or > 180)
            throw new ReglaNegocioException("Las coordenadas no son válidas.");
    }
    public static double DistanciaKm(double lat1, double lon1, double lat2, double lon2)
    {
        var a = Math.Sin((lat2 - lat1) * Math.PI / 360); var b = Math.Sin((lon2 - lon1) * Math.PI / 360);
        return 12742 * Math.Asin(Math.Min(1, Math.Sqrt(a * a + Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) * b * b)));
    }
    public static string Texto(string? value, int min, int max, string campo)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Trim().Length < min || value.Trim().Length > max)
            throw new ReglaNegocioException($"{campo} debe tener entre {min} y {max} caracteres.");
        return value.Trim();
    }
}

