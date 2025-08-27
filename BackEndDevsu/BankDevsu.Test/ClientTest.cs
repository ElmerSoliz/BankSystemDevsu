using BankDevsu.Domain.Entities;
using BankDevsu.Infrastructure.Repositories;

namespace BankDevsu.Test
{
    public class ClientTests : TestBase
    {
        [Fact]
        public async Task AddClient_ShouldAddSuccessfully()
        {
            var context = GetDbContext();
            var repo = new GenericRepository<Client>(context);

            var client = new Client
            {
                Name = "Test Client",
                Identification = "123456",
                PasswordHash = "hashedpass",
                Age = 30
            };

            await repo.AddAsync(client);
            await context.SaveChangesAsync();
            var saved = await repo.GetByIdAsync(client.Id);

            Assert.NotNull(saved);
            Assert.Equal("Test Client", saved!.Name);
        }

        [Fact]
        public async Task ListClients_ShouldReturnAllClients()
        {
            var context = GetDbContext();
            var repo = new GenericRepository<Client>(context);

            await repo.AddAsync(new Client { Name = "Client 1", Identification = "1", PasswordHash = "pass", Age = 25 });
            await repo.AddAsync(new Client { Name = "Client 2", Identification = "2", PasswordHash = "pass", Age = 28 });
            await context.SaveChangesAsync();

            var clients = await repo.ListAsync();
            Assert.Equal(2, clients.Count);
        }
    }
}
