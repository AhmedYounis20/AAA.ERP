using Accounting.Application.Commands.CostCenters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CostCenters;

namespace Accounting.Infrastructure.Handlers.CostCenters;

public sealed class CostCenterDeleteCommandHandler(ICostCenterWorkflowService workflow)
    : ICommandHandler<CostCenterDeleteCommand, ApiResponse<CostCenter>>
{
    public Task<ApiResponse<CostCenter>> Handle(
        CostCenterDeleteCommand request,
        CancellationToken cancellationToken)
        => workflow.Delete(request.Id);
}
