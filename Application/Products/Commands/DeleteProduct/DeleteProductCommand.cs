using CRUD.Application.Common.Exceptions;
using CRUD.Application.Common.Interfaces;
using CRUD.Domain.Entities;
using CRUD.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Application.Products.Commands.DeleteProduct;

[Authorize(nameof(PermissionNames.DeleteProduct))]
public record DeleteProductCommand : IRequest, IAuthorizationRequirement
{
    public Guid Id { get; init; }
};

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService CurrentUserService;


    public DeleteProductCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        CurrentUserService = currentUserService;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }
        var currentUser = await CurrentUserService.GetCurrentUserAsync();

        entity.IsAvailable = false;

        entity.Events.Add(
            new ProductDeletedEvent(entity, currentUser)
        );

        await _context.SaveChangesAsync(cancellationToken);
    }
}
