using AutoMapper;
using CRUD.Application.Common.Interfaces;
using CRUD.Domain.Entities;
using MediatR;

namespace CRUD.Application.Users.Commands.SignUp;

public record SignUpCommand : IRequest<string>
{
    public string Email { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string ConfrimPassword { get; init; } = string.Empty;
}

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, string>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService CurrentUserService;
    private readonly IIdentityService IdentityService;

    public SignUpCommandHandler(IApplicationDbContext context,
    ICurrentUserService currentUserService,
    IIdentityService identityService)
    {
        _context = context;
        CurrentUserService = currentUserService;
        IdentityService = identityService;
    }

    public async Task<string> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var entity = new ApplicationUser()
        {
            Email = request.Email,
            UserName = request.UserName
        };

        await IdentityService.CreateUserAsync(entity, request.Password);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
