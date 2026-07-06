using Accounting.Application.Commands.SubLeadgers.BaseSubLeadgersCommands;
using Accounting.Application.Repositories;
using Accounting.Application.Repositories.Account.SubLeadgers;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.CQRS.Handlers.TreeSetting;
using ERP.BuildingBlocks.Application.Repositories.BaseRepositories;
using ERP.BuildingBlocks.Application.Responses;
using ERP.BuildingBlocks.Domain.Entities;
using Mapster;

namespace Accounting.Infrastructure.Handlers.SubLedger;

/// <summary>
/// Generic create handler for sub-ledger entities. Mirrors <c>SubLeadgerService.Create</c>.
/// </summary>
public abstract class BaseSubLedgerCreateHandler<TEntity, TCreate>(
    IBaseSubLeadgerRepository<TEntity> repository,
    IChartOfAccountRepository chartOfAccountRepository,
    Guid chartAccountParentId,
    SubLeadgerType? subLeadgerType)
    : BaseTreeSettingCreateHandler<TEntity, TCreate>(repository)
    where TEntity : SubLeadgerBaseEntity<TEntity>
    where TCreate : BaseSubLeadgerCreateCommand<TEntity>
{
    protected IChartOfAccountRepository ChartOfAccountRepository => chartOfAccountRepository;

    protected Guid ChartAccountParentId => chartAccountParentId;

    protected SubLeadgerType? SubLeadgerTypeValue => subLeadgerType;

    protected override bool ShouldValidateBusinessRules => false;

    protected virtual Guid ResolveChartOfAccountParentId(TCreate command) => chartAccountParentId;

    protected override async Task<TEntity> MapCreateEntityAsync(
        TCreate command,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var isDomain = command.NodeType.Equals(NodeType.Domain);
        var parentAccountId = ResolveChartOfAccountParentId(command);
        var chartOfAccountParent = await ChartOfAccountRepository.Get(parentAccountId);
        var newCode = await ChartOfAccountRepository.GenerateNewCodeForChild(parentAccountId);
        var entity = command.Adapt<TEntity>();

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
                SubLeadgerType = subLeadgerType
            };
        }
        else
        {
            entity.ChartOfAccountId = null;
            entity.ChartOfAccount = null;
        }

        return entity;
    }

    protected override Task<ApiResponse<TEntity>> MapCreateResponseAsync(
        TEntity entity,
        TCreate command,
        CancellationToken cancellationToken) =>
        Task.FromResult(new ApiResponse<TEntity>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = entity
        });

    protected override Task<ApiResponse<TEntity>?> OnCreateExceptionAsync(
        TCreate command,
        Exception exception,
        CancellationToken cancellationToken) =>
        Task.FromResult<ApiResponse<TEntity>?>(new ApiResponse<TEntity>
        {
            IsSuccess = false,
            Errors = [new MessageTemplate { MessageKey = exception.ToString() }]
        });
}
