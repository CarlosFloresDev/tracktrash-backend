namespace TrackTrash.Application.Ports.Services;
public interface ISeguridadDispositivoService
{
    string Generar();
    string Hash(string clave);
    bool Validar(string clave,string hash);
}
