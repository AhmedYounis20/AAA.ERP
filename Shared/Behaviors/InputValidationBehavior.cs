using FluentValidation;
using MediatR;
using Serilog;
using Shared.Responses;

namespace Shared.Behaviors;

public class ValidationBehavior<TRequest,TResponse>
                :IPipelineBehavior<TRequest,ApiResponse<TResponse>>
                where TRequest : ICommand<ApiResponse<TResponse>>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) 
        => _validators = validators;
    
    public async Task<ApiResponse<TResponse>> Handle(TRequest request, RequestHandlerDelegate<ApiResponse<TResponse>> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            _validators
                .Select(e => e.ValidateAsync(context, cancellationToken))
        );

        var failures = validationResults.Where(e => !e.IsValid && e.Errors.Any()).SelectMany(e => e.Errors).ToList();
        
        if (failures.Any())
        {
            Log.Error($"Faild In Input Validation Of {typeof(TRequest)}");
            return new ApiResponse<TResponse>
            {
                IsSuccess = false,
                ErrorMessages = failures.Select(e=>e.ErrorMessage).ToList()
            };
        }

        return await next();
    }

}