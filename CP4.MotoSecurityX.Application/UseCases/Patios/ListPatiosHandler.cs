using System.Linq;
using CP4.MotoSecurityX.Application.Common;
using CP4.MotoSecurityX.Application.DTOs;
using CP4.MotoSecurityX.Domain.Repositories;

namespace CP4.MotoSecurityX.Application.UseCases.Patios;

public sealed class ListPatiosHandler
{
    private readonly IPatioRepository _repo;
    public ListPatiosHandler(IPatioRepository repo) => _repo = repo;

    public async Task<PagedResult<PatioDto>> HandleAsync(
        int page,
        int pageSize,
        Func<int, int, string> linkFactory,
        CancellationToken ct = default)
    {
        var total = await _repo.CountAsync(ct);
        var itens = await _repo.ListAsync(page, pageSize, ct);

        var data = itens.Select(p => new PatioDto
        {
            Id       = p.Id,
            Nome     = p.Nome,
            Endereco = p.Endereco
        });

        return PagedResult<PatioDto>.Create(data, total, page, pageSize, linkFactory);
    }
}



