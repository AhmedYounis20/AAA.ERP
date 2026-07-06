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
    protected override bool ShouldValidateBusinessRules => false;

    protected override Task<TEntity> MapUpdateEntityAsync(
        TUpdate command,
        TEntity existingEntity,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var newEntity = command.Adapt<TEntity>();

        if (existingEntity.ChartOfAccount is not null)
        {
            newEntity.ChartOfAccountId = existingEntity.ChartOfAccountId;
            newEntity.ChartOfAccount = existingEntity.ChartOfAccount;
            newEntity.ChartOfAccount.Name = command.Name;
            newEntity.ChartOfAccount.NameSecondLanguage = command.NameSecondLanguage;
        }

        return Task.FromResult(newEntity);
    }

    protected override Task<ApiResponse<TEntity>?> OnUpdateExceptionAsync(
        TUpdate command,
        Exception exception,
        CancellationToken cancellationToken) =>
        Task.FromResult<ApiResponse<TEntity>?>(new ApiResponse<TEntity>
        {
            IsSuccess = false,
            Errors = [new MessageTemplate { MessageKey = exception.ToString() }]
        });
}
