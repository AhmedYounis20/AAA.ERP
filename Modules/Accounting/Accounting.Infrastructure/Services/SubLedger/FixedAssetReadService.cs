using Accounting.Application.Commands.FixedAssets;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services;

public class FixedAssetReadService(IFixedAssetService fixedAssetService) : IFixedAssetReadService
{
    public Task<ApiResponse<IEnumerable<FixedAsset>>> GetAll() => fixedAssetService.ReadAll();
    public Task<ApiResponse<PaginatedResult<FixedAsset>>> GetPaginated(SubLeadgerFilterDto filter, CancellationToken cancellationToken = default)
        => fixedAssetService.ReadAllPaginated(filter, cancellationToken);
    public Task<ApiResponse<FixedAsset>> GetById(Guid id) => fixedAssetService.ReadById(id);
    public Task<ApiResponse<FixedAssetCreateCommand>> GetNextFixedAsset(Guid? parentId, FixedAssetType fixedAssetType)
        => fixedAssetService.GetNextFixedAsset(parentId, fixedAssetType);

    public async Task<ApiResponse<IEnumerable<FixedAsset>>> GetLevel(int level = 0)
    {
        var items = await fixedAssetService.GetLevel(level);
        return new ApiResponse<IEnumerable<FixedAsset>> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = items };
    }

    public async Task<ApiResponse<IEnumerable<FixedAsset>>> GetChildren(Guid parentId, int level = 0)
    {
        var items = await fixedAssetService.GetChildren(parentId, level);
        return new ApiResponse<IEnumerable<FixedAsset>> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = items };
    }
}
