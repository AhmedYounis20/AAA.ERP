using Accounting.Application.Commands.Taxes;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Domain.Entities;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Tax default-data helper. CRUD writes are handled by CQRS handlers.
/// </summary>
public class TaxService(IChartOfAccountRepository chartOfAccountRepository) : ITaxService
{
    public async Task<ApiResponse<TaxCreateCommand>> GetNextSubLeadgers(Guid? parentId)
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
