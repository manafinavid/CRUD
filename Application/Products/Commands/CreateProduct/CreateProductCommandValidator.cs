using System.Text.RegularExpressions;
using CRUD.Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Application.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(5).WithMessage("Name must not be less than 5 characters.")
            .MaximumLength(200).WithMessage("Name must not exceed 200 characters.");

        RuleFor(v => v.ProductDate)
            .LessThan(DateTime.UtcNow.AddDays(1))
            .WithMessage("ProductDate must be lower.");

        RuleFor(v => v.ManufacturePhone)
            .NotEmpty().WithMessage("Manufacture Phone is required.")
            .NotNull().WithMessage("Manufacture Phone is required.")
            .MinimumLength(10).WithMessage("ManufacturePhone must not be less than 10 characters.")
            .MaximumLength(15).WithMessage("ManufacturePhone must not exceed 15 characters.");

        RuleFor(v => v.ManufactureEmail)
            .NotEmpty().WithMessage("Manufacture Email is required.")
            .NotNull().WithMessage("Manufacture Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(v => new { v.Name, v.ProductDate })
            .MustAsync(async (o, v) => await BeUnique(o.Name!, o.ProductDate, v));
    }

    public async Task<bool> BeUnique(string name, DateTime date, CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(date).ToDateTime(TimeOnly.MinValue);
        return !await _context.Products
            .AnyAsync(l => l.Name == name && l.ProductDate == today, cancellationToken);
    }
}
