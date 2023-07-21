using CRUD.Domain.Entities;
using CRUD.Domain.Events;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    DbSet<ProductEvent> ProductEvents { get; }

    DbSet<ApplicationUser> ApplicationUsers { get; }
    DbSet<ApplicationUserEvent> ApplicationUserEvents { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);

}
