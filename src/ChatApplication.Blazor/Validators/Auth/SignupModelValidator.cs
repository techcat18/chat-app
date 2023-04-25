﻿using ChatApplication.Blazor.Models.Auth;
using FluentValidation;

namespace ChatApplication.Blazor.Validators.Auth;

public class SignupModelValidator: AbstractValidator<SignupModel>
{
    public SignupModelValidator()
    {
        RuleFor(sm => sm.Email)
            .NotEmpty().WithMessage("Email must not be empty")
            .EmailAddress().WithMessage("Must be a valid email");

        RuleFor(sm => sm.FirstName)
            .NotEmpty().WithMessage("First name must not be empty");

        RuleFor(sm => sm.LastName)
            .NotEmpty().WithMessage("Last name must not be empty");
        
        RuleFor(sm => sm.Password)
            .NotEmpty().WithMessage("Password must not be empty");
        
        RuleFor(sm => sm.ConfirmedPassword)
            .NotEmpty().WithMessage("Password confirmation must not be empty");
    }
}