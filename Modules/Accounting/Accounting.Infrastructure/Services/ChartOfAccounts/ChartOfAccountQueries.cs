using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Chart-of-account read queries.
/// </summary>
public class ChartOfAccountQueries(
    IChartOfAccountRepository repository,
    IChartOfAccountService chartOfAccountService) : IChartOfAccountQueries
{
    private readonly BaseEntityRepositoryQueries<ChartOfAccount> _entityQueries = new(repository);

    public Task<ApiResponse<IEnumerable<ChartOfAccount>>> GetAll()
        => _entityQueries.GetAll();

    public Task<ApiResponse<PaginatedResult<ChartOfAccount>>> GetPaginated(
        ChartOfAccountFilterDto filter,
        CancellationToken cancellationToken = default)
        => _entityQueries.GetPaginated(filter, cancellationToken);

    public Task<ApiResponse<ChartOfAccount>> GetById(Guid id)
        => _entityQueries.GetById(id);

    public Task<string> GenerateNewChildCode(Guid? parentId)
        => chartOfAccountService.GenerateNewCodeForChild(parentId);

    public Task<ApiResponse<ChartOfAccountInputModel>> GetNextAccountDefaultData(Guid? parentId)
        => chartOfAccountService.NextAccountDefaultData(parentId);
}
