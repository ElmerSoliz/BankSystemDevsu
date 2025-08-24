using BankDevsu.Domain.Entities;

namespace BankDevsu.Application.Abstractions
{
    public interface IClientRepository : IGenericRepository<Client> { }
    public interface IAccountRepository : IGenericRepository<Account>
    {
        Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default);
    }
    public interface IMovementRepository : IGenericRepository<Movement>
    {
        Task<decimal> SumDailyDebitsAsync(Guid accountId, DateOnly day, CancellationToken ct = default);
        Task<IReadOnlyList<Movement>> ListByAccountAndDateRangeAsync(Guid accountId, DateTime fromUtc, DateTime toUtc, CancellationToken ct = default);
    }
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
