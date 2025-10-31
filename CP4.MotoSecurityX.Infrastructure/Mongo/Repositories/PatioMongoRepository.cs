using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CP4.MotoSecurityX.Domain.Entities;
using CP4.MotoSecurityX.Domain.Repositories;
using MongoDB.Driver;

namespace CP4.MotoSecurityX.Infrastructure.Mongo.Repositories
{
    public class PatioMongoRepository : IPatioRepository
    {
        private readonly IMongoCollection<Patio> _collection;

        public PatioMongoRepository(IMongoContext context)
            => _collection = context.Database.GetCollection<Patio>("patios");

        public async Task<Patio?> GetByIdAsync(Guid id, CancellationToken ct = default)
            => await _collection.Find(p => p.Id == id).FirstOrDefaultAsync(ct);

        public Task AddAsync(Patio patio, CancellationToken ct = default)
            => _collection.InsertOneAsync(patio, cancellationToken: ct);

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var res = await _collection.DeleteOneAsync(p => p.Id == id, ct);
            return res.DeletedCount > 0;
        }

        public async Task<int> CountAsync(CancellationToken ct = default)
        {
            var total = await _collection.CountDocumentsAsync(FilterDefinition<Patio>.Empty, cancellationToken: ct);
            return (int)total;
        }

        public async Task<IReadOnlyList<Patio>> ListAsync(int page, int pageSize, CancellationToken ct = default)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;

            var items = await _collection
                .Find(FilterDefinition<Patio>.Empty)
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync(ct);

            return items; // List<Patio> cumpre IReadOnlyList<Patio>
        }

        public Task UpdateAsync(Patio patio, CancellationToken ct = default)
            => _collection.ReplaceOneAsync(p => p.Id == patio.Id, patio, new ReplaceOptions { IsUpsert = false }, ct);
    }
}


