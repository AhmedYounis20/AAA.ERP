using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Chart-of-account read queries.
/// </summary>
public class ChartOfAccountQueries(IChartOfAccountRepository repository)
    : BaseTreeSettingRepositoryQueries<ChartOfAccount>(repository),
      IChartOfAccountQueries
{
    public async Task<ApiResponse<PaginatedResult<ChartOfAccount>>> GetPaginated(
        ChartOfAccountFilterDto filter,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = repository.GetQuery().AsNoTracking();

            if (filter.CreatedFrom.HasValue)
                query = query.Where(e => e.CreatedAt >= filter.CreatedFrom.Value);

            if (filter.CreatedTo.HasValue)
                query = query.Where(e => e.CreatedAt <= filter.CreatedTo.Value);

            if (filter.ParentId.HasValue)
            {
                query = filter.ParentId.Value == Guid.Empty
                    ? query.Where(e => e.ParentId == null)
                    : query.Where(e => e.ParentId == filter.ParentId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim();
                query = query.Where(e =>
                    (e.Name != null && e.Name.Contains(term)) ||
                    (e.NameSecondLanguage != null && e.NameSecondLanguage.Contains(term)) ||
                    (e.Code != null && e.Code.Contains(term)));
            }

            if (filter.AccountNature.HasValue)
            {
                var accountNature = (AccountNature)filter.AccountNature.Value;
                query = query.Where(e => e.AccountNature == accountNature);
            }

            if (filter.AccountGuidId.HasValue)
                query = query.Where(e => e.AccountGuidId == filter.AccountGuidId.Value);

            if (filter.IsActiveAccount.HasValue)
                query = query.Where(e => e.IsActiveAccount == filter.IsActiveAccount.Value);

            if (filter.IsPostedAccount.HasValue)
                query = query.Where(e => e.IsPostedAccount == filter.IsPostedAccount.Value);

            if (filter.IsStopDealing.HasValue)
                query = query.Where(e => e.IsStopDealing == filter.IsStopDealing.Value);

            if (filter.SubLeadgerType.HasValue)
            {
                var subLeadgerType = (SubLeadgerType)filter.SubLeadgerType.Value;
                query = query.Where(e => e.SubLeadgerType == subLeadgerType);
            }

            query = string.IsNullOrEmpty(filter.SortBy)
                ? query.OrderByDescending(e => e.CreatedAt)
                : filter.SortBy.ToLower() switch
                {
                    "createdat" => filter.SortDescending
                        ? query.OrderByDescending(e => e.CreatedAt)
                        : query.OrderBy(e => e.CreatedAt),
                    "modifiedat" => filter.SortDescending
                        ? query.OrderByDescending(e => e.ModifiedAt)
                        : query.OrderBy(e => e.ModifiedAt),
                    "code" => filter.SortDescending
                        ? query.OrderByDescending(e => e.Code)
                        : query.OrderBy(e => e.Code),
                    "name" => filter.SortDescending
                        ? query.OrderByDescending(e => e.Name)
                        : query.OrderBy(e => e.Name),
                    _ => query.OrderByDescending(e => e.CreatedAt)
                };

            var result = await query.ToPaginatedResultAsync(filter.PageNumber, filter.PageSize, cancellationToken);

            return new ApiResponse<PaginatedResult<ChartOfAccount>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<PaginatedResult<ChartOfAccount>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<string>> GenerateNewChildCode(Guid? parentId)
    {
        try
        {
            var code = await repository.GenerateNewCodeForChild(parentId);
            return new ApiResponse<string>
            {
                StatusCode = HttpStatusCode.OK,
                IsSuccess = true,
                Result = code
            };
        }
        catch (InvalidOperationException ex) when (ex.Message == ErrorMessages.InvalidChartOfAccountCode)
        {
            return new ApiResponse<string>
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.InvalidChartOfAccountCode }]
            };
        }
        catch (Exception)
        {
            return new ApiResponse<string>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                IsSuccess = false
            };
        }
    }

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
        catch (InvalidOperationException ex) when (ex.Message == ErrorMessages.InvalidChartOfAccountCode)
        {
            return new ApiResponse<ChartOfAccountInputModel>
            {
                StatusCode = HttpStatusCode.BadRequest,
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.InvalidChartOfAccountCode }]
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
