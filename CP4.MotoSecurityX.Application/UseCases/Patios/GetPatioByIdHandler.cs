using CP4.MotoSecurityX.Application.DTOs;
using CP4.MotoSecurityX.Domain.Repositories;

namespace CP4.MotoSecurityX.Application.UseCases.Patios;

public sealed class GetPatioByIdHandler
{
    private readonly IPatioRepository _repo;
    public GetPatioByIdHandler(IPatioRepository repo) => _repo = repo;

    public async Task<PatioDto?> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var p = await _repo.GetByIdAsync(id, ct);
        if (p is null) return null;

        return new PatioDto
        {
            Id       = p.Id,
            Nome     = p.Nome,
            Endereco = p.Endereco
        };
    }
}


