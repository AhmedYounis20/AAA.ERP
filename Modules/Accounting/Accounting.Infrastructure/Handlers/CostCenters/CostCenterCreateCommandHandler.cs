using Accounting.Application.Commands.CostCenters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CostCenters;

namespace Accounting.Infrastructure.Handlers.CostCenters;

public sealed class CostCenterCreateCommandHandler(ICostCenterWorkflowService workflow)
    : ICommandHandler<CostCenterCreateCommand, ApiResponse<CostCenter>>
{
    public Task<ApiResponse<CostCenter>> Handle(
        CostCenterCreateCommand request,
        CancellationToken cancellationToken)
        => workflow.Create(request);
}
