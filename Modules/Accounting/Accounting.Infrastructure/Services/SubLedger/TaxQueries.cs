using Accounting.Application.Commands.Taxes;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Domain.Entities;
using ERP.BuildingBlocks.Persistence.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Tax read queries.
/// </summary>
public class TaxQueries(ITaxRepository repository, IChartOfAccountRepository chartOfAccountRepository)
    : BaseTreeSettingRepositoryQueries<Tax>(repository),
      ITaxQueries
{
    public async Task<ApiResponse<TaxCreateCommand>> GetNextSubLedgers(Guid? parentId)
    {
        if (!parentId.HasValue)
        {
            return new ApiResponse<TaxCreateCommand>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ErrorMessages.ParentIdIsRequired }]
            };
        }

        var code = await chartOfAccountRepository.GenerateNewCodeForChild(parentId);

        return new ApiResponse<TaxCreateCommand>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = new TaxCreateCommand
            {
                Code = code,
                NodeType = NodeType.Domain,
                ParentId = parentId
            }
        };
    }
}
