using CRUD.Application.Common.Interfaces;
using CRUD.Application.Products.Commands.CreateProduct;
using CRUD.Domain.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Application.Products.Commands.CreateProduct;

public class CreateProductCommandRequirement : AuthorizationHandler<CreateProductCommand>
{
    readonly IServiceProvider ServiceProvider;
    public CreateProductCommandRequirement(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CreateProductCommand requirement)
    {
        await Task.CompletedTask;

        if (context.User.HasClaim(c => c.Type == PermissionNames.Administrator.ToString()))
            context.Succeed(requirement);

        else if (context.User.HasClaim(c => c.Type == PermissionNames.CreateProduct.ToString()))
        {
            context.Succeed(requirement);
        }

        return;
    }
}
