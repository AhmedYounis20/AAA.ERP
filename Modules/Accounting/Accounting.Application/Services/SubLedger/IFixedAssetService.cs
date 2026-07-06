using Accounting.Application.Commands.FixedAssets;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Fixed-asset default-data helper. CRUD writes are handled by CQRS workflow.
/// </summary>
public interface IFixedAssetService
{
    Task<ApiResponse<FixedAssetCreateCommand>> GetNextFixedAsset(Guid? parentId, FixedAssetType fixedAssetType);
}
