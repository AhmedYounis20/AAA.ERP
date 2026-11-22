using Accounting.Application.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.CQRS.Handlers.TreeSetting;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Domain.Entities;
using Mapster;

namespace Accounting.Infrastructure.Handlers.SubLedger;

/// <summary>
/// Generic create handler for sub-ledger entities. Mirrors <c>SubLeadgerService.Create</c>.
/// </summary>
public abstract class BaseSubLedgerCreateHandler<TEntity, TCreate>(
    IBaseSubLeadgerRepository<TEntity> repository,
    ISubLedgerCoaParentResolver subLedgerCoaParentResolver,
    ISubLedgerCoaFactory subLedgerCoaFactory)
    : BaseTreeSettingCreateHandler<TEntity, TCreate>(repository)
    where TEntity : SubLeadgerBaseEntity<TEntity>
    where TCreate : BaseSubLeadgerCreateCommand<TEntity>
{
    protected abstract SubLeadgerType SubLedgerType { get; }

    protected virtual Task<ChartOfAccount> ResolveParentAccountAsync(
        TCreate command,
        CancellationToken cancellationToken)
        => subLedgerCoaParentResolver.ResolveAsync(SubLedgerType, cancellationToken: cancellationToken);

    protected override async Task<TEntity> MapCreateEntityAsync(
        TCreate command,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = command.Adapt<TEntity>();
        if (!command.NodeType.Equals(NodeType.Domain))
        {
            entity.ChartOfAccountId = null;
            entity.ChartOfAccount = null;
            return entity;
        }

        var parentAccount = await ResolveParentAccountAsync(command, cancellationToken);
        entity.ChartOfAccount = await subLedgerCoaFactory.CreateChildAccountAsync(
            command.Name,
            command.NameSecondLanguage,
            parentAccount,
            SubLedgerType,
            cancellationToken);

        return entity;
    }

    protected override Task<ApiResponse<TEntity>> MapCreateResponseAsync(
        TEntity entity,
        TCreate command,
        CancellationToken cancellationToken) =>
        Task.FromResult(new ApiResponse<TEntity>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.Created,
            Result = entity
        });
}
