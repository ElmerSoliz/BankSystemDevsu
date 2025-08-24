using BankDevsu.Api.Contracts;
using FluentValidation;

namespace BankDevsu.Api.Validation
{
    public class ClientCreateValidator : AbstractValidator<ClientCreateDto>
    {
        public ClientCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(120);
            RuleFor(x => x.Identification).NotEmpty();
            RuleFor(x => x.Password).MinimumLength(6);
            RuleFor(x => x.Age).InclusiveBetween(0, 120);
        }
    }

    public class AccountCreateValidator : AbstractValidator<AccountCreateDto>
    {
        public AccountCreateValidator()
        {
            RuleFor(x => x.AccountNumber).NotEmpty();
            RuleFor(x => x.InitialBalance).GreaterThanOrEqualTo(0);
        }
    }

    public class MovementCreateValidator : AbstractValidator<MovementCreateDto>
    {
        public MovementCreateValidator()
        {
            RuleFor(x => x.Amount).NotEqual(0);
        }
    }
}
