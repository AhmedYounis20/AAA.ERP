using Accounting.Application.Commands.FixedAssets;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Application.Services.Queries;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for fixed-asset GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IFixedAssetQueries : ITreeSettingQueries<FixedAsset>
{
    Task<ApiResponse<FixedAssetCreateCommand>> GetNextFixedAsset(Guid? parentId, FixedAssetType fixedAssetType);
}
