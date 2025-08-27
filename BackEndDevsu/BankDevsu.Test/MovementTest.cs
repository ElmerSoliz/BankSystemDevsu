using BankDevsu.Domain.Entities;
using BankDevsu.Infrastructure.Repositories;

namespace BankDevsu.Test
{
    public class MovementRepositoryTests : TestBase
    {
        [Fact]
        public async Task SumDailyDebits_ShouldReturnCorrectSum()
        {
            var context = GetDbContext();
            var account = new Account { AccountNumber = "ACC1", CurrentBalance = 500, IsActive = true };
            await context.Accounts.AddAsync(account);

            var m1 = new Movement { Account = account, Amount = -100, DateUtc = DateTime.UtcNow };
            var m2 = new Movement { Account = account, Amount = -50, DateUtc = DateTime.UtcNow };
            await context.Movements.AddRangeAsync(m1, m2);
            await context.SaveChangesAsync();

            var repo = new MovementRepository(context);
            var sum = await repo.SumDailyDebitsAsync(account.Id, DateOnly.FromDateTime(DateTime.UtcNow));

            Assert.Equal(150, sum);
        }
    }
}
