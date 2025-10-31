using CP4.MotoSecurityX.Domain.Repositories;

namespace CP4.MotoSecurityX.Application.UseCases.Motos;

public sealed class MoveMotoToPatioHandler
{
    private readonly IMotoRepository _motoRepo;
    private readonly IPatioRepository _patioRepo;

    public MoveMotoToPatioHandler(IMotoRepository motoRepo, IPatioRepository patioRepo)
    {
        _motoRepo = motoRepo;
        _patioRepo = patioRepo;
    }

    public async Task<bool> HandleAsync(Guid motoId, Guid patioId, CancellationToken ct = default)
    {
        // validações rápidas
        if (motoId == Guid.Empty || patioId == Guid.Empty)
            return false;

        // carrega moto e novo pátio
        var moto = await _motoRepo.GetByIdAsync(motoId, ct);
        if (moto is null) return false;

        var novoPatio = await _patioRepo.GetByIdAsync(patioId, ct);
        if (novoPatio is null) return false;

        // idempotência: já está no mesmo pátio
        if (moto.DentroDoPatio && moto.PatioId == patioId)
            return true;

        // se estava em outro pátio, remove de lá
        if (moto.PatioId is Guid atualPatioId && atualPatioId != patioId)
        {
            var patioAtual = await _patioRepo.GetByIdAsync(atualPatioId, ct);
            if (patioAtual is not null)
            {
                patioAtual.RemoverMoto(moto);
                await _patioRepo.UpdateAsync(patioAtual, ct);
            }
        }

        // admite no novo pátio (aplica invariantes do domínio)
        novoPatio.AdmitirMoto(moto);

        // persiste mudanças (atualiza agregado e entidade)
        await _patioRepo.UpdateAsync(novoPatio, ct);
        await _motoRepo.UpdateAsync(moto, ct);

        return true;
    }
}


