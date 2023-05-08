using ChatApplication.Shared.Models.Auth;
using FluentValidation;

namespace ChatApplication.BLL.Validators.Auth;

public class ChangePasswordModelValidator: AbstractValidator<ChangePasswordModel>
{
    public ChangePasswordModelValidator()
    {
        RuleFor(cpm => cpm.Email)
            .NotEmpty().WithMessage("Email must not be empty")
            .EmailAddress().WithMessage("Must be a valid email");

        RuleFor(cpm => cpm.Password)
            .NotEmpty().WithMessage("Password must not be empty");
    }
}