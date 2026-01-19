using BankBackend.DTOs;
using FluentValidation;

namespace BankBackend.Validators
{
    public class TransferValidator : AbstractValidator<TransferDto>
    {
        public TransferValidator()
        {
            RuleFor(x => x.GonderenId)
                .GreaterThan(0).WithMessage("Sender Account ID must be greater than 0.");

            RuleFor(x => x.AliciId)
                .GreaterThan(0).WithMessage("Receiver Account ID must be greater than 0.")
                .NotEqual(x => x.GonderenId).WithMessage("Sender and Receiver accounts cannot be the same.");

            RuleFor(x => x.Miktar)
                .GreaterThan(0).WithMessage("Transfer amount must be positive.")
                .LessThan(1000000).WithMessage("Transfer amount exceeds the limit (1,000,000).");

            RuleFor(x => x.Aciklama)
                .MaximumLength(100).WithMessage("Description cannot exceed 100 characters.");
        }
    }
}
