using BankDevsu.Domain.Enums;

namespace BankDevsu.Api.Contracts
{
    public record ClientCreateDto(string Name, Gender Gender, int Age, string Identification, string Address, string Phone, string Password);
    public record ClientUpdateDto(string Name, Gender Gender, int Age, string Address, string Phone, bool IsActive);

    public record AccountCreateDto(Guid ClientId, string AccountNumber, AccountType AccountType, decimal InitialBalance, bool IsActive);
    public record AccountUpdateDto(bool IsActive);

    public record MovementCreateDto(Guid AccountId, MovementType MovementType, decimal Amount, DateTime? DateUtc = null);
    public record ReportQueryDto(Guid ClientId, DateTime FromUtc, DateTime ToUtc, bool AsPdfBase64 = false);
}
