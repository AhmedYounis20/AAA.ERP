using Accounting.Application.Dtos.Filters;
using Accounting.Domain.Models.Entities.CostCenters;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

public interface ICostCenterReadService
{
    Task<ApiResponse<IEnumerable<CostCenter>>> GetAll();
    Task<ApiResponse<PaginatedResult<CostCenter>>> GetPaginated(CostCenterFilterDto filter, CancellationToken cancellationToken = default);
    Task<ApiResponse<CostCenter>> GetById(Guid id);
    Task<ApiResponse<IEnumerable<CostCenter>>> GetLevel(int level = 0);
    Task<ApiResponse<IEnumerable<CostCenter>>> GetChildren(Guid parentId, int level = 0);
}
