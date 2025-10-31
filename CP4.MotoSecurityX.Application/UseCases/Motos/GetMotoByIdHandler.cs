using CP4.MotoSecurityX.Application.DTOs;
using CP4.MotoSecurityX.Domain.Repositories;

namespace CP4.MotoSecurityX.Application.UseCases.Motos;

public sealed class GetMotoByIdHandler
{
    private readonly IMotoRepository _repo;
    public GetMotoByIdHandler(IMotoRepository repo) => _repo = repo;

    public async Task<MotoDto?> HandleAsync(Guid id, CancellationToken ct = default)
    {
        var m = await _repo.GetByIdAsync(id, ct);
        return m is null ? null : new MotoDto(m.Id, m.Placa.Value, m.Modelo, m.DentroDoPatio, m.PatioId);
    }
}


