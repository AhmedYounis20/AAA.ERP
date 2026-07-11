using Accounting.Application.Commands.CostCenters;
using Accounting.Domain.Models.Entities.CostCenters;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Write-side workflow for cost center CRUD orchestration (Category C).
/// </summary>
public interface ICostCenterWorkflowService
{
    Task<ApiResponse<CostCenter>> Create(CostCenterCreateCommand command);

    Task<ApiResponse<CostCenter>> Update(CostCenterUpdateCommand command);

    Task<ApiResponse<CostCenter>> Delete(Guid id);
}
