using CRUD.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Application.Users.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator(IApplicationDbContext context)
    {

        RuleFor(v => v.UserName)
            .NotEmpty().WithMessage("UserName is required.");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password is required.");

    }
}
