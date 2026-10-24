using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Domain.Entities;

public sealed class DispositivoIoT
{
    private DispositivoIoT() { }
    public Guid Id { get; private set; }
    public Guid ContenedorId { get; private set; }
    public string ClaveHash { get; private set; } = "";
    public double AlturaCm { get; private set; }
    public DateTimeOffset? UltimoContacto { get; private set; }
    public int? Bateria { get; private set; }
    public int Version { get; private set; }
    public static DispositivoIoT Crear(Guid contenedor, string hash, double altura)
    {
        if (!double.IsFinite(altura) || altura is < 20 or > 500) throw new ReglaNegocioException("Altura permitida: 20 a 500 cm.");
        return new() {Id=Guid.NewGuid(),ContenedorId=contenedor,ClaveHash=hash,AlturaCm=altura};
    }
    public void Contactar(DateTimeOffset ahora, int bateria) { UltimoContacto=ahora; Bateria=bateria; Version++; }
}

