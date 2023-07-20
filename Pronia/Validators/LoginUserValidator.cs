using FluentValidation;

namespace Pronia.Validators
{
	public class LoginUserValidator:AbstractValidator<LoginVM>
	{
		public LoginUserValidator()
		{
            RuleFor(u => u.UsernameOrEmail)
             .NotEmpty()
             .NotNull()
             .WithMessage("Please fill in the Name field")
             .MaximumLength(200)
             .MinimumLength(3)
             .WithMessage("Please enter min 3 max 25 characters");

            RuleFor(u => u.Password)
                    .NotEmpty()
                    .NotNull()
                    .WithMessage("Please fill in the Name field");
        }
    }
}

