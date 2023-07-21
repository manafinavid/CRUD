using CRUD.Domain.Entities;

namespace CRUD.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    Task<ApplicationUser> GetCurrentUserAsync();
}
