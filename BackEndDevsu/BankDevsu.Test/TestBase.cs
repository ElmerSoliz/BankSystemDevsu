using BankDevsu.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BankDevsu.Test
{
    public class TestBase
    {
        protected BankingDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<BankingDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new BankingDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }
    }
}

