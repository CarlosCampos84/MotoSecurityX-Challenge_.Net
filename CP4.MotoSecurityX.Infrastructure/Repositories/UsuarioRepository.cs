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
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _db;
        public UsuarioRepository(AppDbContext db) => _db = db;

        public async Task<Usuario?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _db.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);

        public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct = default)
            => await _db.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email, ct);

        // ← sua interface pede List<Usuario>
        public async Task<List<Usuario>> ListAsync(int page, int pageSize, CancellationToken ct = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            return await _db.Usuarios.AsNoTracking()
                .OrderBy(u => u.Nome)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
        }

        // ← interface exige int
        public async Task<int> CountAsync(CancellationToken ct = default)
            => await _db.Usuarios.CountAsync(ct);

        public async Task AddAsync(Usuario usuario, CancellationToken ct = default)
        {
            await _db.Usuarios.AddAsync(usuario, ct);
            await _db.SaveChangesAsync(ct);
        }

        // interface: DeleteAsync(Usuario)
        public async Task DeleteAsync(Usuario usuario, CancellationToken ct = default)
        {
            _db.Usuarios.Remove(usuario);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(Usuario usuario, CancellationToken ct = default)
        {
            _db.Usuarios.Update(usuario);
            await _db.SaveChangesAsync(ct);
        }
    }
}
