using CP4.MotoSecurityX.Domain.Entities;

namespace CP4.MotoSecurityX.Domain.Repositories;

public interface IMotoRepository
{
    Task<Moto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Moto moto, CancellationToken ct = default);
    Task UpdateAsync(Moto moto, CancellationToken ct = default);

    // novos
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<int> CountAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Moto>> ListAsync(int page, int pageSize, CancellationToken ct = default);
}


