using Accounting.Application.Commands.Taxes;
using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Entities;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Domain.Entities;

namespace Accounting.Infrastructure.Handlers.Taxes;

public class TaxCreateCommandHandler(
    ITaxRepository repository,
    IChartOfAccountRepository chartOfAccountRepository)
    : ICommandHandler<TaxCreateCommand, ApiResponse<Tax>>
{
    public async Task<ApiResponse<Tax>> Handle(TaxCreateCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var isDomain = command.NodeType.Equals(NodeType.Domain);
            var parentAccountId = command.ChartOfAccountId;

            var chartOfAccountParent = await chartOfAccountRepository.Get(parentAccountId);
            var newCode = await chartOfAccountRepository.GenerateNewCodeForChild(parentAccountId);

            var entity = command.Adapt<Tax>();
            entity.ChartOfAccountId = null;
            if (isDomain)
            {
                entity.ChartOfAccount = new ChartOfAccount
                {
                    Name = command.Name,
                    NameSecondLanguage = command.NameSecondLanguage,
                    ParentId = parentAccountId,
                    Code = newCode,
                    AccountNature = chartOfAccountParent?.AccountNature ?? AccountNature.Debit,
                    IsDepreciable = chartOfAccountParent?.IsDepreciable ?? false,
                    IsActiveAccount = chartOfAccountParent?.IsActiveAccount ?? false,
                    IsStopDealing = chartOfAccountParent?.IsStopDealing ?? true,
                    IsPostedAccount = chartOfAccountParent?.IsPostedAccount ?? false,
                    AccountGuidId = chartOfAccountParent?.AccountGuidId ?? Guid.NewGuid(),
                    IsCreatedFromSubLeadger = true,
                    SubLeadgerType = SubLeadgerType.Tax
                };
            }
            else
            {
                entity.ChartOfAccountId = null;
                entity.ChartOfAccount = null;
            }

            await repository.Add(entity);

            return new ApiResponse<Tax>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = entity,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Tax>
            {
                IsSuccess = false,
                Errors = [new MessageTemplate { MessageKey = ex.ToString() }]
            };
        }
    }
}
