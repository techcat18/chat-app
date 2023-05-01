using ChatApplication.BLL.Models.Chat;
using FluentValidation;

namespace ChatApplication.BLL.Validators.Chat;

public class CreateChatModelValidator: AbstractValidator<CreateChatModel>
{
    public CreateChatModelValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name must not be empty");

        RuleFor(c => c.ChatTypeId)
            .NotEmpty().WithMessage("Chat type Id must not be empty");
    }
}