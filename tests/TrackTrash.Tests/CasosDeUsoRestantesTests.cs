using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TrackTrash.Application.Behaviors;
using TrackTrash.Application.UseCases.EventosAuditoria.Queries.ListarEventosAuditoria;
using TrackTrash.Application.UseCases.LecturasNivel.Queries.ListarLecturasNivel;
using TrackTrash.Application.UseCases.MaterialesAceptados.Commands.AgregarMaterialAceptado;
using TrackTrash.Application.UseCases.PoliticasRecompensas.Queries.ObtenerPoliticaRecompensas;
using TrackTrash.Application.UseCases.PuntosAcopio.Commands.CrearPuntoAcopio;
using TrackTrash.Domain.Entities;
using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Tests;

public sealed class CasosDeUsoRestantesTests
{
    private static ServiceProvider Proveedor()
    {
        var services = new ServiceCollection();
        services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(ValidationBehavior<,>).Assembly));
        services.AddValidatorsFromAssembly(typeof(ValidationBehavior<,>).Assembly, includeInternalTypes: true);
        return services.BuildServiceProvider();
    }

    private static AsociacionReciclaje Asociacion(params MaterialReciclable[] materiales) =>
        AsociacionReciclaje.Crear(Guid.NewGuid(), Guid.NewGuid(), "Recicladores del Sur", "Av. Arequipa 123",
            -16.4, -71.5, 500, materiales);

    [Fact]
    public void Agregar_un_material_nuevo_lo_incorpora_a_la_asociacion()
    {
        var a = Asociacion(MaterialReciclable.Vidrio);
        a.AgregarMaterial(MaterialReciclable.Metal);
        Assert.Contains(a.Materiales, m => m.Material == MaterialReciclable.Metal);
    }

    [Fact]
    public void Agregar_un_material_repetido_es_regla_de_negocio()
    {
        var a = Asociacion(MaterialReciclable.Vidrio);
        Assert.Throws<ReglaNegocioException>(() => a.AgregarMaterial(MaterialReciclable.Vidrio));
    }

    [Fact]
    public void No_se_puede_quitar_el_ultimo_material_ni_uno_que_no_se_acepta()
    {
        var a = Asociacion(MaterialReciclable.Vidrio);
        Assert.Throws<ReglaNegocioException>(() => a.QuitarMaterial(MaterialReciclable.Vidrio));
        Assert.Throws<ReglaNegocioException>(() => a.QuitarMaterial(MaterialReciclable.Metal));
    }

    [Fact]
    public void Quitar_un_material_cuando_hay_otros_lo_elimina()
    {
        var a = Asociacion(MaterialReciclable.Vidrio, MaterialReciclable.Metal);
        a.QuitarMaterial(MaterialReciclable.Metal);
        Assert.DoesNotContain(a.Materiales, m => m.Material == MaterialReciclable.Metal);
    }

    [Theory]
    [InlineData("Punto Centro", "Av. Ejército 100", -16.4, -71.5, true)]
    [InlineData("", "Av. Ejército 100", -16.4, -71.5, false)]
    [InlineData("Punto Centro", "Av. Ejército 100", 95, -71.5, false)]
    [InlineData("Punto Centro", "Av. Ejército 100", -16.4, 200, false)]
    public void Crear_punto_de_acopio_valida_la_forma(string nombre, string direccion, double lat, double lng, bool valido)
    {
        var validador = Proveedor().GetRequiredService<IValidator<CrearPuntoAcopioCommand>>();
        var resultado = validador.Validate(new CrearPuntoAcopioCommand
        { Nombre = nombre, Direccion = direccion, Latitud = lat, Longitud = lng });
        Assert.Equal(valido, resultado.IsValid);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, true)]
    [InlineData(200, true)]
    [InlineData(201, false)]
    public void Los_listados_limitan_el_maximo_de_filas(int maximo, bool valido)
    {
        var proveedor = Proveedor();
        Assert.Equal(valido, proveedor.GetRequiredService<IValidator<ListarLecturasNivelQuery>>()
            .Validate(new ListarLecturasNivelQuery { DispositivoId = Guid.NewGuid(), Maximo = maximo }).IsValid);
        Assert.Equal(valido, proveedor.GetRequiredService<IValidator<ListarEventosAuditoriaQuery>>()
            .Validate(new ListarEventosAuditoriaQuery { Maximo = maximo }).IsValid);
    }

    [Fact]
    public void Agregar_material_exige_asociacion_y_material_validos()
    {
        var validador = Proveedor().GetRequiredService<IValidator<AgregarMaterialAceptadoCommand>>();
        Assert.False(validador.Validate(new AgregarMaterialAceptadoCommand { AsociacionId = Guid.Empty, Material = MaterialReciclable.Metal }).IsValid);
        Assert.False(validador.Validate(new AgregarMaterialAceptadoCommand { AsociacionId = Guid.NewGuid(), Material = (MaterialReciclable)99 }).IsValid);
    }

    [Fact]
    public async Task La_politica_de_recompensas_devuelve_los_valores_del_dominio()
    {
        var dto = await Proveedor().GetRequiredService<ISender>().Send(new ObtenerPoliticaRecompensasQuery());
        Assert.Equal(PoliticaRecompensas.PuntosPorEntrega, dto.PuntosPorEntrega);
        Assert.Equal(PoliticaRecompensas.MaximoDiario, dto.MaximoDiario);
    }
}
