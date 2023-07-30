using System.ComponentModel.DataAnnotations;
using FluentValidation;
using MediatR;
using ValidationException = CRUD.Application.Common.Exceptions.ValidationException;

namespace CRUD.Application.Common.Behaviours;

public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        if (_validators.Any())
        {
            var context = new ValidationContext<TRequest>(request);

            List<FluentValidation.Results.ValidationResult> validationResults = new();
            foreach (var validator in _validators)
            {
                validationResults.Add(await validator.ValidateAsync(context, cancellationToken));
            }

            var failures = validationResults
                .Where(r => r.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();

            if (failures.Any())
                throw new ValidationException(failures);
        }
        return await next();
    }
}
