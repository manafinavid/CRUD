using AutoMapper;
using CRUD.Application.Common.Interfaces;
using CRUD.Domain.Entities;
using CRUD.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace CRUD.Application.Products.Commands.CreateProduct;

[Authorize(nameof(PermissionNames.CreateProduct))]
public class CreateProductCommand : IRequest<Guid>, IAuthorizationRequirement
{
    public string? Name { get; set; }
    public DateTime ProductDate { get; set; }
    public string? ManufacturePhone { get; set; }
    public string? ManufactureEmail { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper Mapper;
    private readonly ICurrentUserService CurrentUserService;

    public CreateProductCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
    {
        _context = context;
        Mapper = mapper;
        CurrentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = Mapper.Map<Product>(request);
        var currentUser = await CurrentUserService.GetCurrentUserAsync();

        entity.IsAvailable = true;
        entity.Events.Add(
            new ProductCreatedEvent(entity, currentUser)
        );

        _context.Products.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
