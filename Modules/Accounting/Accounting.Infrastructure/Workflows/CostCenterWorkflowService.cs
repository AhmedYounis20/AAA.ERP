using Accounting.Application.Commands.CostCenters;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CostCenters;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Domain.Entities;

namespace Accounting.Infrastructure.Workflows;

/// <summary>
/// Cost center write orchestration (COA linking, domain node rules, transactional updates).
/// </summary>
public class CostCenterWorkflowService(
    ICostCenterRepository repository,
    IChartOfAccountRepository chartOfAccountRepository,
    IUnitOfWork unitOfWork) : ICostCenterWorkflowService
{
    public async Task<ApiResponse<CostCenter>> Create(CostCenterCreateCommand command)
    {
        try
        {
            var validationResult = await ValidateCreate(command);
            if (!validationResult.isValid)
            {
                return new ApiResponse<CostCenter>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }

            var costCenter = command.Adapt<CostCenter>();

            if (command.NodeType.Equals(NodeType.Domain) &&
                command.CostCenterType.Equals(CostCenterType.RelatedToAccount) &&
                command.ChartOfAccounts is { Count: > 0 })
            {
                costCenter.ChartOfAccounts = command.ChartOfAccounts
                    .Select(accountId => new CostCenterChartOfAccount
                    {
                        ChartOfAccountId = accountId,
                        CreatedAt = DateTime.Now,
                        ModifiedAt = DateTime.Now,
                    })
                    .ToList();
            }
            else
            {
                costCenter.ChartOfAccounts = null!;
            }

            costCenter.CreatedAt = DateTime.Now;
            costCenter.ModifiedAt = DateTime.Now;

            var result = await repository.Add(costCenter);
            return new ApiResponse<CostCenter>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = result
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            return new ApiResponse<CostCenter>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<CostCenter>> Update(CostCenterUpdateCommand command)
    {
        await unitOfWork.BeginTransactionAsync();
        try
        {
            var validationResult = await ValidateUpdate(command);
            if (!validationResult.isValid)
            {
                await unitOfWork.RollbackAsync();
                return new ApiResponse<CostCenter>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }

            var oldCostCenter = validationResult.entity!;
            oldCostCenter.Name = command.Name;
            oldCostCenter.NameSecondLanguage = command.NameSecondLanguage;
            if (oldCostCenter.NodeType.Equals(NodeType.Domain))
            {
                oldCostCenter.CostCenterType = command.CostCenterType;
                oldCostCenter.Percent = command.Percent;
                if (oldCostCenter.CostCenterType.Equals(CostCenterType.RelatedToAccount))
                {
                    var chartOfAccountsToRemove = oldCostCenter.ChartOfAccounts
                        .Where(e => command.ChartOfAccounts is null || !command.ChartOfAccounts.Contains(e.ChartOfAccountId))
                        .ToList();
                    oldCostCenter.ChartOfAccounts = oldCostCenter.ChartOfAccounts
                        .Where(e => !chartOfAccountsToRemove.Contains(e))
                        .ToList();
                    repository.RemoveChartOfAccounts(chartOfAccountsToRemove);

                    var chartOfAccountsToAdd = command.ChartOfAccounts is null
                        ? []
                        : command.ChartOfAccounts
                            .Except(oldCostCenter.ChartOfAccounts.Select(e => e.ChartOfAccountId))
                            .ToList();
                    chartOfAccountsToAdd.ForEach(e =>
                    {
                        oldCostCenter.ChartOfAccounts.Add(new CostCenterChartOfAccount
                        {
                            ChartOfAccountId = e,
                            CreatedAt = DateTime.Now,
                            ModifiedAt = DateTime.Now
                        });
                    });
                }
            }

            await repository.Update(oldCostCenter);
            await unitOfWork.CommitAsync();
            var updated = await repository.Get(command.Id);
            return new ApiResponse<CostCenter>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = updated
            };
        }
        catch (Exception ex)
        {
            Log.Error(ex.ToString());
            await unitOfWork.RollbackAsync();
            return new ApiResponse<CostCenter>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    public async Task<ApiResponse<CostCenter>> Delete(Guid id)
    {
        try
        {
            var validationResult = await ValidateDelete(id);
            if (!validationResult.isValid)
            {
                return new ApiResponse<CostCenter>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }

            var entity = validationResult.entity;
            if (entity is not null)
                await repository.Delete(entity);

            return new ApiResponse<CostCenter>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CostCenter>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    private async Task<(bool isValid, List<string> errors)> ValidateCreate(CostCenterCreateCommand command)
    {
        var (isValid, errors) = await ValidateDuplicateNamesForCreate(command);

        if (command.ChartOfAccounts is null)
            return (isValid, errors);

        var accounts = await chartOfAccountRepository.Get(e => command.ChartOfAccounts.Contains(e.Id));
        if (accounts.Count() != command.ChartOfAccounts.Count)
        {
            isValid = false;
            errors.Add(ErrorMessages.SomeChartOfAcountsNotExisted);
        }

        return (isValid, errors);
    }

    private async Task<(bool isValid, List<string> errors, CostCenter? entity)> ValidateUpdate(
        CostCenterUpdateCommand command)
    {
        var (isValid, errors, oldCostCenter) = await ValidateDuplicateNamesForUpdate(command);

        if (oldCostCenter is null)
        {
            isValid = false;
            errors.Add(ErrorMessages.RecordNotFound);
            return (false, errors, null);
        }

        if (command.ChartOfAccounts is not null)
        {
            var chartOfAccountsExists = await chartOfAccountRepository.Get(e => command.ChartOfAccounts.Contains(e.Id));
            if (chartOfAccountsExists.Count() != command.ChartOfAccounts.Count)
            {
                isValid = false;
                errors.Add(ErrorMessages.SomeChartOfAcountsNotExisted);
            }
        }

        if (oldCostCenter.NodeType != command.NodeType)
        {
            isValid = false;
            errors.Add(ErrorMessages.CannotChangeNodeType);
        }

        return (isValid, errors, oldCostCenter);
    }

    private async Task<(bool isValid, List<string> errors, CostCenter? entity)> ValidateDelete(Guid id)
    {
        if (await repository.HasChildren(id))
            return (false, [ErrorMessages.CannotDeleteParent], null);

        var entity = await repository.Get(id);
        if (entity is null)
            return (false, [ErrorMessages.RecordNotFound], null);

        return (true, [], entity);
    }

    private async Task<(bool isValid, List<string> errors)> ValidateDuplicateNamesForCreate(
        CostCenterCreateCommand command)
    {
        var existedEntity = await repository.GetByNames(command.Name, command.NameSecondLanguage);
        if (existedEntity is null)
            return (true, []);

        var errors = CollectDuplicateNameErrors(existedEntity, command.Name, command.NameSecondLanguage);
        return (errors.Count == 0, errors);
    }

    private async Task<(bool isValid, List<string> errors, CostCenter? entity)> ValidateDuplicateNamesForUpdate(
        CostCenterUpdateCommand command)
    {
        var oldEntity = await repository.Get(command.Id);
        var existedEntity = await repository.GetByNames(command.Name, command.NameSecondLanguage);
        if (existedEntity is null || existedEntity.Id == command.Id)
            return (true, [], oldEntity);

        var errors = CollectDuplicateNameErrors(existedEntity, command.Name, command.NameSecondLanguage);
        return (errors.Count == 0, errors, oldEntity);
    }

    private static List<string> CollectDuplicateNameErrors(
        CostCenter existedEntity,
        string? name,
        string? nameSecondLanguage)
    {
        var errors = new List<string>();

        if (existedEntity.Name.Trim().ToUpper() == name?.Trim().ToUpper())
            errors.Add(ErrorMessages.WithSameNameIsExisted);

        if (existedEntity.NameSecondLanguage.Trim().ToUpper() == nameSecondLanguage?.Trim().ToUpper())
            errors.Add(ErrorMessages.WithSameNameSecondLanguageIsExisted);

        return errors;
    }
}
