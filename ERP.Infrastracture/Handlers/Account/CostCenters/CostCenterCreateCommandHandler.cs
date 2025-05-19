using ERP.Application.Services.Account;
using ERP.Domain.Commands.Account.CostCenters;
using ERP.Domain.Models.Entities.Account.CostCenters;
using Shared;

namespace ERP.Infrastracture.Handlers.Account.CostCenters;

public class CostCenterCreateCommandHandler(ICostCenterService service) : ICommandHandler<CostCenterCreateCommand, ApiResponse<CostCenter>>
{
    public async Task<ApiResponse<CostCenter>> Handle(CostCenterCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}