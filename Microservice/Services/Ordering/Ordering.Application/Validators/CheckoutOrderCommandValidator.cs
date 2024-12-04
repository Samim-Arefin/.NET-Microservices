using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators
{
    public class CheckoutOrderCommandValidator : AbstractValidator<CheckoutOrderCommand>
    {
        public CheckoutOrderCommandValidator()
        {
            RuleFor(o => o.UserName)
                .NotEmpty()
                .WithMessage("User name is required.")
                .NotNull()
                .MaximumLength(70)
                .WithMessage("User name must not exceed 70 characters.");

            RuleFor(o => o.TotalPrice)
                .NotEmpty()
                .WithMessage("Total price is required.")
                .GreaterThan(0)
                .WithMessage("Total price should be greater than zero.");

            RuleFor(o => o.EmailAddress)
                .NotEmpty()
                .WithMessage("Email address is required.");

            RuleFor(o => o.FirstName)
                .NotEmpty()
                .NotNull()
                .WithMessage("First name is required");

            RuleFor(o => o.LastName)
                .NotEmpty()
                .NotNull()
                .WithMessage("Last name is required");
            
            RuleFor(o => o.PhoneNumber)
                .NotEmpty()
                .NotNull()
                .WithMessage("Phone number is required");

            RuleFor(o => o.Address)
               .NotEmpty()
               .NotNull()
               .WithMessage("Address is required");

            RuleFor(o => o.City)
               .NotEmpty()
               .NotNull()
               .WithMessage("City is required");

            RuleFor(o => o.State)
               .NotEmpty()
               .NotNull()
               .WithMessage("State is required");

            RuleFor(o => o.ZipCode)
               .NotEmpty()
               .NotNull()
               .WithMessage("Zip code is required");

            RuleFor(o => o.CardName)
               .NotEmpty()
               .NotNull()
               .WithMessage("Card name is required");

            RuleFor(o => o.CardNumber)
               .NotEmpty()
               .NotNull()
               .WithMessage("Card number is required");

            RuleFor(o => o.CVV)
               .NotEmpty()
               .NotNull()
               .WithMessage("CVV is required");

            RuleFor(o => o.Expiration)
               .NotEmpty()
               .NotNull()
               .WithMessage("Expiration date is required");
            
            RuleFor(o => o.CreatedBy)
               .NotNull()
               .WithMessage("Created by is required");
        }
    }
}
