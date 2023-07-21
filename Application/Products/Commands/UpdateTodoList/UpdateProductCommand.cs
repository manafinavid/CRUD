using AutoMapper;
using CRUD.Application.Common.Exceptions;
using CRUD.Application.Common.Interfaces;
using CRUD.Application.Products.Commands.CreateProduct;
using CRUD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Application.Products.Commands.UpdateProduct;

[Authorize(nameof(PermissionNames.UpdateProduct))]
public record UpdateProductCommand : CreateProductCommand , IAuthorizationRequirement
{
    public Guid Id { get; init; }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper Mapper;
    private readonly ICurrentUserService CurrentUserService;

    public UpdateProductCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        Mapper = mapper;
        CurrentUserService = currentUserService;
    }

    public async Task<Guid> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        Mapper.Map(request, entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;

    }
}
