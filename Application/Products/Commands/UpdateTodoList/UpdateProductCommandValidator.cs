using CRUD.Application.Common.Interfaces;
using CRUD.Application.Products.Commands.CreateProduct;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Application.Products.Commands.UpdateProduct;

public class UpdateProductCommandValidator : CreateProductCommandValidator
{
    public UpdateProductCommandValidator(IApplicationDbContext context)
        : base(context) { }
}
