using CP4.MotoSecurityX.Domain.Entities;

namespace CP4.MotoSecurityX.Domain.Repositories;

public interface IPatioRepository
{
    Task<Patio?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Patio patio, CancellationToken ct = default);
    Task UpdateAsync(Patio patio, CancellationToken ct = default);

    // novos
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<int> CountAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Patio>> ListAsync(int page, int pageSize, CancellationToken ct = default);
}


