using CRUD.Application.Common.Interfaces;
using CRUD.Application.Products.Commands.UpdateProduct;
using CRUD.Domain.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CRUD.Application.Products.Commands.CreateProduct;

public class UpdateProductCommandRequirement : AuthorizationHandler<UpdateProductCommand>
{
    readonly IServiceProvider ServiceProvider;
    public UpdateProductCommandRequirement(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UpdateProductCommand requirement)
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            var Dbcontext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var CurrentUserService = scope.ServiceProvider.GetRequiredService<ICurrentUserService>();

            if (context.User.HasClaim(c => c.Type == PermissionNames.Administrator.ToString()))
                context.Succeed(requirement);

            else if (context.User.HasClaim(c => c.Type == PermissionNames.UpdateProduct.ToString()))
            {
                var amICreator = await Dbcontext.ProductEvents.Where(x => x is ProductUpdateEvent &&
                                                x.ProductId == Guid.Parse(requirement.Id) &&
                                                x.CreatorId == CurrentUserService.UserId
                                                )
                                        .AnyAsync();
                if (amICreator)
                {
                    context.Succeed(requirement);
                }
            }
        }
        return;
    }
}
