using TrackTrash.Domain.Entities;
using TrackTrash.Application.UseCases.Entregas;

namespace TrackTrash.Tests;

public sealed class ArchitectureTests
{
    [Fact]
    public void El_nucleo_no_depende_de_las_capas_externas()
    {
        var domain = typeof(SolicitudRecojo).Assembly.GetReferencedAssemblies().Select(x => x.Name!).ToList();
        var application = typeof(ConfirmarEntregaUseCase).Assembly.GetReferencedAssemblies().Select(x => x.Name!).ToList();
        Assert.DoesNotContain(domain, x => x.StartsWith("TrackTrash.") || x.StartsWith("Microsoft.EntityFrameworkCore"));
        Assert.DoesNotContain(application, x => x is "TrackTrash.Infrastructure" or "TrackTrash.Api" || x.StartsWith("Microsoft.EntityFrameworkCore"));
    }
}
