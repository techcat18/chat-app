using ChatApplication.Shared.Models.Chat;
using FluentValidation;

namespace ChatApplication.BLL.Validators.Chat;

public class UpdateChatModelValidator: AbstractValidator<UpdateChatModel>
{
    public UpdateChatModelValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name must not be empty");

        RuleFor(c => c.ChatTypeId)
            .NotEmpty().WithMessage("Chat type Id must not be empty");
    }
}