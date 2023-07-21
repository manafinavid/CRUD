using AutoMapper;
using CRUD.Application.Common.Exceptions;
using CRUD.Application.Common.Interfaces;
using CRUD.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CRUD.Application.Users.Commands.Login;

public record LoginCommand : IRequest
{
    public string UserName { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IIdentityService IdentityService;

    public LoginCommandHandler(IApplicationDbContext context, IIdentityService identityService)
    {
        _context = context;
        IdentityService = identityService;
    }

    public async Task Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        await IdentityService.AuthorizeAsync(request.UserName, request.Password);

        await Task.CompletedTask;
    }
}
