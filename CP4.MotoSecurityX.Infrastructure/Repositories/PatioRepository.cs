using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CP4.MotoSecurityX.Domain.Entities;
using CP4.MotoSecurityX.Domain.Repositories;
using CP4.MotoSecurityX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CP4.MotoSecurityX.Infrastructure.Repositories
{
    public class PatioRepository : IPatioRepository
    {
        private readonly AppDbContext _db;
        public PatioRepository(AppDbContext db) => _db = db;

        public async Task<Patio?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _db.Patios.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, ct);

        public async Task AddAsync(Patio patio, CancellationToken ct = default)
        {
            await _db.Patios.AddAsync(patio, ct);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Patio patio, CancellationToken ct = default)
        {
            _db.Patios.Update(patio);
            await _db.SaveChangesAsync(ct);
        }

        // ← interface exige bool
        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _db.Patios.FirstOrDefaultAsync(p => p.Id == id, ct);
            if (entity is null) return false;
            _db.Patios.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        // ← interface exige int
        public async Task<int> CountAsync(CancellationToken ct = default)
            => await _db.Patios.CountAsync(ct);

        // ← interface exige IReadOnlyList<Patio>
        public async Task<IReadOnlyList<Patio>> ListAsync(int page, int pageSize, CancellationToken ct = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var items = await _db.Patios.AsNoTracking()
                .OrderBy(p => p.Nome)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return items;
        }
    }
}
