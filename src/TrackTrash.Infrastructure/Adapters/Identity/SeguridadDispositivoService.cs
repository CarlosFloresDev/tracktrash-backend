using System.Security.Cryptography;
using System.Text;
using TrackTrash.Application.Ports.Services;
namespace TrackTrash.Infrastructure.Adapters.Identity;
public sealed class SeguridadDispositivoService:ISeguridadDispositivoService
{
    public string Generar()=>Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
    public string Hash(string clave)=>Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(clave)));
    public bool Validar(string clave,string hash)=>clave.Length is >= 16 and <= 256 &&
        CryptographicOperations.FixedTimeEquals(Encoding.ASCII.GetBytes(Hash(clave)),Encoding.ASCII.GetBytes(hash));
}
