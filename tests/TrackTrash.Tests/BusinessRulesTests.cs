using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TrackTrash.Application.DTOs.Ecosistema;
using TrackTrash.Application.DTOs.SolicitudesRecojo;
using TrackTrash.Domain.Entities;
using TrackTrash.Infrastructure.Persistence;
namespace TrackTrash.Tests;

public sealed partial class ApiFlowTests
{
    [Fact]
    public async Task Canjes_concurrentes_no_gastan_dos_veces_el_mismo_saldo()
    {
        using var vecino=Client("demo-vecino");using var r=Client("demo-reciclador");var s=await Crear(vecino);
        (await r.PostAsync($"/api/v1/solicitudes/{s.Id}/aceptacion",null)).EnsureSuccessStatusCode();
        (await r.PostAsJsonAsync($"/api/v1/solicitudes/{s.Id}/confirmacion",new ConfirmarEntregaRequest(s.CodigoConfirmacion!))).EnsureSuccessStatusCode();
        var b=(await vecino.GetFromJsonAsync<List<BeneficioDto>>("/api/v1/beneficios"))!.First(x=>x.CostoPuntos==20);
        var results=await Task.WhenAll(vecino.PostAsJsonAsync("/api/v1/canjes",new CanjearRequest(b.Id,Guid.NewGuid())),vecino.PostAsJsonAsync("/api/v1/canjes",new CanjearRequest(b.Id,Guid.NewGuid())));
        Assert.Single(results,x=>x.IsSuccessStatusCode);Assert.All(results.Where(x=>!x.IsSuccessStatusCode),x=>Assert.True(x.StatusCode is HttpStatusCode.Conflict or HttpStatusCode.UnprocessableEntity));
        Assert.Equal(0,(await vecino.GetFromJsonAsync<ImpactoDto>("/api/v1/perfil/impacto"))!.Puntos);
    }
    [Fact]
    public async Task Limite_diario_no_impide_confirmar_una_entrega_legitima()
    {
        var user=Guid.Parse("11111111-1111-1111-1111-111111111111");var recycler=Guid.Parse("22222222-2222-2222-2222-222222222222");var now=DateTimeOffset.UtcNow;
        using(var scope=_factory.Services.CreateScope())
        {
            var db=scope.ServiceProvider.GetRequiredService<TrackTrashDbContext>();
            for(var i=0;i<10;i++)
            {var item=SolicitudRecojo.Crear(user,Guid.NewGuid(),MaterialReciclable.Vidrio,1,"Prueba de límite diario",now.AddDays(1),now,"123456");item.Aceptar(recycler);item.Confirmar(recycler,"123456",now);db.Add(item);db.Add(MovimientoPuntos.PorEntrega(item,now));}
            await db.SaveChangesAsync();
        }
        using var vecino=Client("demo-vecino");using var r=Client("demo-reciclador");var s=await Crear(vecino);
        (await r.PostAsync($"/api/v1/solicitudes/{s.Id}/aceptacion",null)).EnsureSuccessStatusCode();
        (await r.PostAsJsonAsync($"/api/v1/solicitudes/{s.Id}/confirmacion",new ConfirmarEntregaRequest(s.CodigoConfirmacion!))).EnsureSuccessStatusCode();
        Assert.Equal(200,(await vecino.GetFromJsonAsync<ImpactoDto>("/api/v1/perfil/impacto"))!.Puntos);
        Assert.Equal("Confirmada",(await vecino.GetFromJsonAsync<SolicitudDto>($"/api/v1/solicitudes/{s.Id}"))!.Estado);
    }
    [Fact]
    public async Task Suscripciones_son_privadas_y_el_reporte_de_paso_tiene_limite()
    {
        using var vecino=Client("demo-vecino");using var otro=Client("demo-vecino-2");var rutas=(await vecino.GetFromJsonAsync<List<RutaDto>>("/api/v1/rutas"))!;Assert.NotEmpty(rutas);
        var id=rutas[0].Id;(await vecino.PostAsJsonAsync($"/api/v1/rutas/{id}/suscripcion",new SuscribirRutaRequest(20))).EnsureSuccessStatusCode();
        Assert.True((await vecino.GetFromJsonAsync<List<RutaDto>>("/api/v1/rutas"))!.Single(r=>r.Id==id).Suscrito);
        Assert.False((await otro.GetFromJsonAsync<List<RutaDto>>("/api/v1/rutas"))!.Single(r=>r.Id==id).Suscrito);
        (await vecino.PostAsync($"/api/v1/rutas/{id}/paso",null)).EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.UnprocessableEntity,(await vecino.PostAsync($"/api/v1/rutas/{id}/paso",null)).StatusCode);
    }
    [Fact]
    public async Task Auditoria_registra_actor_sin_copiar_direccion_ni_codigo()
    {
        using var vecino=Client("demo-vecino");var s=await Crear(vecino);
        using var scope=_factory.Services.CreateScope();var db=scope.ServiceProvider.GetRequiredService<TrackTrashDbContext>();
        var audit=await db.Set<EventoAuditoria>().SingleAsync(e=>e.Entidad==nameof(SolicitudRecojo)&&e.Referencia==s.Id.ToString());
        Assert.Equal("11111111-1111-1111-1111-111111111111",audit.Actor);Assert.DoesNotContain(s.CodigoConfirmacion!,audit.Campos);Assert.DoesNotContain(s.Direccion!,audit.Campos);
    }
}
public sealed class RecompensasDomainTests
{
    [Fact]public void Racha_cambia_al_cruzar_medianoche_en_Peru_y_no_se_duplica_en_el_dia()
    {var p=PerfilUsuario.Crear(Guid.NewGuid());var a=new DateTimeOffset(2026,10,24,4,59,0,TimeSpan.Zero);p.RegistrarEntrega(a);p.RegistrarEntrega(a.AddMinutes(1));p.RegistrarEntrega(a.AddHours(1));Assert.Equal(2,p.RachaActual);Assert.Equal(2,p.RachaMaxima);Assert.Equal(0,p.RachaVigente(a.AddDays(4)));}
    [Theory][InlineData(180,true)][InlineData(200,false)][InlineData(0,true)]
    public void Recompensas_respetan_limite_diario(int puntos,bool permitido)=>Assert.Equal(permitido,PoliticaRecompensas.PuedeOtorgar(puntos));
}
