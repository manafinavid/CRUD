using System.Reflection;
using CRUD.Application.Common.Interfaces;
using CRUD.Infrastructure.Persistence;
using CRUD.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag;
using NSwag.Generation.Processors.Security;
using ZymLabs.NSwag.FluentValidation;
using CRUD.Application.Products.Commands.CreateProduct;
using CRUD.Application.Products.Commands.DeleteProduct;
using CRUD.Application.Products.Commands.UpdateProduct;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllersWithViews();

        services.AddRazorPages();

        services.AddScoped<FluentValidationSchemaProcessor>(provider =>
        {
            var validationRules = provider.GetService<IEnumerable<FluentValidationRule>>();
            var loggerFactory = provider.GetService<ILoggerFactory>();

            return new FluentValidationSchemaProcessor(provider, validationRules, loggerFactory);
        });

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddOpenApiDocument((configure, serviceProvider) =>
        {
            var fluentValidationSchemaProcessor = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<FluentValidationSchemaProcessor>();

            // Add the fluent validations schema processor
            configure.SchemaProcessors.Add(fluentValidationSchemaProcessor);

            configure.Title = "CRUD API";
            configure.AddSecurity("Authorization", Enumerable.Empty<string>(), new OpenApiSecurityScheme
            {
                Type = OpenApiSecuritySchemeType.Http,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("Authorization"));
        });


        services.AddTransient<AuthorizationHandler<CreateProductCommand>, CreateProductCommandRequirement>();
        services.AddTransient<AuthorizationHandler<UpdateProductCommand>, UpdateProductCommandRequirement>();
        services.AddTransient<AuthorizationHandler<DeleteProductCommand>, DeleteProductCommandRequirement>();

        services.AddAuthorization<IServiceProvider>((opt, serviceProvider) =>
        {
            opt.AddPolicy(PermissionNames.CreateProduct.ToString(),
                options => options.AddRequirements(
                    new CreateProductCommand()
                ));

            opt.AddPolicy(PermissionNames.UpdateProduct.ToString(),
                options => options.AddRequirements(
                    new UpdateProductCommand()
                ));

            opt.AddPolicy(PermissionNames.DeleteProduct.ToString(),
                options => options.AddRequirements(
                    new DeleteProductCommand()
                ));

        });


        services.AddAuthorization();

        services.AddHttpContextAccessor();

        return services;
    }
    public static IServiceCollection AddAuthorization(this IServiceCollection services,
        Action<AuthorizationOptions, IServiceProvider> configure)
    {
        return services.AddAuthorization();
    }
}

