using System.Security.Claims;

using CRUD.Application.Common.Interfaces;
using CRUD.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CRUD.WebApi.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IApplicationDbContext DbContext;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IApplicationDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        DbContext = dbContext;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub");
    public Task<ApplicationUser> GetCurrentUserAsync()
        => DbContext.ApplicationUsers.SingleAsync(x => x.Id == UserId);
}
