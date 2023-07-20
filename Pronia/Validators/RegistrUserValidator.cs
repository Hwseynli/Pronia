using FluentValidation;

namespace Pronia.Validators
{
	public class RegistrUserValidator:AbstractValidator<RegistrVM>
	{
		public RegistrUserValidator()
		{
            RuleFor(u => u.Name)
              .NotEmpty()
              .NotNull()
              .WithMessage("Please fill in the Name field")
              .MaximumLength(25)
              .MinimumLength(3)
              .WithMessage("Please enter min 3 max 25 characters");
            RuleFor(u => u.Surname)
              .NotEmpty()
              .NotNull()
              .WithMessage("Please fill in the Surname field")
              .MaximumLength(35)
              .MinimumLength(3)
              .WithMessage("Please enter min 3 max 35 characters");
            RuleFor(u => u.Username)
              .NotEmpty()
              .NotNull()
              .WithMessage("Please fill in the Username field")
              .MaximumLength(50)
              .MinimumLength(3)
              .WithMessage("Please enter min 3 max 50 characters");
            RuleFor(u => u.Email)
              .NotEmpty()
              .NotNull()
              .WithMessage("Please fill in the Email field")
              .MaximumLength(100)
              .MinimumLength(3)
              .WithMessage("Please enter min 3 max 100 characters");
            RuleFor(u => u.Password)
                .NotEmpty()
                .NotNull()
                .WithMessage("Please fill in the Password field");
            RuleFor(u => u.ConfirmPassword)
                .NotEmpty()
                .NotNull()
                .WithMessage("Please fill in the ConfirmPassword field");
            RuleFor(u => u.Gender)
              .NotEmpty()
              .NotNull()
              .WithMessage("Please fill in the Gender field")
              .MaximumLength(25)
              .MinimumLength(3)
              .WithMessage("Please enter min 3 max 25 characters");
        }
	}
}

