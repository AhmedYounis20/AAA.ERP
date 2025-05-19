using Domain.Account.Commands.Currencies;
using Domain.Account.Models.Entities.CostCenters;
using ERP.Application.Services.Account;
using Shared;

namespace ERP.Infrastracture.Handlers.CostCenters;

public class CostCenterCreateCommandHandler(ICostCenterService service) : ICommandHandler<CostCenterCreateCommand, ApiResponse<CostCenter>>
{
    public async Task<ApiResponse<CostCenter>> Handle(CostCenterCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}