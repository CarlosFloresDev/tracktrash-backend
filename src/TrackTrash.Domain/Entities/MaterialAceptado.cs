using TrackTrash.Domain.Exceptions;

namespace TrackTrash.Domain.Entities;

public sealed class MaterialAceptado
{
    private MaterialAceptado() { }
    public MaterialAceptado(Guid asociacion, MaterialReciclable material) { AsociacionId=asociacion; Material=material; }
    public Guid AsociacionId { get; private set; }
    public MaterialReciclable Material { get; private set; }
}

