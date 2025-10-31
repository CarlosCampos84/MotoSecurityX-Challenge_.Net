using CP4.MotoSecurityX.Domain.Repositories;

namespace CP4.MotoSecurityX.Application.UseCases.Motos;

public sealed class DeleteMotoHandler
{
    private readonly IMotoRepository _repo;
    public DeleteMotoHandler(IMotoRepository repo) => _repo = repo;

    // o repositório espera Guid; deletar diretamente por id
    public Task<bool> HandleAsync(Guid id, CancellationToken ct = default)
        => _repo.DeleteAsync(id, ct);
}


