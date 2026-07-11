using Accounting.Application.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.CQRS.Handlers.TreeSetting;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using Mapster;

namespace Accounting.Infrastructure.Handlers.SubLedger;

/// <summary>
/// Generic update handler for sub-ledger entities. Mirrors <c>SubLeadgerService.Update</c>.
/// </summary>
public abstract class BaseSubLedgerUpdateHandler<TEntity, TUpdate>(IBaseSubLeadgerRepository<TEntity> repository)
    : BaseTreeSettingUpdateHandler<TEntity, TUpdate>(repository)
    where TEntity : SubLeadgerBaseEntity<TEntity>
    where TUpdate : BaseSubLeadgerUpdateCommand<TEntity>
{
    protected override Task<TEntity> MapUpdateEntityAsync(
        TUpdate command,
        TEntity existingEntity,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var existingChartOfAccount = existingEntity.ChartOfAccount;
        var existingChartOfAccountId = existingEntity.ChartOfAccountId;
        command.Adapt(existingEntity);

        if (existingChartOfAccount is not null)
        {
            existingEntity.ChartOfAccountId = existingChartOfAccountId;
            existingEntity.ChartOfAccount = existingChartOfAccount;
            existingEntity.ChartOfAccount.Name = command.Name;
            existingEntity.ChartOfAccount.NameSecondLanguage = command.NameSecondLanguage;
        }

        return Task.FromResult(existingEntity);
    }
}
