using System.Reflection;
using CRUD.Application.Common.Exceptions;
using CRUD.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace CRUD.Application.Common.Behaviours;

public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;
    private readonly IServiceProvider ServiceProvider;

    public AuthorizationBehaviour(
        ICurrentUserService currentUserService,
        IIdentityService identityService,
        IServiceProvider serviceProvider)
    {
        _currentUserService = currentUserService;
        _identityService = identityService;
        ServiceProvider = serviceProvider;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>();

        if (authorizeAttributes.Any())
        {
            if (_currentUserService.UserId == null)
            {
                throw new UnauthorizedAccessException();
            }
        }

        return await next();
    }
}
