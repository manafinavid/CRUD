using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CRUD.Application.Common.Interfaces;
using CRUD.Application.Common.Models;
using CRUD.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CRUD.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
    private readonly IAuthorizationService _authorizationService;
    private readonly SignInManager<ApplicationUser> SignInManager;
    private readonly IConfiguration Configuration;
    private IHttpContextAccessor HttpContextAccessor;
    private HttpContext HttpContext => HttpContextAccessor.HttpContext ?? throw new NullReferenceException("HttpContext is null");

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
        IAuthorizationService authorizationService,
        SignInManager<ApplicationUser> singinManager,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        _authorizationService = authorizationService;
        SignInManager = singinManager;
        Configuration = configuration;
        HttpContextAccessor = httpContextAccessor;
    }

    public async Task<string?> GetUserNameAsync(string userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        return user.UserName;
    }

    public async Task<(Result Result, string UserId)> CreateUserAsync(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);

        await _userManager.AddClaimsAsync(user,
        new List<Claim>()
        {
            new Claim(PermissionNames.User.ToString(),PermissionNames.User.ToString()),
            new Claim(PermissionNames.CreateProduct.ToString(),PermissionNames.CreateProduct.ToString()),
            new Claim(PermissionNames.UpdateProduct.ToString(),PermissionNames.UpdateProduct.ToString()),
            new Claim(PermissionNames.DeleteProduct.ToString(),PermissionNames.DeleteProduct.ToString()),
        });

        return (result.ToApplicationResult(), user.Id);
    }

    public async Task<bool> IsInRoleAsync(string userId, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null && await _userManager.IsInRoleAsync(user, role);
    }

    public async Task<bool> AuthorizeAsync(string username, string password)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.UserName == username);
        if (user != null)
        {
            var isValid = await SignInManager.PasswordSignInAsync(user, password, true, false);

            var JWToken = new JwtSecurityToken(
                    claims: GetUserClaims(user),
                    notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                    expires: new DateTimeOffset(DateTime.Now.AddDays(1)).DateTime,
                    signingCredentials: new SigningCredentials
                            (new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(Configuration["IdentityServer:Jwt:SecretKey"] ?? string.Empty)
                            ), SecurityAlgorithms.HmacSha256Signature)
                );
            var token = new JwtSecurityTokenHandler().WriteToken(JWToken);

            //HttpContext.Response.Cookies
            HttpContext.Response.Cookies.Append("Bearer", token);

            return isValid.Succeeded;
        }

        return false;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

        return user != null ? await DeleteUserAsync(user) : Result.Success();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
        var result = await _userManager.DeleteAsync(user);

        return result.ToApplicationResult();
    }

    private Claim[] GetUserClaims(ApplicationUser user)
        => new Claim[]
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim("Email", user.Email ?? string.Empty),
            new Claim("UserName" , user.UserName ?? string.Empty),
        };
}
