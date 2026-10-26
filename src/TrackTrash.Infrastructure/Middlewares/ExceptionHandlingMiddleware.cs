using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrackTrash.Application.Exceptions;
using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Infrastructure.Middlewares;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try { await next(context); }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested) { }
        catch (Exception ex)
        {
            if (context.Response.HasStarted) throw;
            var status = ex switch
            {
                AccesoDenegadoException => 403,
                RecursoNoEncontradoException => 404,
                ConflictoPersistenciaException => 409,
                ReglaNegocioException => 422,
                _ => 500
            };
            if (status == 500) logger.LogError(ex, "Fallo no controlado. TraceId: {TraceId}", context.TraceIdentifier);
            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = status,
                Title = status == 500 ? "No pudimos completar la operación." : ex.Message,
                Extensions = { ["traceId"] = context.TraceIdentifier }
            }, options: null, contentType: "application/problem+json", cancellationToken: context.RequestAborted);
        }
    }
}
