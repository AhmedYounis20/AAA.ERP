using Accounting.Application.Commands.Taxes;
using Accounting.Application.Services;
using Accounting.Domain.Entities;

namespace Accounting.Infrastructure.Handlers.Taxes;

public class TaxCreateCommandHandler(ITaxService service)
    : ICommandHandler<TaxCreateCommand, ApiResponse<Tax>>
{
    public Task<ApiResponse<Tax>> Handle(TaxCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
