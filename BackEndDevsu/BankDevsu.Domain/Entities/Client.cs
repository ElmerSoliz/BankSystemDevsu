namespace BankDevsu.Domain.Entities
{
    public class Client : Person
    {
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
