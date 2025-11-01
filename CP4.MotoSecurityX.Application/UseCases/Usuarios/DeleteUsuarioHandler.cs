using CP4.MotoSecurityX.Domain.Repositories;

namespace CP4.MotoSecurityX.Application.UseCases.Usuarios;

public class DeleteUsuarioHandler
{
    private readonly IUsuarioRepository _repo;
    public DeleteUsuarioHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<bool> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var u = await _repo.GetByIdAsync(id, ct);
        if (u is null) return false;
        await _repo.DeleteAsync(u, ct);
        return true;
    }
}



