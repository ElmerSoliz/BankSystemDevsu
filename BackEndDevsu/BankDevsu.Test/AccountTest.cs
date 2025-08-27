using BankDevsu.Domain.Entities;
using BankDevsu.Infrastructure.Repositories;

namespace BankDevsu.Test
{
    public class AccountRepositoryTests : TestBase
    {
        [Fact]
        public async Task AddAccount_Should_SaveSuccessfully()
        {
            var context = GetDbContext();
            var repo = new AccountRepository(context);

            var account = new Account
            {
                AccountNumber = "ACC001",
                InitialBalance = 100,
                CurrentBalance = 100,
                IsActive = true
            };

            await repo.AddAsync(account);
            await context.SaveChangesAsync();

            var saved = await repo.GetByIdAsync(account.Id);
            Assert.NotNull(saved);
            Assert.Equal("ACC001", saved!.AccountNumber);
        }

        [Fact]
        public async Task GetByAccountNumber_ShouldReturnAccount()
        {
            var context = GetDbContext();

            var client = new Client { Name = "Alice", Identification = "111", PasswordHash = "pass", Age = 30 };
            await context.Clients.AddAsync(client);

            var account = new Account { AccountNumber = "ACC002", InitialBalance = 200, CurrentBalance = 200, IsActive = true, Client = client };
            await context.Accounts.AddAsync(account);
            await context.SaveChangesAsync();

            var repo = new AccountRepository(context);
            var found = await repo.GetByAccountNumberAsync("ACC002");

            Assert.NotNull(found);
            Assert.Equal("ACC002", found!.AccountNumber);
            Assert.Equal("Alice", found.Client!.Name);
        }
    }
}
