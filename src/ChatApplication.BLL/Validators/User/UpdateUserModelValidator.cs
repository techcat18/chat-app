using ChatApplication.Shared.Models.User;
using FluentValidation;

namespace ChatApplication.BLL.Validators.User;

public class UpdateUserModelValidator: AbstractValidator<UpdateUserModel>
{
    public UpdateUserModelValidator()
    {
        RuleFor(um => um.Id)
            .NotEmpty().WithMessage("Id must not be empty");
        
        RuleFor(um => um.Email)
            .NotEmpty().WithMessage("Email must not be empty")
            .EmailAddress().WithMessage("Must be a valid email");

        RuleFor(um => um.FirstName)
            .NotEmpty().WithMessage("First name must not be empty");

        RuleFor(um => um.LastName)
            .NotEmpty().WithMessage("Last name must not be empty");

        RuleFor(um => um.Age)
            .GreaterThan(0).WithMessage("Age must be greater than zero");
    }
}