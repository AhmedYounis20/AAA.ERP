using Accounting.Application.Commands.Branches;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Write-side workflow for branch CRUD orchestration (Category C).
/// </summary>
public interface IBranchWorkflowService
{
    Task<ApiResponse<Branch>> Create(BranchCreateCommand command);

    Task<ApiResponse<Branch>> Update(BranchUpdateCommand command);

    Task<ApiResponse<Branch>> Delete(Guid id);
}
