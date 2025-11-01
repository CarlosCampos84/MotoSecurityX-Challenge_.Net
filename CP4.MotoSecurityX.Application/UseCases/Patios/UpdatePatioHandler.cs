using CP4.MotoSecurityX.Application.DTOs;
using CP4.MotoSecurityX.Domain.Repositories;

namespace CP4.MotoSecurityX.Application.UseCases.Patios;

public class UpdatePatioHandler
{
    private readonly IPatioRepository _repo;
    public UpdatePatioHandler(IPatioRepository repo) => _repo = repo;

    public async Task<bool> HandleAsync(Guid id, UpdatePatioDto dto, CancellationToken ct = default)
    {
        var p = await _repo.GetByIdAsync(id, ct);
        if (p is null) return false;

        p.AtualizarNome(dto.Nome);
        p.AtualizarEndereco(dto.Endereco);

        await _repo.UpdateAsync(p, ct);
        return true;
    }
}


