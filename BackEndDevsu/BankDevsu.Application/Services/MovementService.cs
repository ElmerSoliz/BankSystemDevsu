using BankDevsu.Application.Abstractions;
using BankDevsu.Domain.Entities;
using BankDevsu.Domain.Enums;
using BankDevsu.Domain.Exceptions;

namespace BankDevsu.Application.Services
{
    public interface IMovementService
    {
        Task<Movement> CreateAsync(Guid accountId, MovementType type, decimal amount, DateTime? whenUtc = null, CancellationToken ct = default);
    }

    public class MovementService : IMovementService
    {
        private readonly IAccountRepository _accounts;
        private readonly IMovementRepository _movements;
        private readonly IUnitOfWork _uow;
        private const decimal DAILY_WITHDRAWAL_LIMIT = 1000m;

        public MovementService(IAccountRepository accounts, IMovementRepository movements, IUnitOfWork uow)
        {
            _accounts = accounts;
            _movements = movements;
            _uow = uow;
        }

        public async Task<Movement> CreateAsync(Guid accountId, MovementType type, decimal amount, DateTime? whenUtc = null, CancellationToken ct = default)
        {
            var account = await _accounts.GetByIdAsync(accountId, ct) ?? throw new NotFoundException("Account not found");
            if (!account.IsActive) throw new ValidationException("Account is inactive");
            if (type == MovementType.Credit && amount <= 0) throw new ValidationException("Credit amount must be positive");
            if (type == MovementType.Debit && amount >= 0) throw new ValidationException("Debit amount must be negative");

            var absAmount = Math.Abs(amount);
            var now = whenUtc ?? DateTime.UtcNow;

            if (type == MovementType.Debit)
            {
                var today = DateOnly.FromDateTime(now.Date);
                var taken = await _movements.SumDailyDebitsAsync(accountId, today, ct);

                if (taken + absAmount > DAILY_WITHDRAWAL_LIMIT)
                    throw new DailyLimitExceededException("Cupo diario Excedido");

                if (account.CurrentBalance <= 0 || account.CurrentBalance - absAmount < 0)
                    throw new BalanceUnavailableException("Saldo no disponible");
            }

            var newBalance = account.CurrentBalance + amount;
            account.CurrentBalance = newBalance;

            var movement = new Movement
            {
                AccountId = accountId,
                MovementType = type,
                Amount = amount,
                DateUtc = now,
                BalanceAfterTransaction = newBalance
            };

            await _movements.AddAsync(movement, ct);
            await _accounts.UpdateAsync(account, ct);
            await _uow.SaveChangesAsync(ct);

            return movement;
        }
    }
}
