using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Domain.Entities;

public sealed class LecturaNivel
{
    private LecturaNivel() { }
    public Guid Id { get; private set; }
    public Guid DispositivoId { get; private set; }
    public long Secuencia { get; private set; }
    public int Porcentaje { get; private set; }
    public int Bateria { get; private set; }
    public DateTimeOffset MedidaEn { get; private set; }
    public DateTimeOffset RecibidaEn { get; private set; }
    public static LecturaNivel Crear(Guid dispositivo,long secuencia,IReadOnlyList<double> distancias,double altura,int bateria,DateTimeOffset medida,DateTimeOffset ahora)
    {
        if(secuencia<0 || distancias.Count is < 3 or > 11 || distancias.Any(d=>!double.IsFinite(d)||d<0||d>altura)
            || bateria is < 0 or > 100 || medida>ahora.AddMinutes(2) || medida<ahora.AddDays(-7))
            throw new ReglaNegocioException("La lectura requiere 3–11 distancias válidas, secuencia, batería y fecha dentro de siete días.");
        var orden=distancias.Order().ToArray(); var mitad=orden.Length/2;
        var mediana=orden.Length%2==1?orden[mitad]:(orden[mitad-1]+orden[mitad])/2;
        return new() {Id=Guid.NewGuid(),DispositivoId=dispositivo,Secuencia=secuencia,Porcentaje=(int)Math.Round(100*(1-mediana/altura)),Bateria=bateria,MedidaEn=medida.ToUniversalTime(),RecibidaEn=ahora};
    }
}

