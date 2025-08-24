using BankDevsu.Domain.Common;
using BankDevsu.Domain.Enums;

namespace BankDevsu.Domain.Entities
{
    public class Account : BaseEntity
    {
        public string AccountNumber { get; set; } = string.Empty;
        public AccountType AccountType { get; set; }
        public decimal InitialBalance { get; set; }
        public bool IsActive { get; set; } = true;

        public decimal CurrentBalance { get; set; }

        public Guid ClientId { get; set; }
        public Client? Client { get; set; }

        public ICollection<Movement> Movements { get; set; } = new List<Movement>();
    }
}
