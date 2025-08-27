using BankDevsu.Domain.Common;
using BankDevsu.Domain.Enums;
using System.Text.Json.Serialization;

namespace BankDevsu.Domain.Entities
{
    public class Movement : BaseEntity
    {
        public DateTime DateUtc { get; set; } = DateTime.UtcNow;
        public MovementType MovementType { get; set; }
        public decimal Amount { get; set; }
        public decimal BalanceAfterTransaction { get; set; }

        public Guid AccountId { get; set; }
        [JsonIgnore]
        public Account? Account { get; set; }
    }
}
