using Microsoft.EntityFrameworkCore;
using TrackTrash.Domain.Entities;
namespace TrackTrash.Infrastructure.Persistence;

public static class DemoSeed
{
    public static readonly Guid AsociacionPrincipal=Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
    public static async Task InicializarAsync(TrackTrashDbContext db,CancellationToken ct=default)
    {
        if(await db.Distritos.AnyAsync(ct)){await InicializarRutasAsync(db,ct);return;}
        var centro=Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
        var cayma=Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
        db.Distritos.AddRange(Distrito.Crear(centro,"Arequipa Cercado"),Distrito.Crear(cayma,"Cayma"));
        var a=AsociacionReciclaje.Crear(AsociacionPrincipal,centro,"EcoSur · aliado de demostración","Punto de referencia de prueba, Cercado",-16.4012,-71.5373,10000,Enum.GetValues<MaterialReciclable>(),true);a.Verificar();
        var b=AsociacionReciclaje.Crear(Guid.NewGuid(),cayma,"Circular Cayma · demostración","Punto de referencia de prueba, Cayma",-16.3761,-71.5494,300,[MaterialReciclable.PapelCarton,MaterialReciclable.Plastico],true);b.Verificar();
        db.Asociaciones.AddRange(a,b);
        foreach(var (id,nombre,asoc) in new[]{("11111111-1111-1111-1111-111111111111","Vecino de prueba",false),("22222222-2222-2222-2222-222222222222","Reciclador de prueba",true),("33333333-3333-3333-3333-333333333333","Segundo vecino",false),("44444444-4444-4444-4444-444444444444","Segundo reciclador",true),("55555555-5555-5555-5555-555555555555","Operador de prueba",false)})
        {
            var p=PerfilUsuario.Crear(Guid.Parse(id));p.Actualizar(nombre,"","Dirección de demostración, Arequipa",-16.3988,-71.5369,true);
            if(asoc)p.VincularAsociacion(a.Id);db.Perfiles.Add(p);
            db.Notificaciones.Add(Notificacion.Crear(p.Id,"bienvenida","Bienvenido a TrackTrash","Este entorno contiene datos de prueba. Explora el mapa y coordina tu primera entrega.",DateTimeOffset.UtcNow));
        }
        var sitios=new[]{("Plaza de referencia · Demo",-16.3988,-71.5369,32),("Zona universitaria · Demo",-16.4060,-71.5240,65),("Parque del barrio · Demo",-16.3920,-71.5400,92),("Punto verde · Demo",-16.4010,-71.5450,14)};
        foreach(var (nombre,lat,lng,nivel) in sitios)
        {
            var c=Contenedor.Crear(centro,nombre,lat,lng,true);c.ActualizarNivel(nivel,DateTimeOffset.UtcNow,"Dato de demostración");db.Contenedores.Add(c);
        }
        db.PuntosAcopio.AddRange(PuntoAcopio.Crear("Estación Circular · Demo","Ubicación referencial de demostración",-16.3961,-71.5312,true),PuntoAcopio.Crear("Acopio del barrio · Demo","Ubicación referencial de demostración",-16.4050,-71.5410,true));
        db.Beneficios.AddRange(Beneficio.Crear("Semillas para tu hogar","Beneficio de prueba: kit de semillas. No representa un premio comercial real.",20,25,true),Beneficio.Crear("Bolsa reutilizable","Beneficio de demostración para validar el flujo de canje.",60,10,true),Beneficio.Crear("Taller de compostaje","Acceso de demostración a una actividad educativa.",100,8,true));
        await db.SaveChangesAsync(ct);
        await InicializarRutasAsync(db,ct);
    }
    private static async Task InicializarRutasAsync(TrackTrashDbContext db,CancellationToken ct)
    {
        if(await db.Set<RutaRecoleccion>().AnyAsync(ct))return;
        var distrito=await db.Distritos.Select(d=>d.Id).FirstAsync(ct);
        db.AddRange(RutaRecoleccion.Crear(distrito,"Ruta Centro · Demo","Zona de referencia de demostración",[1,3,5],new TimeOnly(19,0),true),
            RutaRecoleccion.Crear(distrito,"Ruta Barrio Verde · Demo","Zona de referencia de demostración",[2,4,6],new TimeOnly(20,0),true));
        await db.SaveChangesAsync(ct);
    }
}
