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
public class UpdateProductCommand : IRequest<Guid>, IAuthorizationRequirement
{
    public string Id { get; set; } = string.Empty;
    public string? Name { get; set; }
    public DateTime ProductDate { get; set; }
    public string? ManufacturePhone { get; set; }
    public string? ManufactureEmail { get; set; }
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
        var entity = await _context.Products.SingleOrDefaultAsync(x => x.Id == Guid.Parse(request.Id), cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        Mapper.Map(request, entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;

    }
}
