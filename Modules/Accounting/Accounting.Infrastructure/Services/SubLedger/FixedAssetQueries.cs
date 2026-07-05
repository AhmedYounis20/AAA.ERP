using Accounting.Application.Commands.FixedAssets;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Fixed-asset read queries. Transitional: delegates to <see cref="IFixedAssetService"/>.
/// </summary>
public class FixedAssetQueries(IFixedAssetService fixedAssetService)
    : BaseTreeSettingQueries<FixedAsset, FixedAssetCreateCommand, FixedAssetUpdateCommand>(fixedAssetService),
      IFixedAssetQueries
{
    public Task<ApiResponse<FixedAssetCreateCommand>> GetNextFixedAsset(Guid? parentId, FixedAssetType fixedAssetType)
        => fixedAssetService.GetNextFixedAsset(parentId, fixedAssetType);
}
