using Accounting.Application.Commands.FixedAssets;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Responses;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Fixed-asset read queries.
/// </summary>
public class FixedAssetQueries(IFixedAssetRepository repository, IFixedAssetService fixedAssetService)
    : BaseTreeSettingRepositoryQueries<FixedAsset>(repository),
      IFixedAssetQueries
{
    public Task<ApiResponse<FixedAssetCreateCommand>> GetNextFixedAsset(
        Guid? parentId,
        FixedAssetType fixedAssetType)
        => fixedAssetService.GetNextFixedAsset(parentId, fixedAssetType);
}
