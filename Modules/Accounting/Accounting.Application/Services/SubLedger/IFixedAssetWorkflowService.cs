using Accounting.Application.Commands.FixedAssets;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Write-side workflow for fixed-asset CRUD orchestration (Category C).
/// </summary>
public interface IFixedAssetWorkflowService
{
    Task<ApiResponse<FixedAsset>> Create(FixedAssetCreateCommand command);

    Task<ApiResponse<FixedAsset>> Update(FixedAssetUpdateCommand command);

    Task<ApiResponse<FixedAsset>> Delete(Guid id);
}
