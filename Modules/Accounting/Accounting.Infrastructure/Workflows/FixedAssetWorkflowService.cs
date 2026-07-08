using Accounting.Application.Commands.FixedAssets;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using Accounting.Domain.Models.Entities.SubLeadgers;
using Accounting.Domain.Utility;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Domain.Entities;

namespace Accounting.Infrastructure.Workflows;

/// <summary>
/// Fixed-asset write orchestration (asset-type COA rules, depreciation accounts, transactional UoW).
/// </summary>
public class FixedAssetWorkflowService(
    IUnitOfWork unitOfWork,
    IFixedAssetRepository repository,
    IChartOfAccountRepository chartOfAccountRepository) : IFixedAssetWorkflowService
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

        try
        {
            var isDomain = command.NodeType.Equals(NodeType.Domain);
            var fixedAssetId = Guid.Parse(SD.FixedAssetsIds[command.FixedAssetType ?? FixedAssetType.Lands]);
            var chartOfAccountParent = await chartOfAccountRepository.Get(fixedAssetId);
            var newCode = await chartOfAccountRepository.GenerateNewCodeForChild(fixedAssetId);
            var entity = command.Adapt<FixedAsset>();

            if (isDomain)
            {
                if (!command.IsDepreciable)
                {
                    entity.AssetLifeSpanByYears = 0;
                    entity.DepreciationRate = 0;
                }

                var fixedAssetsDepreciationDetails =
                    SD.FixedAssetsDepreciationDetails[command.FixedAssetType ?? FixedAssetType.Lands];
                var accumlatedDepriciation = command.FixedAssetType == FixedAssetType.Lands
                    ? null
                    : await chartOfAccountRepository.Get(Guid.Parse(fixedAssetsDepreciationDetails[0]));
                var expensesDepriciation = command.FixedAssetType == FixedAssetType.Lands
                    ? null
                    : await chartOfAccountRepository.Get(Guid.Parse(fixedAssetsDepreciationDetails[1]));

                entity.ChartOfAccount = new ChartOfAccount
                {
                    Name = command.Name,
                    NameSecondLanguage = command.NameSecondLanguage,
                    ParentId = fixedAssetId,
                    Code = newCode,
                    AccountNature = chartOfAccountParent?.AccountNature ?? AccountNature.Debit,
                    IsDepreciable = chartOfAccountParent?.IsDepreciable ?? false,
                    IsActiveAccount = chartOfAccountParent?.IsActiveAccount ?? false,
                    IsStopDealing = chartOfAccountParent?.IsStopDealing ?? true,
                    IsPostedAccount = chartOfAccountParent?.IsPostedAccount ?? false,
                    AccountGuidId = chartOfAccountParent?.AccountGuidId ?? Guid.NewGuid(),
                    IsCreatedFromSubLeadger = true,
                    SubLeadgerType = SubLeadgerType.FixedAsset
                };

                if (accumlatedDepriciation != null)
                {
                    var accumlatedDepreciationAccount =
                        await chartOfAccountRepository.Get(Guid.Parse(SD.AccumlatedDepreciationId));
                    var newAccumlatedCode =
                        await chartOfAccountRepository.GenerateNewCodeForChild(accumlatedDepriciation.Id);
                    entity.AccumlatedAccount = new ChartOfAccount
                    {
                        Name = $"{accumlatedDepreciationAccount.Name} {command.Name}",
                        NameSecondLanguage =
                            $"{accumlatedDepreciationAccount.NameSecondLanguage} {command.NameSecondLanguage}",
                        ParentId = accumlatedDepriciation.Id,
                        Code = newAccumlatedCode,
                        AccountNature = accumlatedDepriciation?.AccountNature ?? AccountNature.Debit,
                        IsDepreciable = accumlatedDepriciation?.IsDepreciable ?? false,
                        IsActiveAccount = accumlatedDepriciation?.IsActiveAccount ?? false,
                        IsStopDealing = accumlatedDepriciation?.IsStopDealing ?? true,
                        IsPostedAccount = accumlatedDepriciation?.IsPostedAccount ?? false,
                        AccountGuidId = accumlatedDepriciation?.AccountGuidId ?? Guid.NewGuid(),
                        IsCreatedFromSubLeadger = true
                    };
                }

                if (expensesDepriciation != null)
                {
                    var expensesDepreciationAccount =
                        await chartOfAccountRepository.Get(Guid.Parse(SD.ExpensesDepreciationId));
                    var newExpenseCode =
                        await chartOfAccountRepository.GenerateNewCodeForChild(expensesDepriciation.Id);
                    entity.ExpensesAccount = new ChartOfAccount
                    {
                        Name = $"{expensesDepreciationAccount.Name} {command.Name}",
                        NameSecondLanguage =
                            $"{expensesDepreciationAccount.NameSecondLanguage} {command.NameSecondLanguage}",
                        ParentId = expensesDepriciation.Id,
                        Code = newExpenseCode,
                        AccountNature = expensesDepriciation?.AccountNature ?? AccountNature.Debit,
                        IsDepreciable = expensesDepriciation?.IsDepreciable ?? false,
                        IsActiveAccount = expensesDepriciation?.IsActiveAccount ?? false,
                        IsStopDealing = expensesDepriciation?.IsStopDealing ?? true,
                        IsPostedAccount = expensesDepriciation?.IsPostedAccount ?? false,
                        AccountGuidId = expensesDepriciation?.AccountGuidId ?? Guid.NewGuid(),
                        IsCreatedFromSubLeadger = true
                    };
                }
            }
            else
            {
                entity.ChartOfAccountId = null;
                entity.ChartOfAccount = null;
            }

            await repository.Add(entity);

            return new ApiResponse<FixedAsset>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
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

            if (entity.AccumlatedAccountId.HasValue)
            {
                var accumlatedDepreciationAccount =
                    await chartOfAccountRepository.Get(Guid.Parse(SD.AccumlatedDepreciationId));

                var acculatedAccount =
                    await chartOfAccountRepository.Get(entity.AccumlatedAccountId.Value);
                acculatedAccount.Name = $"{accumlatedDepreciationAccount.Name} {command.Name}";
                acculatedAccount.NameSecondLanguage =
                    $"{accumlatedDepreciationAccount.NameSecondLanguage} {command.NameSecondLanguage}";
                await chartOfAccountRepository.Update(acculatedAccount);
            }

            if (entity.ExpensesAccountId.HasValue)
            {
                var expensesDepreciationAccount =
                    await chartOfAccountRepository.Get(Guid.Parse(SD.ExpensesDepreciationId));

                var expenseAccount =
                    await chartOfAccountRepository.Get(entity.ExpensesAccountId.Value);
                expenseAccount.Name = $"{expensesDepreciationAccount.Name} {command.Name}";
                expenseAccount.NameSecondLanguage =
                    $"{expensesDepreciationAccount.NameSecondLanguage} {command.NameSecondLanguage}";
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
