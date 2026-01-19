using BankBackend.Models;
using FluentValidation;

namespace BankBackend.Validators
{
    public class MusteriValidator : AbstractValidator<Musteriler>
    {
        public MusteriValidator()
        {
            RuleFor(x => x.Ad)
                .NotEmpty().WithMessage("Name is required.")
                .Length(2, 50).WithMessage("Name must be between 2 and 50 characters.");

            RuleFor(x => x.Soyad)
                .NotEmpty().WithMessage("Surname is required.")
                .Length(2, 50).WithMessage("Surname must be between 2 and 50 characters.");

            RuleFor(x => x.TcKimlikNo)
                .NotEmpty().WithMessage("Identity Number (TC) is required.")
                .Length(11).WithMessage("Identity Number must be exactly 11 digits.")
                .Matches("^[0-9]*$").WithMessage("Identity Number must consist of digits only.");

            RuleFor(x => x.Sifre)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
