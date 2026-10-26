using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TrackTrash.Application.UseCases.Rutas;
namespace TrackTrash.Infrastructure.BackgroundJobs;

public sealed class RecordatoriosWorker(IServiceScopeFactory scopes, ILogger<RecordatoriosWorker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(30));
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try { using var scope = scopes.CreateScope(); await scope.ServiceProvider.GetRequiredService<ProcesarRecordatoriosUseCase>().ExecuteAsync(stoppingToken); }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested) { break; }
            catch (Exception e) { logger.LogWarning(e, "No se completó el ciclo de recordatorios; se reintentará."); }
        }
    }
}
