using System.Security.Cryptography;
using System.Text;
using TrackTrash.Application.Ports.Services;

namespace TrackTrash.Infrastructure.Adapters.Identity;

public sealed class SeguridadDispositivoService : ISeguridadDispositivoService
{
    public string Generar() => Convert.ToHexString(RandomNumberGenerator.GetBytes(32));

    public string Hash(string clave) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(clave)));

    public bool Validar(string clave, string hashEsperado)
    {
        var actual = SHA256.HashData(Encoding.UTF8.GetBytes(clave));
        if (hashEsperado.Length != actual.Length * 2) return false;
        try
        {
            return CryptographicOperations.FixedTimeEquals(actual, Convert.FromHexString(hashEsperado));
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
