using CP4.MotoSecurityX.Domain.Repositories;

namespace CP4.MotoSecurityX.Application.UseCases.Patios;

public sealed class DeletePatioHandler
{
    private readonly IPatioRepository _repo;
    public DeletePatioHandler(IPatioRepository repo) => _repo = repo;

    public Task<bool> HandleAsync(Guid id, CancellationToken ct = default)
        => _repo.DeleteAsync(id, ct);
}


