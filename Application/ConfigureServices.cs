using System.Reflection;
using AutoMapper.Internal;
using CRUD.Application.Common.Behaviours;
using CRUD.Application.Common.Mappings;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(opt =>
        {
            opt.Internal().MethodMappingEnabled = false;
            opt.AddProfile<MappingProfile>();
        });

        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        });

        return services;
    }

    public static IServiceCollection AddAuthorization<TDep>(
    this IServiceCollection services,
    Action<AuthorizationOptions, TDep> configure) where TDep : class
    {
        services.AddOptions<AuthorizationOptions>().Configure<TDep>(configure);
        return services.AddAuthorization();
    }

}
