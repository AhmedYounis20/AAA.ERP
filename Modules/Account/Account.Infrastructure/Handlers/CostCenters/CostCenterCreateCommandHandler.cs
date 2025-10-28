using Account.Domain.Commands.CostCenters;
using Account.Domain.Models.Entities.CostCenters;

namespace Account.Infrastructure.Handlers.CostCenters;

public class CostCenterCreateCommandHandler(ICostCenterService service) : ICommandHandler<CostCenterCreateCommand, ApiResponse<CostCenter>>
{
    public async Task<ApiResponse<CostCenter>> Handle(CostCenterCreateCommand request,
        CancellationToken cancellationToken)
    {
        return await service.Create(request);
    }
}
