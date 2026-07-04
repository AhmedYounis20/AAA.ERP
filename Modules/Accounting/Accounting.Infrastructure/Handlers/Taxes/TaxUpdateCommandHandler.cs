using Accounting.Application.Commands.Taxes;
using Accounting.Application.Services;
using Accounting.Domain.Entities;

namespace Accounting.Infrastructure.Handlers.Taxes;

public class TaxUpdateCommandHandler(ITaxService service)
    : ICommandHandler<TaxUpdateCommand, ApiResponse<Tax>>
{
    public Task<ApiResponse<Tax>> Handle(TaxUpdateCommand request, CancellationToken cancellationToken)
        => service.Update(request);
}
