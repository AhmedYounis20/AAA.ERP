using Accounting.Application.Commands.FixedAssets;
using Accounting.Application.Dtos.Filters;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

public interface IFixedAssetReadService
{
    Task<ApiResponse<IEnumerable<FixedAsset>>> GetAll();
    Task<ApiResponse<PaginatedResult<FixedAsset>>> GetPaginated(SubLeadgerFilterDto filter, CancellationToken cancellationToken = default);
    Task<ApiResponse<FixedAsset>> GetById(Guid id);
    Task<ApiResponse<FixedAssetCreateCommand>> GetNextFixedAsset(Guid? parentId, FixedAssetType fixedAssetType);
    Task<ApiResponse<IEnumerable<FixedAsset>>> GetLevel(int level = 0);
    Task<ApiResponse<IEnumerable<FixedAsset>>> GetChildren(Guid parentId, int level = 0);
}
