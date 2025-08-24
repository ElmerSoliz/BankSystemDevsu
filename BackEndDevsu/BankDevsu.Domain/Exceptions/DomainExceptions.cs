namespace BankDevsu.Domain.Exceptions
{
    public class DomainException : Exception { public DomainException(string message) : base(message) { } }
    public class NotFoundException : DomainException { public NotFoundException(string message) : base(message) { } }
    public class ValidationException : DomainException { public ValidationException(string message) : base(message) { } }
    public class BalanceUnavailableException : DomainException { public BalanceUnavailableException(string message) : base(message) { } }
    public class DailyLimitExceededException : DomainException { public DailyLimitExceededException(string message) : base(message) { } }
}
