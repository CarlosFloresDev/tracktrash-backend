using System.Security.Cryptography;
using TrackTrash.Application.Ports.Services;

namespace TrackTrash.Infrastructure.Adapters.Identity;

public sealed class CodigoEntregaService : ICodigoEntregaService
{
    public string Generar() => RandomNumberGenerator.GetInt32(0, 1_000_000).ToString("D6");
}
