using BankDevsu.Application.Abstractions;
using BankDevsu.Domain.Common;
using BankDevsu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BankDevsu.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected readonly BankingDbContext _ctx;
        public GenericRepository(BankingDbContext ctx) => _ctx = ctx;

        public async Task AddAsync(T entity, CancellationToken ct = default) => await _ctx.Set<T>().AddAsync(entity, ct);

        public async Task DeleteAsync(T entity, CancellationToken ct = default)
        {
            _ctx.Set<T>().Remove(entity);
            await Task.CompletedTask;
        }

        public IQueryable<T> Query() => _ctx.Set<T>().AsQueryable();

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default) => await _ctx.Set<T>().FindAsync(new object?[] { id }, ct);

        public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
        {
            IQueryable<T> q = _ctx.Set<T>();
            if (predicate != null) q = q.Where(predicate);
            return await q.AsNoTracking().ToListAsync(ct);
        }

        public Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            _ctx.Set<T>().Update(entity);
            return Task.CompletedTask;
        }
    }
}
