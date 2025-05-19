using Domain.Account.Commands.Currencies;
using Domain.Account.Models.Entities.CostCenters;


namespace ERP.Infrastracture.Handlers.CostCenters;

public class CostCenterUpdateCommandHandler(ICostCenterService service) : ICommandHandler<CostCenterUpdateCommand, ApiResponse<CostCenter>>
{
    public async Task<ApiResponse<CostCenter>> Handle(CostCenterUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}