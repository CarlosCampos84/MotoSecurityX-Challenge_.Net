using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CP4.MotoSecurityX.Domain.Entities;
using CP4.MotoSecurityX.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using CP4.MotoSecurityX.Infrastructure.Data;

namespace CP4.MotoSecurityX.Infrastructure.Repositories
{
    public class MotoRepository : IMotoRepository
    {
        private readonly AppDbContext _db;
        public MotoRepository(AppDbContext db) => _db = db;

        public async Task<Moto?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _db.Motos.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id, ct);

        public async Task AddAsync(Moto moto, CancellationToken ct = default)
        {
            await _db.Motos.AddAsync(moto, ct);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Moto moto, CancellationToken ct = default)
        {
            _db.Motos.Update(moto);
            await _db.SaveChangesAsync(ct);
        }

        // ← interface exige bool
        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _db.Motos.FirstOrDefaultAsync(m => m.Id == id, ct);
            if (entity is null) return false;
            _db.Motos.Remove(entity);
            await _db.SaveChangesAsync(ct);
            return true;
        }

        // ← interface exige int
        public async Task<int> CountAsync(CancellationToken ct = default)
            => await _db.Motos.CountAsync(ct);

        // ← interface exige IReadOnlyList<Moto>
        public async Task<IReadOnlyList<Moto>> ListAsync(int page, int pageSize, CancellationToken ct = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var items = await _db.Motos.AsNoTracking()
                .OrderBy(m => m.Modelo)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return items;
        }
    }
}
