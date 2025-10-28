using Account.Domain.Commands.CostCenters;
using Account.Domain.Models.Entities.CostCenters;
namespace Account.Infrastructure.Handlers.CostCenters;

public class CostCenterUpdateCommandHandler(ICostCenterService service) : ICommandHandler<CostCenterUpdateCommand, ApiResponse<CostCenter>>
{
    public async Task<ApiResponse<CostCenter>> Handle(CostCenterUpdateCommand request, CancellationToken cancellationToken)
    {
        return await service.Update(request);
    }
}
