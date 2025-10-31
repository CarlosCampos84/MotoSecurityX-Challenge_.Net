using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CP4.MotoSecurityX.Domain.Entities;
using CP4.MotoSecurityX.Domain.Repositories;
using MongoDB.Driver;

namespace CP4.MotoSecurityX.Infrastructure.Mongo.Repositories
{
    public class MotoMongoRepository : IMotoRepository
    {
        private readonly IMongoCollection<Moto> _collection;

        public MotoMongoRepository(IMongoContext context)
            => _collection = context.Database.GetCollection<Moto>("motos");

        public async Task<Moto?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _collection.Find(m => m.Id == id).FirstOrDefaultAsync(ct);

        public async Task<IReadOnlyList<Moto>> ListAsync(int page, int pageSize, CancellationToken ct = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var items = await _collection
                .Find(FilterDefinition<Moto>.Empty)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(ct);

            return items; // List<Moto> atende IReadOnlyList<Moto>
        }

        public async Task<int> CountAsync(CancellationToken ct = default)
        {
            var total = await _collection.CountDocumentsAsync(FilterDefinition<Moto>.Empty, cancellationToken: ct);
            return (int)total;
        }

        public Task AddAsync(Moto moto, CancellationToken ct = default)
            => _collection.InsertOneAsync(moto, cancellationToken: ct);

        public Task UpdateAsync(Moto moto, CancellationToken ct = default)
            => _collection.ReplaceOneAsync(m => m.Id == moto.Id, moto, new ReplaceOptions { IsUpsert = false }, ct);

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var result = await _collection.DeleteOneAsync(m => m.Id == id, ct);
            return result.DeletedCount > 0;
        }
    }
}


