using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Domain.Entities;

public enum EstadoSolicitud { Pendiente, Aceptada, EnCamino, Confirmada, Cancelada }
public enum MaterialReciclable { PapelCarton, Plastico, Vidrio, Metal }

public sealed class SolicitudRecojo
{
    private SolicitudRecojo() { }

    public Guid Id { get; private set; }
    public Guid VecinoId { get; private set; }
    public Guid? RecicladorId { get; private set; }
    public Guid? AsociacionId { get; private set; }
    public double? Latitud { get; private set; }
    public double? Longitud { get; private set; }
    public string? MotivoCancelacion { get; private set; }
    public Guid ClaveSolicitud { get; private set; }
    public MaterialReciclable Material { get; private set; }
    public decimal CantidadEstimadaKg { get; private set; }
    public string Direccion { get; private set; } = "";
    public DateTimeOffset ProgramadaPara { get; private set; }
    public DateTimeOffset CreadaEn { get; private set; }
    public DateTimeOffset? ConfirmadaEn { get; private set; }
    public EstadoSolicitud Estado { get; private set; }
    public string CodigoConfirmacion { get; private set; } = "";
    public int Version { get; private set; }

    public static SolicitudRecojo Crear(Guid vecinoId, Guid clave, MaterialReciclable material,
        decimal cantidadKg, string direccion, DateTimeOffset programada, DateTimeOffset ahora, string codigo, double? latitud = null, double? longitud = null)
    {
        if (vecinoId == Guid.Empty || clave == Guid.Empty)
            throw new ReglaNegocioException("La solicitud debe identificar al vecino y su operación.");
        if (!Enum.IsDefined(material)) throw new ReglaNegocioException("Selecciona un material válido.");
        if (cantidadKg <= 0 || cantidadKg > 500)
            throw new ReglaNegocioException("La cantidad estimada debe ser mayor a 0 y hasta 500 kg.");
        if (decimal.Round(cantidadKg, 2) != cantidadKg)
            throw new ReglaNegocioException("La cantidad estimada admite hasta dos decimales.");
        if (string.IsNullOrWhiteSpace(direccion) || direccion.Trim().Length is < 8 or > 250)
            throw new ReglaNegocioException("Escribe una dirección de entre 8 y 250 caracteres.");
        if (programada <= ahora || programada > ahora.AddDays(30))
            throw new ReglaNegocioException("Elige una fecha futura dentro de los próximos 30 días.");
        if (codigo.Length != 6 || !codigo.All(char.IsAsciiDigit))
            throw new ReglaNegocioException("El código de confirmación debe tener seis dígitos.");

        if (latitud.HasValue != longitud.HasValue) throw new ReglaNegocioException("Indica ambas coordenadas.");
        if (latitud.HasValue) Ubicacion.Validar(latitud.Value, longitud!.Value);
        return new SolicitudRecojo
        {
            Id = Guid.NewGuid(),
            VecinoId = vecinoId,
            ClaveSolicitud = clave,
            Material = material,
            CantidadEstimadaKg = cantidadKg,
            Direccion = direccion.Trim(),
            ProgramadaPara = programada.ToUniversalTime(),
            CreadaEn = ahora.ToUniversalTime(),
            CodigoConfirmacion = codigo,
            Estado = EstadoSolicitud.Pendiente,
            Latitud = latitud,
            Longitud = longitud
        };
    }

    public void Aceptar(Guid recicladorId)
    {
        if (Estado == EstadoSolicitud.Aceptada && RecicladorId == recicladorId) return;
        if (Estado != EstadoSolicitud.Pendiente)
            throw new ReglaNegocioException("La solicitud ya no está disponible.");
        if (recicladorId == Guid.Empty || recicladorId == VecinoId)
            throw new ReglaNegocioException("No puedes aceptar tu propia solicitud.");
        RecicladorId = recicladorId;
        Estado = EstadoSolicitud.Aceptada;
        Version++;
    }

    public void Asignar(Guid asociacion) { if (Estado != EstadoSolicitud.Pendiente) throw new ReglaNegocioException("Solo se asignan solicitudes pendientes."); AsociacionId = asociacion; Version++; }
    public void IniciarViaje(Guid reciclador)
    {
        if (RecicladorId != reciclador) throw new ReglaNegocioException("Solo el reciclador asignado puede iniciar el viaje.");
        if (Estado == EstadoSolicitud.EnCamino) return;
        if (Estado != EstadoSolicitud.Aceptada) throw new ReglaNegocioException("El recojo debe estar aceptado.");
        Estado = EstadoSolicitud.EnCamino; Version++;
    }
    public void Cancelar(string motivo)
    {
        if (Estado is EstadoSolicitud.Confirmada or EstadoSolicitud.Cancelada) throw new ReglaNegocioException("Este recojo ya está cerrado.");
        MotivoCancelacion = Ubicacion.Texto(motivo, 5, 300, "Motivo"); Estado = EstadoSolicitud.Cancelada; Version++;
    }
    public void Reprogramar(DateTimeOffset fecha, DateTimeOffset ahora)
    {
        if (Estado != EstadoSolicitud.Pendiente || fecha <= ahora || fecha > ahora.AddDays(30)) throw new ReglaNegocioException("Solo puedes reprogramar un recojo pendiente dentro de los próximos 30 días.");
        ProgramadaPara = fecha.ToUniversalTime(); Version++;
    }

    public bool Confirmar(Guid recicladorId, string codigo, DateTimeOffset ahora)
    {
        if (RecicladorId != recicladorId)
            throw new ReglaNegocioException("Solo el reciclador asignado puede confirmar la entrega.");
        if (CodigoConfirmacion != codigo)
            throw new ReglaNegocioException("El código de entrega no es correcto.");
        if (Estado == EstadoSolicitud.Confirmada) return false;
        if (Estado is not EstadoSolicitud.Aceptada and not EstadoSolicitud.EnCamino)
            throw new ReglaNegocioException("La solicitud debe estar aceptada antes de confirmarse.");
        Estado = EstadoSolicitud.Confirmada;
        ConfirmadaEn = ahora.ToUniversalTime();
        Version++;
        return true;
    }
}
