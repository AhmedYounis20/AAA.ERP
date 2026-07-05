using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CostCenters;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services;

public class CostCenterReadService(ICostCenterService costCenterService) : ICostCenterReadService
{
    public Task<ApiResponse<IEnumerable<CostCenter>>> GetAll() => costCenterService.ReadAll();
    public Task<ApiResponse<PaginatedResult<CostCenter>>> GetPaginated(CostCenterFilterDto filter, CancellationToken cancellationToken = default)
        => costCenterService.ReadAllPaginated(filter, cancellationToken);
    public Task<ApiResponse<CostCenter>> GetById(Guid id) => costCenterService.ReadById(id);

    public async Task<ApiResponse<IEnumerable<CostCenter>>> GetLevel(int level = 0)
    {
        var items = await costCenterService.GetLevel(level);
        return new ApiResponse<IEnumerable<CostCenter>> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = items };
    }

    public async Task<ApiResponse<IEnumerable<CostCenter>>> GetChildren(Guid parentId, int level = 0)
    {
        var items = await costCenterService.GetChildren(parentId, level);
        return new ApiResponse<IEnumerable<CostCenter>> { IsSuccess = true, StatusCode = HttpStatusCode.OK, Result = items };
    }
}
