using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Domain.Entities;

public sealed class PerfilUsuario
{
    private PerfilUsuario() { }
    public Guid Id { get; private set; }
    public string Nombre { get; private set; } = "Vecino TrackTrash";
    public string Telefono { get; private set; } = "";
    public string Direccion { get; private set; } = "";
    public double? Latitud { get; private set; }
    public double? Longitud { get; private set; }
    public Guid? AsociacionId { get; private set; }
    public bool RecibirAvisos { get; private set; } = true;
    public bool AvisosHorarios { get; private set; } = true;
    public bool AvisosRecojos { get; private set; } = true;
    public bool AvisosIncidencias { get; private set; } = true;
    public bool AvisosRecompensas { get; private set; } = true;

    public bool PermiteAviso(TipoAviso tipo) => RecibirAvisos && tipo switch
    {
        TipoAviso.Sistema => true,
        TipoAviso.Horarios => AvisosHorarios,
        TipoAviso.Recojos => AvisosRecojos,
        TipoAviso.Incidencias => AvisosIncidencias,
        TipoAviso.Recompensas => AvisosRecompensas,
        _ => false
    };

    public void ActualizarPreferenciasAvisos(bool horarios, bool recojos, bool incidencias, bool recompensas)
    {
        AvisosHorarios = horarios;
        AvisosRecojos = recojos;
        AvisosIncidencias = incidencias;
        AvisosRecompensas = recompensas;
        Version++;
    }
    public int Version { get; private set; }
    public DateOnly? UltimoDiaEntrega { get; private set; }
    public int RachaActual { get; private set; }
    public int RachaMaxima { get; private set; }
    public void RegistrarEntrega(DateTimeOffset ahora)
    {
        var dia = DateOnly.FromDateTime(ahora.ToOffset(TimeSpan.FromHours(-5)).DateTime);
        if (UltimoDiaEntrega != dia) { RachaActual = UltimoDiaEntrega == dia.AddDays(-1) ? RachaActual + 1 : 1; RachaMaxima = Math.Max(RachaMaxima, RachaActual); UltimoDiaEntrega = dia; }
        Version++;
    }
    public int RachaVigente(DateTimeOffset ahora) => UltimoDiaEntrega >= DateOnly.FromDateTime(ahora.ToOffset(TimeSpan.FromHours(-5)).DateTime).AddDays(-1) ? RachaActual : 0;
    public static PerfilUsuario Crear(Guid id) => new() { Id = id };
    public void Actualizar(string nombre, string telefono, string direccion, double? lat, double? lng, bool avisos)
    {
        Nombre = Ubicacion.Texto(nombre, 2, 100, "Nombre");
        if (telefono.Length > 20 || telefono.Any(c => !char.IsAsciiDigit(c) && c is not '+' and not ' '))
            throw new ReglaNegocioException("El teléfono no es válido.");
        if (direccion.Length > 250 || lat.HasValue != lng.HasValue) throw new ReglaNegocioException("La dirección o ubicación no es válida.");
        if (lat.HasValue) Ubicacion.Validar(lat.Value, lng!.Value);
        Telefono = telefono.Trim(); Direccion = direccion.Trim(); Latitud = lat; Longitud = lng; RecibirAvisos = avisos; Version++;
    }
    public void VincularAsociacion(Guid id) { AsociacionId = id; Version++; }
    public void RegistrarOperacionSaldo() { Version++; }
}

