using ChatApplication.Blazor.Models.Auth;
using FluentValidation;

namespace ChatApplication.Blazor.Validators.Auth;

public class LoginModelValidator: AbstractValidator<LoginModel>
{
    public LoginModelValidator()
    {
        RuleFor(lm => lm.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(lm => lm.Password)
            .NotEmpty();
    }
    
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<LoginModel>.CreateWithOptions((LoginModel)model, x => x.IncludeProperties(propertyName)));
        return result.IsValid ? Array.Empty<string>() : result.Errors.Select(e => e.ErrorMessage);
    };
}