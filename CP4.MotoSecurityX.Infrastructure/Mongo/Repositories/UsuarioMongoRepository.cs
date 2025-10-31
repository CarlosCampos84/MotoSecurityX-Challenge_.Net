using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CP4.MotoSecurityX.Domain.Entities;
using CP4.MotoSecurityX.Domain.Repositories;
using MongoDB.Driver;

namespace CP4.MotoSecurityX.Infrastructure.Mongo.Repositories
{
    public class UsuarioMongoRepository : IUsuarioRepository
    {
        private readonly IMongoCollection<Usuario> _collection;

        public UsuarioMongoRepository(IMongoContext context)
            => _collection = context.Database.GetCollection<Usuario>("usuarios");

        public async Task<Usuario?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _collection.Find(u => u.Id == id).FirstOrDefaultAsync(ct);

        public async Task<Usuario?> GetByEmailAsync(string email, CancellationToken ct = default)
            => await _collection.Find(u => u.Email == email).FirstOrDefaultAsync(ct);

        public async Task<List<Usuario>> ListAsync(int page, int pageSize, CancellationToken ct = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            return await _collection
                .Find(FilterDefinition<Usuario>.Empty)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(ct);
        }

        public async Task<int> CountAsync(CancellationToken ct = default)
        {
            var total = await _collection.CountDocumentsAsync(FilterDefinition<Usuario>.Empty, cancellationToken: ct);
            return (int)total;
        }

        public Task AddAsync(Usuario usuario, CancellationToken ct = default)
            => _collection.InsertOneAsync(usuario, cancellationToken: ct);

        public Task DeleteAsync(Usuario usuario, CancellationToken ct = default)
            => _collection.DeleteOneAsync(u => u.Id == usuario.Id, ct);

        public Task UpdateAsync(Usuario usuario, CancellationToken ct = default)
            => _collection.ReplaceOneAsync(u => u.Id == usuario.Id, usuario, new ReplaceOptions { IsUpsert = false }, ct);
    }
}


