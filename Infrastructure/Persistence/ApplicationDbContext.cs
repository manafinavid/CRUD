using System.Reflection;
using CRUD.Application.Common.Interfaces;
using CRUD.Domain.Entities;
using CRUD.Domain.Events;
using CRUD.Infrastructure.Identity;
using Duende.IdentityServer.EntityFramework.Options;
using MediatR;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CRUD.Infrastructure.Persistence;

public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>, IApplicationDbContext
{
    private readonly IMediator _mediator;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IOptions<OperationalStoreOptions> operationalStoreOptions,
        IMediator mediator)
        : base(options, operationalStoreOptions)
    {
        _mediator = mediator;
    }

    public DbSet<Product> Products { get; set; }  = null!;
    public DbSet<ProductEvent> ProductEvents { get; set; } = null!;

    public DbSet<ApplicationUser> ApplicationUsers { get; set; } = null!;
    public DbSet<ApplicationUserEvent> ApplicationUserEvents { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUserEvent>()
            .HasOne(c => c.Creator)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<ApplicationUserEvent>()
            .HasOne(c => c.ApplicationUser)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);

    }

}
