using System.Text.RegularExpressions;
using CRUD.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Application.Users.Commands.SignUp;

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    private readonly IApplicationDbContext _context;

    public SignUpCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.UserName)
            .NotEmpty().WithMessage("UserName is required.")
            .MinimumLength(5).WithMessage("Name must not be larger than 5 characters.")
            .MaximumLength(30).WithMessage("Name must not exceed 30 characters.");

        RuleFor(v => v.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must not be larger than 8 characters.")
            .MaximumLength(30).WithMessage("Password must not exceed 30 characters.");

        RuleFor(v => v.ConfrimPassword)
            .Equal(x => x.Password).WithMessage("Password and ConfrimPassword must be same.");

    }

    public async Task<bool> BeUnique(string name, DateTime date, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(date).ToDateTime(TimeOnly.MinValue);
        return await _context.Products
            .AnyAsync(l => l.Name == name && l.ProductDate == today, cancellationToken);
    }
}
