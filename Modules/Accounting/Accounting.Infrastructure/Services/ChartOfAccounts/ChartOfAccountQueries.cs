using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Chart-of-account read queries. Transitional: delegates to <see cref="IChartOfAccountService"/>.
/// </summary>
public class ChartOfAccountQueries(IChartOfAccountService chartOfAccountService) : IChartOfAccountQueries
{
    public Task<ApiResponse<IEnumerable<ChartOfAccount>>> GetAll()
        => chartOfAccountService.ReadAll();

    public Task<ApiResponse<PaginatedResult<ChartOfAccount>>> GetPaginated(
        ChartOfAccountFilterDto filter,
        CancellationToken cancellationToken = default)
        => chartOfAccountService.ReadAllPaginated(filter, cancellationToken);

    public Task<ApiResponse<ChartOfAccount>> GetById(Guid id)
        => chartOfAccountService.ReadById(id);

    public Task<string> GenerateNewChildCode(Guid? parentId)
        => chartOfAccountService.GenerateNewCodeForChild(parentId);

    public Task<ApiResponse<ChartOfAccountInputModel>> GetNextAccountDefaultData(Guid? parentId)
        => chartOfAccountService.NextAccountDefaultData(parentId);
}
