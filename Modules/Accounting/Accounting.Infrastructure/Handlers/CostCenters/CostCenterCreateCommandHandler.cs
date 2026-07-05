using Accounting.Application.Commands.CostCenters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CostCenters;

namespace Accounting.Infrastructure.Handlers.CostCenters;

public class CostCenterCreateCommandHandler(ICostCenterService service)
    : ICommandHandler<CostCenterCreateCommand, ApiResponse<CostCenter>>
{
    public Task<ApiResponse<CostCenter>> Handle(CostCenterCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
