using CP4.MotoSecurityX.Application.DTOs;
using CP4.MotoSecurityX.Domain.Repositories;

namespace CP4.MotoSecurityX.Application.UseCases.Motos;

public class UpdateMotoHandler
{
    private readonly IMotoRepository _repo;
    public UpdateMotoHandler(IMotoRepository repo) => _repo = repo;

    public async Task<bool> HandleAsync(Guid id, UpdateMotoDto dto, CancellationToken ct = default)
    {
        var moto = await _repo.GetByIdAsync(id, ct);
        if (moto is null) return false;

        moto.AtualizarModelo(dto.Modelo);
        moto.AtualizarPlaca(dto.Placa);

        await _repo.UpdateAsync(moto, ct);
        return true;
    }
}


