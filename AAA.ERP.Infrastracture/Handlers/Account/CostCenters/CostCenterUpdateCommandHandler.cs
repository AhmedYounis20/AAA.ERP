using ERP.Domain.Commands.Account.CostCenters;
using ERP.Domain.Models.Entities.Account.CostCenters;


namespace ERP.Infrastracture.Handlers.Account.CostCenters;

public class CostCenterUpdateCommandHandler(ICostCenterService service) : ICommandHandler<CostCenterUpdateCommand, ApiResponse<CostCenter>>
{
    public async Task<ApiResponse<CostCenter>> Handle(CostCenterUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}