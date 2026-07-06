using Accounting.Application.Commands.CostCenters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CostCenters;

namespace Accounting.Infrastructure.Handlers.CostCenters;

public sealed class CostCenterUpdateCommandHandler(ICostCenterWorkflowService workflow)
    : ICommandHandler<CostCenterUpdateCommand, ApiResponse<CostCenter>>
{
    public Task<ApiResponse<CostCenter>> Handle(
        CostCenterUpdateCommand request,
        CancellationToken cancellationToken)
        => workflow.Update(request);
}
