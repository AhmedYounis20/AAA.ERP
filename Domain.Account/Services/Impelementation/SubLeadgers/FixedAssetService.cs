using Domain.Account.Commands.SubLeadgers.FixedAssets;
using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Models.Entities.SubLeadgers;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Repositories.Interfaces.SubLeadgers;
using Domain.Account.Services.Impelementation.SubLeadgers.SubLeadgerBaseService;
using Domain.Account.Services.Interfaces.SubLeadgers;
using Domain.Account.Utility;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Shared.BaseEntities;
using Shared.Responses;

namespace AAA.ERP.Services.Impelementation.SubLeadgers;

public class FixedAssetService : SubLeadgerService<FixedAsset, FixedAssetCreateCommand, FixedAssetUpdateCommand>,
    IFixedAssetService
{
    private IUnitOfWork _unitOfWork;
    private IFixedAssetRepository _repository;
    private IHttpContextAccessor _accessor;

    public FixedAssetService(IUnitOfWork unitOfWork, IFixedAssetRepository repository, IHttpContextAccessor accessor)
        : base(unitOfWork, repository, accessor, SD.BankChartAccountId,SubLeadgerType.FixedAsset)
    {
        _unitOfWork = unitOfWork;
        _accessor = accessor;
        _repository = repository;
    }

    public override async Task<ApiResponse<FixedAsset>> Create(FixedAssetCreateCommand command, bool isValidate = true)
    {
        try
        {
            bool isDomain = command.NodeType.Equals(NodeType.Domain);
            Guid fixedAssetId = Guid.Parse(SD.FixedAssetsIds[command.FixedAssetType ?? FixedAssetType.Lands]);
            ChartOfAccount? chartOfAccountParent =
                await _unitOfWork.ChartOfAccountRepository.Get(fixedAssetId);
            string newCode = await _unitOfWork
                .ChartOfAccountRepository
                .GenerateNewCodeForChild(fixedAssetId);
            FixedAsset entity = command.Adapt<FixedAsset>();
            if (isDomain)
            {
                if (!command.IsDepreciable)
                {
                    entity.AssetLifeSpanByYears = 0;
                    entity.DepreciationRate = 0;
                }
                
                var fixedAssetsDepreciationDetails =
                    SD.FixedAssetsDepreciationDetails[command.FixedAssetType ?? FixedAssetType.Lands];
                ChartOfAccount? accumlatedDepriciation =
                    command.FixedAssetType == FixedAssetType.Lands ? null : await _unitOfWork.ChartOfAccountRepository.Get(Guid.Parse(fixedAssetsDepreciationDetails[0]));
                ChartOfAccount expensesDepriciation =
                    command.FixedAssetType == FixedAssetType.Lands ? null : await _unitOfWork.ChartOfAccountRepository.Get(Guid.Parse(fixedAssetsDepreciationDetails[1]));
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
                    var accumlatedDepreciationAccount = await
                        _unitOfWork.ChartOfAccountRepository.Get(Guid.Parse(SD.AccumlatedDepreciationId));
                    string newAccumlatedCode = await _unitOfWork
                        .ChartOfAccountRepository
                        .GenerateNewCodeForChild(accumlatedDepriciation.Id);
                    entity.AccumlatedAccount =
                        new ChartOfAccount()
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
                    var expensesDepreciationAccount = await
                        _unitOfWork.ChartOfAccountRepository.Get(Guid.Parse(SD.ExpensesDepreciationId));
                    string newAccumlatedCode = await _unitOfWork
                        .ChartOfAccountRepository
                        .GenerateNewCodeForChild(expensesDepriciation.Id);
                    entity.ExpensesAccount =
                        new ChartOfAccount()
                        {
                            Name = $"{expensesDepreciationAccount.Name} {command.Name}",
                            NameSecondLanguage =
                                $"{expensesDepreciationAccount.NameSecondLanguage} {command.NameSecondLanguage}",
                            ParentId = expensesDepriciation.Id,
                            Code = newAccumlatedCode,
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

            await _repository.Add(entity);

            return new ApiResponse<FixedAsset>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<FixedAsset>
            {
                IsSuccess = false,
                ErrorMessages = [ex.ToString()]
            };
        }
    }

    public async Task<ApiResponse<FixedAssetCreateCommand>> GetNextFixedAsset(Guid? parentId,
        FixedAssetType fixedAssetType)
    {
        var chartOfAccountId = Guid.Parse(SD.FixedAssetsIds[fixedAssetType]);
        var assetCodes = SD.FixedAssetsDepreciationDetails[fixedAssetType];
        string? code =
            await _unitOfWork.ChartOfAccountRepository.GenerateNewCodeForChild(chartOfAccountId);
        string accumelatedCode = fixedAssetType == FixedAssetType.Lands ? "": await _unitOfWork.ChartOfAccountRepository.GenerateNewCodeForChild(Guid.Parse(assetCodes[0]));
        string expenseCode = fixedAssetType == FixedAssetType.Lands ? "": await _unitOfWork.ChartOfAccountRepository.GenerateNewCodeForChild(Guid.Parse(assetCodes[1]));
        FixedAssetCreateCommand inputModel = Activator.CreateInstance<FixedAssetCreateCommand>();

        inputModel.Code = code;
        inputModel.ExpensesCode = expenseCode;
        inputModel.AccumelatedCode = accumelatedCode;
        inputModel.NodeType = NodeType.Domain;
        inputModel.ParentId = parentId;
        inputModel.FixedAssetType = fixedAssetType;

        return new ApiResponse<FixedAssetCreateCommand>
        {
            IsSuccess = true,
            StatusCode = HttpStatusCode.OK,
            Result = inputModel
        };
    }

    public override async Task<ApiResponse<FixedAsset>> Update(FixedAssetUpdateCommand command, bool isValidate = true)
    {
        await _unitOfWork.BeginTransactionAsync();
        try
        {
            bool isDomain = command.NodeType.Equals(NodeType.Domain);
            FixedAsset? entity = await _repository.GetAsNoTracking(command.Id);
            if (entity != null)
            {
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
                    var account = await _unitOfWork.ChartOfAccountRepository.Get(entity.ChartOfAccountId.Value);
                    account.Name = command.Name;
                    account.NameSecondLanguage = command.NameSecondLanguage;
                    await _unitOfWork.ChartOfAccountRepository.Update(account);
                }

                if (entity.AccumlatedAccountId.HasValue)
                {
                    var accumlatedDepreciationAccount = await
                        _unitOfWork.ChartOfAccountRepository.Get(Guid.Parse(SD.AccumlatedDepreciationId));

                    var acculatedAccount =
                        await _unitOfWork.ChartOfAccountRepository.Get(entity.AccumlatedAccountId.Value);
                    acculatedAccount.Name = $"{accumlatedDepreciationAccount.Name} {command.Name}";
                    acculatedAccount.NameSecondLanguage =
                        $"{accumlatedDepreciationAccount.NameSecondLanguage} {command.NameSecondLanguage}";
                    await _unitOfWork.ChartOfAccountRepository.Update(acculatedAccount);
                }

                if (entity.ExpensesAccountId.HasValue)
                {
                    var expensesDepreciationAccount = await
                        _unitOfWork.ChartOfAccountRepository.Get(Guid.Parse(SD.ExpensesDepreciationId));

                    var acculatedAccount =
                        await _unitOfWork.ChartOfAccountRepository.Get(entity.ExpensesAccountId
                            .Value);
                    acculatedAccount.Name = $"{expensesDepreciationAccount.Name} {command.Name}";
                    acculatedAccount.NameSecondLanguage =
                        $"{expensesDepreciationAccount.NameSecondLanguage} {command.NameSecondLanguage}";
                    await _unitOfWork.ChartOfAccountRepository.Update(acculatedAccount);
                }

                await _repository.Update(entity);
            }

            await _unitOfWork.CommitAsync();
            return new ApiResponse<FixedAsset>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity,
            };
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return new ApiResponse<FixedAsset>
            {
                IsSuccess = false,
                ErrorMessages = [ex.ToString()]
            };
        }
    }

    public override async Task<ApiResponse<FixedAsset>> Delete(Guid id, bool isValidate = true)
    {
        var validationResult = await ValidateDelete(id);
        if (validationResult.isValid)
        {
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (validationResult.entity is not null)
                {
                    var entity = validationResult.entity;
                    entity.ChartOfAccount = null;
                    entity.ExpensesAccount = null;
                    entity.AccumlatedAccount = null;
                    await _repository.Delete(entity);

                    if (entity.ChartOfAccountId.HasValue)
                        await _unitOfWork.ChartOfAccountRepository.Delete(entity.ChartOfAccountId.Value);
                    if (entity.AccumlatedAccountId.HasValue)
                        await _unitOfWork.ChartOfAccountRepository.Delete(entity.AccumlatedAccountId.Value);
                    if (entity.ExpensesAccountId.HasValue)
                        await _unitOfWork.ChartOfAccountRepository.Delete(entity.ExpensesAccountId.Value);
                }

                await _unitOfWork.CommitAsync();

                return new ApiResponse<FixedAsset>
                {
                    IsSuccess = true,
                    StatusCode = HttpStatusCode.OK,
                    Result = validationResult.entity
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                Log.Error(ex.Message.ToString());
                return new ApiResponse<FixedAsset>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    ErrorMessages = [ex.Message.Trim().ToString()]
                };
            }
        }

        return new ApiResponse<FixedAsset>
        {
            IsSuccess = false,
            StatusCode = HttpStatusCode.BadRequest,
            ErrorMessages = validationResult.errors
        };
    }
}