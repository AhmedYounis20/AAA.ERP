using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Dtos.Filters;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Application.Services.Queries;

namespace Accounting.Application.Services;

/// <summary>
/// Read-side contract for chart-of-account GET endpoints (hybrid CQRS — no writes).
/// </summary>
public interface IChartOfAccountQueries : ITreeSettingQueries<ChartOfAccount>
{
    Task<ApiResponse<PaginatedResult<ChartOfAccount>>> GetPaginated(
        ChartOfAccountFilterDto filter,
        CancellationToken cancellationToken = default);

    Task<string> GenerateNewChildCode(Guid? parentId);

    Task<ApiResponse<ChartOfAccountInputModel>> GetNextAccountDefaultData(Guid? parentId);
}
