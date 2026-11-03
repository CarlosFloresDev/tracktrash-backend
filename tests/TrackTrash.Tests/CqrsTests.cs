using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TrackTrash.Application;
using TrackTrash.Application.Exceptions;
using TrackTrash.Application.Ports.Services;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.ConfirmarEntrega;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Commands.CrearSolicitudRecojo;
using TrackTrash.Application.UseCases.SolicitudesRecojo.Queries.ListarSolicitudesRecojo;
using TrackTrash.Domain.Entities;

namespace TrackTrash.Tests;

public sealed class CqrsTests
{
    private static readonly System.Reflection.Assembly Application = typeof(ConfirmarEntregaCommand).Assembly;

    private static IEnumerable<Type> Requests() => Application.GetTypes()
        .Where(t => t is { IsClass: true, IsAbstract: false } && t.Namespace!.Contains(".UseCases.")
            && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>)));

    [Fact]
    public void Cada_request_tiene_handler_y_cada_command_tiene_validator()
    {
        var tipos = Application.GetTypes();
        foreach (var request in Requests())
        {
            Assert.Contains(tipos, t => t.Name == request.Name + "Handler" && t.Namespace == request.Namespace);
            if (request.Name.EndsWith("Command"))
                Assert.Contains(tipos, t => t.Name == request.Name + "Validator" && t.Namespace == request.Namespace);
        }
    }

    [Fact]
    public void Los_requests_son_inmutables()
    {
        foreach (var request in Requests())
            foreach (var p in request.GetProperties().Where(p => p.DeclaringType == request && p.Name != "EqualityContract"))
                Assert.True(p.SetMethod is null || p.SetMethod.ReturnParameter.GetRequiredCustomModifiers()
                    .Contains(typeof(System.Runtime.CompilerServices.IsExternalInit)), $"{request.Name}.{p.Name} debe ser init");
    }

    [Fact]
    public void Handlers_y_validators_son_internal_sealed()
    {
        foreach (var t in Application.GetTypes().Where(t => t.Namespace!.Contains(".UseCases.")
            && (t.Name.EndsWith("Handler") || t.Name.EndsWith("Validator"))))
            Assert.True(t.IsSealed && !t.IsPublic, t.Name);
    }

    [Fact]
    public async Task Un_request_invalido_lanza_ValidacionException_antes_del_handler()
    {
        var sender = Proveedor().GetRequiredService<ISender>();
        var ex = await Assert.ThrowsAsync<ValidacionException>(() => sender.Send(new CrearSolicitudRecojoCommand
        {
            ClaveSolicitud = Guid.NewGuid(), Material = MaterialReciclable.Vidrio,
            CantidadEstimadaKg = 0, Direccion = "corta", ProgramadaPara = DateTimeOffset.UtcNow.AddDays(1)
        }));
        Assert.Contains(nameof(CrearSolicitudRecojoCommand.CantidadEstimadaKg), ex.Errores.Keys);
        Assert.Contains(nameof(CrearSolicitudRecojoCommand.Direccion), ex.Errores.Keys);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(10001)]
    public async Task La_pagina_fuera_de_rango_es_error_de_validacion(int pagina) =>
        await Assert.ThrowsAsync<ValidacionException>(() =>
            Proveedor().GetRequiredService<ISender>().Send(new ListarSolicitudesRecojoQuery { Pagina = pagina }));

    [Theory]
    [InlineData("12345", false)]
    [InlineData("12345a", false)]
    [InlineData("123456", true)]
    public void El_codigo_de_entrega_debe_tener_seis_digitos(string codigo, bool valido) =>
        Assert.Equal(valido, Proveedor().GetRequiredService<IValidator<ConfirmarEntregaCommand>>()
            .Validate(new ConfirmarEntregaCommand { Id = Guid.NewGuid(), Codigo = codigo }).IsValid);

    private static ServiceProvider Proveedor()
    {
        var services = new ServiceCollection();
        services.AddApplication();
        services.AddSingleton<IUsuarioActualService>(new UsuarioFalso());
        return services.BuildServiceProvider();
    }

    private sealed class UsuarioFalso : IUsuarioActualService
    {
        public Guid Id { get; } = Guid.NewGuid();
        public bool EsReciclador => false;
    }
}
