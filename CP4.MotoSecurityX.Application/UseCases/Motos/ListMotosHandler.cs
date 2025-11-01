using System.Linq;
using CP4.MotoSecurityX.Application.Common;
using CP4.MotoSecurityX.Application.DTOs;
using CP4.MotoSecurityX.Domain.Repositories;

namespace CP4.MotoSecurityX.Application.UseCases.Motos;

public sealed class ListMotosHandler
{
    private readonly IMotoRepository _repo;
    public ListMotosHandler(IMotoRepository repo) => _repo = repo;

    public async Task<PagedResult<MotoDto>> HandleAsync(
        int page,
        int pageSize,
        Func<int, int, string> linkFactory,
        CancellationToken ct = default)
    {
        var total = await _repo.CountAsync(ct);
        var itens = await _repo.ListAsync(page, pageSize, ct);

        var data = itens.Select(m => new MotoDto
        {
            Id            = m.Id,
            Placa         = m.Placa.Value,
            Modelo        = m.Modelo,
            DentroDoPatio = m.DentroDoPatio,
            PatioId       = m.PatioId
        });

        return PagedResult<MotoDto>.Create(data, total, page, pageSize, linkFactory);
    }
}


