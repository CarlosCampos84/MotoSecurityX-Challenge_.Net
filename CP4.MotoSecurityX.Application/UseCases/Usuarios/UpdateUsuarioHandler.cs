using CP4.MotoSecurityX.Application.DTOs;
using CP4.MotoSecurityX.Domain.Repositories;

namespace CP4.MotoSecurityX.Application.UseCases.Usuarios;

public class UpdateUsuarioHandler
{
    private readonly IUsuarioRepository _repo;
    public UpdateUsuarioHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<bool> HandleAsync(Guid id, UpdateUsuarioDto dto, CancellationToken ct = default)
    {
        var u = await _repo.GetByIdAsync(id, ct);
        if (u is null) return false;
        u.AtualizarNome(dto.Nome);
        u.AtualizarEmail(dto.Email);
        await _repo.UpdateAsync(u, ct);
        return true;
    }
}



