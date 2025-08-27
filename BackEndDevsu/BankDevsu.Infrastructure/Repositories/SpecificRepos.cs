using BankDevsu.Application.Abstractions;
using BankDevsu.Domain.Entities;
using BankDevsu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankDevsu.Infrastructure.Repositories
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(BankingDbContext ctx) : base(ctx) { }
    }

    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(BankingDbContext ctx) : base(ctx) { }

        public Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default)
            => _ctx.Accounts.Include(a => a.Client).FirstOrDefaultAsync(a => a.AccountNumber == accountNumber, ct);
    }

    public class MovementRepository : GenericRepository<Movement>, IMovementRepository
    {
        public MovementRepository(BankingDbContext ctx) : base(ctx) { }

        public async Task<decimal> SumDailyDebitsAsync(Guid accountId, DateOnly day, CancellationToken ct = default)
        {
            var from = day.ToDateTime(TimeOnly.MinValue);
            var to = day.ToDateTime(TimeOnly.MaxValue);

            var sum = await _ctx.Movements
                .Where(m => m.AccountId == accountId && m.DateUtc >= from && m.DateUtc <= to && m.Amount < 0)
                .SumAsync(m => -m.Amount, ct);

            return sum;
        }

        public async Task<IReadOnlyList<Movement>> ListByAccountAndDateRangeAsync(Guid accountId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default)
        {
            return await _ctx.Movements
                .Where(m => m.AccountId == accountId && m.DateUtc >= fromUtc && m.DateUtc <= toUtc)
                .OrderBy(m => m.DateUtc)
                .AsNoTracking()
                .ToListAsync(ct);
        }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private readonly BankingDbContext _ctx;
        public UnitOfWork(BankingDbContext ctx) => _ctx = ctx;
        public Task<int> SaveChangesAsync(CancellationToken ct = default) => _ctx.SaveChangesAsync(ct);
    }
}
