using Accounting.Application.Commands.CostCenters;
using Accounting.Application.Services;
using ERP.Domain.Models.Entities.Account.CostCenters;

namespace Accounting.Infrastructure.Handlers.CostCenters;

public class CostCenterCreateCommandHandler(ICostCenterService service)
    : ICommandHandler<CostCenterCreateCommand, ApiResponse<CostCenter>>
{
    public Task<ApiResponse<CostCenter>> Handle(CostCenterCreateCommand request, CancellationToken cancellationToken)
        => service.Create(request);
}
