using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Dtos.Filters;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for chart-of-account GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IChartOfAccountReadService
{
    Task<ApiResponse<IEnumerable<ChartOfAccount>>> GetAll();

    Task<ApiResponse<PaginatedResult<ChartOfAccount>>> GetPaginated(
        ChartOfAccountFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<ChartOfAccount>> GetById(Guid id);

    Task<string> GenerateNewChildCode(Guid? parentId);

    Task<ApiResponse<ChartOfAccountInputModel>> GetNextAccountDefaultData(Guid? parentId);
}
