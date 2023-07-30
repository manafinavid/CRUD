using AutoMapper;
using CRUD.Application.Common.Exceptions;
using CRUD.Application.Common.Interfaces;
using CRUD.Domain.Entities;
using CRUD.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Application.Products.Commands.GetProduct;


public record ProductViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime ProductDate { get; set; }
    public string ManufacturePhone { get; set; } = string.Empty;
    public string ManufactureEmail { get; set; } = string.Empty;
}
public class GetProductCommand : IRequest<List<ProductViewModel>>
{
    public string Id { get; set; } = string.Empty;
    public string Term { get; set; } = string.Empty;
    public string FromDate { get; set; } = DateTime.UtcNow.AddYears(-1000).ToShortDateString();
    public string ToDate { get; set; } = DateTime.UtcNow.ToShortDateString();
    public int ResultPerPage { get; set; } = 20;
    public int Page { get; set; } = 1;
};

public class GetProductCommandHandler : IRequestHandler<GetProductCommand, List<ProductViewModel>>
{

    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService CurrentUserService;
    private readonly IMapper Mapper;


    public GetProductCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
    {
        _context = context;
        CurrentUserService = currentUserService;
        Mapper = mapper;
    }
    public async Task<List<ProductViewModel>> Handle(GetProductCommand request, CancellationToken cancellationToken)
    {
        var term = string.IsNullOrEmpty(request.Term) ? string.Empty :
                         string.Join("AND", request.Term?.Split() ?? new string[0]);
        var query = _context.Products.Where(c => c.IsAvailable).AsQueryable();

        if (!string.IsNullOrEmpty(request.Id))
        {
            query = query.Where(x => x.Id == Guid.Parse(request.Id));
        }
        if (!string.IsNullOrEmpty(term))
        {
            query = query.Where(x => EF.Functions.Contains(x.Name, term));
        }
        if (!string.IsNullOrEmpty(request.FromDate))
        {
            var d = DateTime.Parse(request.FromDate);
            query = query.Where(x => x.ProductDate >= d);
        }
        if (!string.IsNullOrEmpty(request.ToDate))
        {
            var d = DateTime.Parse(request.ToDate);
            query = query.Where(x => x.ProductDate <= d);
        }

        var results = await query.OrderByDescending(x => x.ProductDate)
                .Skip(request.ResultPerPage * (request.Page - 1))
                .Take(request.ResultPerPage)
                .ToListAsync();

        return Mapper.Map<List<Product>, List<ProductViewModel>>(results);
    }


}
