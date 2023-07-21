using CRUD.Application.Common.Models;
using CRUD.Domain.Entities;

namespace CRUD.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string?> GetUserNameAsync(string userId);

    Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(ApplicationUser user, string password);

    Task<Result> DeleteUserAsync(string userId);
}
