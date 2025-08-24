using BankDevsu.Domain.Common;
using BankDevsu.Domain.Enums;

namespace BankDevsu.Domain.Entities
{
    public class Person : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Gender Gender { get; set; } = Gender.Unknown;
        public int Age { get; set; }
        public string Identification { get; set; } = string.Empty; // unique constraint
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }
}
