using Accounting.Application.Commands.FixedAssets;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Domain.Entities;

namespace Accounting.Infrastructure.Workflows;

/// <summary>
/// Fixed-asset write orchestration (asset-type COA rules, depreciation accounts, transactional UoW).
/// </summary>
public class FixedAssetWorkflowService(
    IUnitOfWork unitOfWork,
    IFixedAssetRepository repository,
    IChartOfAccountRepository chartOfAccountRepository,
    ISubLedgerCoaParentResolver subLedgerCoaParentResolver,
    ISubLedgerCoaFactory subLedgerCoaFactory) : IFixedAssetWorkflowService
{
    public async Task<ApiResponse<FixedAsset>> Create(FixedAssetCreateCommand command)
    {
        var duplicateValidation = await ValidateDuplicateNames(command.Name, command.NameSecondLanguage);
        if (!duplicateValidation.isValid)
        {
            return new ApiResponse<FixedAsset>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = duplicateValidation.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
            };
        }

        await unitOfWork.BeginTransactionAsync();
        try
        {
            var isDomain = command.NodeType.Equals(NodeType.Domain);
            var assetType = command.FixedAssetType ?? FixedAssetType.Lands;
            var entity = command.Adapt<FixedAsset>();

            if (isDomain)
            {
                if (!command.IsDepreciable)
                {
                    entity.AssetLifeSpanByYears = 0;
                    entity.DepreciationRate = 0;
                }

                var assetParent = await subLedgerCoaParentResolver.ResolveAsync(
                    SubLeadgerType.FixedAsset,
                    assetType,
                    SubLedgerAccountRole.Asset);

                entity.ChartOfAccount = await subLedgerCoaFactory.CreateChildAccountAsync(
                    command.Name,
                    command.NameSecondLanguage,
                    assetParent,
                    SubLeadgerType.FixedAsset);

                if (assetType != FixedAssetType.Lands)
                {
                    var accumulatedParent = await subLedgerCoaParentResolver.ResolveAsync(
                        SubLeadgerType.FixedAsset,
                        assetType,
                        SubLedgerAccountRole.AccumulatedDepreciation);
                    var expenseParent = await subLedgerCoaParentResolver.ResolveAsync(
                        SubLeadgerType.FixedAsset,
                        assetType,
                        SubLedgerAccountRole.DepreciationExpense);

                    var accumulatedNamePrefix = await GetDepreciationNamePrefixAsync(accumulatedParent);
                    var expenseNamePrefix = await GetDepreciationNamePrefixAsync(expenseParent);

                    entity.AccumlatedAccount = await subLedgerCoaFactory.CreatePrefixedChildAccountAsync(
                        accumulatedNamePrefix,
                        command.Name,
                        command.NameSecondLanguage,
                        accumulatedParent);

                    entity.ExpensesAccount = await subLedgerCoaFactory.CreatePrefixedChildAccountAsync(
                        expenseNamePrefix,
                        command.Name,
                        command.NameSecondLanguage,
                        expenseParent);
                }
            }
            else
            {
                entity.ChartOfAccountId = null;
                entity.ChartOfAccount = null;
            }

            await repository.Add(entity);
            await unitOfWork.CommitAsync();

            return new ApiResponse<FixedAsset>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return new ApiResponse<FixedAsset>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<FixedAsset>> Update(FixedAssetUpdateCommand command)
    {
        var duplicateValidation = await ValidateDuplicateNames(command.Name, command.NameSecondLanguage, command.Id);
        if (!duplicateValidation.isValid)
        {
            return new ApiResponse<FixedAsset>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = duplicateValidation.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
            };
        }

        await unitOfWork.BeginTransactionAsync();
        try
        {
            var entity = await repository.GetAsNoTracking(command.Id);
            if (entity is null)
            {
                await unitOfWork.RollbackAsync();
                return new ApiResponse<FixedAsset>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = [new MessageTemplate { MessageKey = ErrorMessages.RecordNotFound }]
                };
            }

            if (entity.NodeType != command.NodeType)
            {
                await unitOfWork.RollbackAsync();
                return new ApiResponse<FixedAsset>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = [new MessageTemplate { MessageKey = ErrorMessages.CannotChangeNodeType }]
                };
            }

            entity.Name = command.Name;
            entity.NameSecondLanguage = command.NameSecondLanguage;
            entity.Notes = command.Notes;
            if (command.NodeType == NodeType.Domain)
            {
                entity.ManufactureCompany = command.ManufactureCompany;
                entity.Model = command.Model;
                entity.Serial = command.Serial;
                entity.Version = command.Version;
                entity.DepreciationRate = command.DepreciationRate;
                entity.IsDepreciable = command.IsDepreciable;
                entity.AssetLifeSpanByYears = command.AssetLifeSpanByYears;
                if (!command.IsDepreciable)
                {
                    entity.AssetLifeSpanByYears = 0;
                    entity.DepreciationRate = 0;
                }
            }

            if (entity.ChartOfAccountId.HasValue)
            {
                var account = await chartOfAccountRepository.Get(entity.ChartOfAccountId.Value);
                account.Name = command.Name;
                account.NameSecondLanguage = command.NameSecondLanguage;
                await chartOfAccountRepository.Update(account);
            }

            if (entity.AccumlatedAccountId.HasValue && entity.FixedAssetType.HasValue)
            {
                var accumulatedParent = await subLedgerCoaParentResolver.ResolveAsync(
                    SubLeadgerType.FixedAsset,
                    entity.FixedAssetType,
                    SubLedgerAccountRole.AccumulatedDepreciation);
                var accumulatedNamePrefix = await GetDepreciationNamePrefixAsync(accumulatedParent);

                var acculatedAccount = await chartOfAccountRepository.Get(entity.AccumlatedAccountId.Value);
                acculatedAccount.Name = $"{accumulatedNamePrefix.Name} {command.Name}";
                acculatedAccount.NameSecondLanguage =
                    $"{accumulatedNamePrefix.NameSecondLanguage} {command.NameSecondLanguage}";
                await chartOfAccountRepository.Update(acculatedAccount);
            }

            if (entity.ExpensesAccountId.HasValue && entity.FixedAssetType.HasValue)
            {
                var expenseParent = await subLedgerCoaParentResolver.ResolveAsync(
                    SubLeadgerType.FixedAsset,
                    entity.FixedAssetType,
                    SubLedgerAccountRole.DepreciationExpense);
                var expenseNamePrefix = await GetDepreciationNamePrefixAsync(expenseParent);

                var expenseAccount = await chartOfAccountRepository.Get(entity.ExpensesAccountId.Value);
                expenseAccount.Name = $"{expenseNamePrefix.Name} {command.Name}";
                expenseAccount.NameSecondLanguage =
                    $"{expenseNamePrefix.NameSecondLanguage} {command.NameSecondLanguage}";
                await chartOfAccountRepository.Update(expenseAccount);
            }

            await repository.Update(entity);

            await unitOfWork.CommitAsync();
            return new ApiResponse<FixedAsset>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            return new ApiResponse<FixedAsset>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<FixedAsset>> Delete(Guid id)
    {
        var validationResult = await ValidateDelete(id);
        if (!validationResult.isValid)
        {
            return new ApiResponse<FixedAsset>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
            };
        }

        await unitOfWork.BeginTransactionAsync();
        try
        {
            if (validationResult.entity is not null)
            {
                var entity = validationResult.entity;
                entity.ChartOfAccount = null;
                entity.ExpensesAccount = null;
                entity.AccumlatedAccount = null;
                await repository.Delete(entity);

                if (entity.ChartOfAccountId.HasValue)
                    await chartOfAccountRepository.Delete(entity.ChartOfAccountId.Value);
                if (entity.AccumlatedAccountId.HasValue)
                    await chartOfAccountRepository.Delete(entity.AccumlatedAccountId.Value);
                if (entity.ExpensesAccountId.HasValue)
                    await chartOfAccountRepository.Delete(entity.ExpensesAccountId.Value);
            }

            await unitOfWork.CommitAsync();

            return new ApiResponse<FixedAsset>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = validationResult.entity
            };
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackAsync();
            Log.Error(ex.Message);
            return new ApiResponse<FixedAsset>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message.Trim() }]
            };
        }
    }

    private async Task<ChartOfAccount> GetDepreciationNamePrefixAsync(ChartOfAccount depreciationBaseAccount)
    {
        if (!depreciationBaseAccount.ParentId.HasValue)
            return depreciationBaseAccount;

        var header = await chartOfAccountRepository.Get(depreciationBaseAccount.ParentId.Value);
        return header ?? depreciationBaseAccount;
    }

    private async Task<(bool isValid, List<string> errors, FixedAsset? entity)> ValidateDelete(Guid id)
    {
        if (await repository.HasChildren(id))
            return (false, ["CannotDeleteParent"], null);

        var entity = await repository.Get(id);
        if (entity is null)
            return (false, [ErrorMessages.RecordNotFound], null);

        return (true, [], entity);
    }

    private async Task<(bool isValid, List<string>? errors)> ValidateDuplicateNames(
        string? name,
        string? nameSecondLanguage,
        Guid? excludeId = null)
    {
        var existedEntity = await repository.GetByNames(name, nameSecondLanguage);
        if (existedEntity is null || (excludeId.HasValue && existedEntity.Id == excludeId.Value))
            return (true, null);

        var errors = new List<string>();
        if (existedEntity.Name?.Trim().ToUpper() == name?.Trim().ToUpper())
            errors.Add(ErrorMessages.WithSameNameIsExisted);
        if (existedEntity.NameSecondLanguage?.Trim().ToUpper() == nameSecondLanguage?.Trim().ToUpper())
            errors.Add(ErrorMessages.WithSameNameSecondLanguageIsExisted);

        return errors.Count == 0 ? (true, null) : (false, errors);
    }
}
