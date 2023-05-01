using ChatApplication.BLL.Models.Message;
using FluentValidation;

namespace ChatApplication.BLL.Validators.Message;

public class CreateMessageModelValidator: AbstractValidator<CreateMessageModel>
{
    public CreateMessageModelValidator()
    {
        RuleFor(m => m.SenderId)
            .NotEmpty().WithMessage("Sender Id must not be empty");

        RuleFor(m => m.ChatId)
            .NotEmpty().WithMessage("Chat Id must not be emtpy");

        RuleFor(m => m.Content)
            .NotEmpty().WithMessage("Content must not be empty");
    }
}