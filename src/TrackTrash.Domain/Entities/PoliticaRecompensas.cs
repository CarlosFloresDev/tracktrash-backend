namespace TrackTrash.Domain.Entities;

public static class PoliticaRecompensas
{
    public const int PuntosPorEntrega = 20;
    public const int MaximoDiario = 200;
    public static bool PuedeOtorgar(int yaOtorgados) => yaOtorgados + PuntosPorEntrega <= MaximoDiario;
}
