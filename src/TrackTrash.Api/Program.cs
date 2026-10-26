using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using TrackTrash.Infrastructure.Configuration;
using TrackTrash.Infrastructure.Middlewares;
using TrackTrash.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);
builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.WithOrigins(
    builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? []).AllowAnyHeader().AllowAnyMethod()));
builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = 429;
    o.AddPolicy("operaciones", context => RateLimitPartition.GetFixedWindowLimiter(
        (context.User.FindFirst("sub")?.Value ?? context.Connection.RemoteIpAddress?.ToString() ?? "anonimo")+(HttpMethods.IsGet(context.Request.Method)?":lectura":":escritura"),
        _ => new FixedWindowRateLimiterOptions { PermitLimit = HttpMethods.IsGet(context.Request.Method)?120:30, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 }));
});

var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();
// Render termina TLS y solo entrega HTTP al contenedor. Su configuración de
// proceso declara ese contrato; no confiamos en cabeceras enviadas por clientes.
if (RenderHosting.HasManagedHttps(builder.Configuration))
    app.Use((context, next) => { context.Request.Scheme = Uri.UriSchemeHttps; return next(context); });
if (!app.Environment.IsDevelopment()) app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers().RequireRateLimiting("operaciones");
app.MapHealthChecks("/health/live");
app.MapGet("/health/ready", async (TrackTrashDbContext db, CancellationToken ct) =>
    await db.Database.CanConnectAsync(ct) && !(await db.Database.GetPendingMigrationsAsync(ct)).Any()
        ? Results.Ok(new { status = "ready" }) : Results.StatusCode(503));
if (app.Environment.IsDevelopment()) app.MapOpenApi();

if (args.Contains("--migrate") || args.Contains("--seed-pilot"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<TrackTrashDbContext>();
    if (args.Contains("--migrate"))
    {
        await db.Database.MigrateAsync();
        if (app.Environment.IsDevelopment() && builder.Configuration.GetValue<bool>("Authentication:Demo")) await DemoSeed.InicializarAsync(db);
    }
    if (args.Contains("--seed-pilot")) await PilotSeed.InicializarAsync(db, builder.Configuration);
    return;
}
app.Run();

public partial class Program;
