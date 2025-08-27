using BankDevsu.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankDevsu.Infrastructure.Persistence
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) { }

        public DbSet<Client> Clients => Set<Client>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Movement> Movements => Set<Movement>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>(e =>
            {
                e.HasIndex(c => c.Identification).IsUnique();
                e.Property(c => c.Name).HasMaxLength(120).IsRequired();
                e.Property(c => c.PasswordHash).HasMaxLength(256).IsRequired();
            });

            modelBuilder.Entity<Account>(e =>
            {
                e.HasIndex(a => a.AccountNumber).IsUnique();
                e.Property(a => a.InitialBalance).HasColumnType("decimal(18,2)");
                e.Property(a => a.CurrentBalance).HasColumnType("decimal(18,2)");
                e.HasOne(a => a.Client)
                 .WithMany()
                 .HasForeignKey(a => a.ClientId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Movement>(e =>
            {
                e.Property(m => m.Amount).HasColumnType("decimal(18,2)");
                e.Property(m => m.BalanceAfterTransaction).HasColumnType("decimal(18,2)");
                e.HasOne(m => m.Account).WithMany(a => a.Movements).HasForeignKey(m => m.AccountId).OnDelete(DeleteBehavior.Cascade);
                e.HasIndex(m => new { m.AccountId, m.DateUtc });
            });
        }
    }
}
