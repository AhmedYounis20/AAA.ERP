using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Chart-of-account read queries.
/// </summary>
public class ChartOfAccountQueries(IChartOfAccountRepository repository)
    : BaseTreeSettingRepositoryQueries<ChartOfAccount>(repository),
      IChartOfAccountQueries
{
    private readonly BaseEntityRepositoryQueries<ChartOfAccount> _entityQueries = new(repository);

    public Task<ApiResponse<PaginatedResult<ChartOfAccount>>> GetPaginated(
        ChartOfAccountFilterDto filter,
        CancellationToken cancellationToken = default)
        => _entityQueries.GetPaginated(filter, cancellationToken);

    public Task<string> GenerateNewChildCode(Guid? parentId)
        => repository.GenerateNewCodeForChild(parentId);

    public async Task<ApiResponse<ChartOfAccountInputModel>> GetNextAccountDefaultData(Guid? parentId)
    {
        try
        {
            ChartOfAccount? parent = parentId is not null ? await repository.Get(parentId ?? Guid.Empty) : null;
            var inputModel = new ChartOfAccountInputModel
            {
                parentId = parentId,
                AccountNature = parent?.AccountNature ?? AccountNature.Debit,
                AccountGuidId = parent?.AccountGuidId ?? Guid.Empty,
                Code = await repository.GenerateNewCodeForChild(parentId),
                IsPostedAccount = parent?.IsPostedAccount ?? false,
                IsStopDealing = parent?.IsStopDealing ?? false,
                IsDepreciable = false,
                IsActiveAccount = true,
            };

            return new ApiResponse<ChartOfAccountInputModel>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = inputModel
            };
        }
        catch (Exception)
        {
            return new ApiResponse<ChartOfAccountInputModel>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                IsSuccess = false,
            };
        }
    }
}
