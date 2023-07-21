using AutoMapper;
using CRUD.Application.Common.Interfaces;
using CRUD.Domain.Entities;
using CRUD.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CRUD.Application.Users.Commands.SignOut;

public record SignOutCommand : IRequest { }

public class SignOutCommandHandler : IRequestHandler<SignOutCommand>
{
    private readonly SignInManager<ApplicationUser> SignInManager;
    private readonly ICurrentUserService CurrentUserService;

    public SignOutCommandHandler(SignInManager<ApplicationUser> signInManager, ICurrentUserService currentUserService)
    {
        SignInManager = signInManager;
        CurrentUserService = currentUserService;
    }

    public async Task Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrEmpty(CurrentUserService.UserId))
        {
            var currentUser = await CurrentUserService.GetCurrentUserAsync();
            await SignInManager.SignOutAsync();
        }
    }
}
