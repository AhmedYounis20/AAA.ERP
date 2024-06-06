using FluentValidation;
using Mapster;
using MediatR;
using Serilog;
using Shared.BaseEntities;
using Shared.Responses;

namespace Shared.Behaviors;

public class ValidationBehavior<TRequest, TResult> : IPipelineBehavior<TRequest,TResult>
    where TRequest : IRequest<TResult>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }
    
    public async Task<TResult> Handle(TRequest request, RequestHandlerDelegate<TResult> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators.Select(e => e.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults
            .SelectMany(e => e.Errors)
            .Where(e => e != null)
            .ToList();
        
        if (failures.Any())
        {
            Log.Error($"Failed In Input Validation Of {typeof(TRequest)}");
            var response = Activator.CreateInstance<TResult>();

            var res = new ApiResponse<BaseEntity>();
            res.IsSuccess = false;
            res.StatusCode = System.Net.HttpStatusCode.BadRequest;
            res.ErrorMessages = failures.Select(e=>e.ErrorMessage.ToString()).ToList();
            response = res.Adapt<TResult>();
            return response;
        }

        return await next();
    }
}