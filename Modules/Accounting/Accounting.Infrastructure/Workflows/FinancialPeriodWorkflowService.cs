using Accounting.Application.Commands.FinancialPeriods;
using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using ERP.BuildingBlocks.Application.Constants;
using Inventory.Contracts.Integration;

namespace Accounting.Infrastructure.Workflows;

/// <summary>
/// Financial period write orchestration (period sequencing, cross-module usage guards).
/// </summary>
public class FinancialPeriodWorkflowService(
    IFinancialPeriodRepository repository,
    IEntryRepository entryRepository,
    IFinancialPeriodInventoryUsageQueries inventoryUsageQueries) : IFinancialPeriodWorkflowService
{
    public async Task<ApiResponse<FinancialPeriod>> Create(FinancialPeriodCreateCommand command)
    {
        try
        {
            var validationResult = await ValidateCreate(command);
            if (!validationResult.isValid)
            {
                return new ApiResponse<FinancialPeriod>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }

            var lastFinancialPeriod = await repository.GetLastFinancialPeriod();
            var entity = command.Adapt<FinancialPeriod>();
            if (lastFinancialPeriod is not null)
                entity.StartDate = lastFinancialPeriod.EndDate.AddTicks(1);

            entity.EndDate = entity.StartDate.AddMonths(entity.PeriodTypeByMonth).AddTicks(-1);

            entity = await repository.Add(entity);

            return new ApiResponse<FinancialPeriod>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.Created,
                Result = entity,
            };
        }
        catch (Exception)
        {
            return new ApiResponse<FinancialPeriod>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
            };
        }
    }

    public async Task<ApiResponse<FinancialPeriod>> Update(FinancialPeriodUpdateCommand command)
    {
        try
        {
            var validationResult = await ValidateUpdate(command);
            if (!validationResult.isValid)
            {
                return new ApiResponse<FinancialPeriod>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }

            var entity = validationResult.entity;
            if (entity is not null)
            {
                entity.YearNumber = command.YearNumber;
                if (command.PeriodTypeByMonth != entity.PeriodTypeByMonth)
                {
                    entity.PeriodTypeByMonth = command.PeriodTypeByMonth;
                    entity.EndDate = entity.StartDate.AddMonths(entity.PeriodTypeByMonth).AddTicks(-1);
                }

                await repository.Update(entity);
            }

            return new ApiResponse<FinancialPeriod>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception)
        {
            return new ApiResponse<FinancialPeriod>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
            };
        }
    }

    public async Task<ApiResponse<FinancialPeriod>> Delete(Guid id)
    {
        try
        {
            var validationResult = await ValidateDelete(id);
            if (!validationResult.isValid)
            {
                return new ApiResponse<FinancialPeriod>
                {
                    IsSuccess = false,
                    StatusCode = HttpStatusCode.BadRequest,
                    Errors = validationResult.errors?.Select(e => new MessageTemplate { MessageKey = e }).ToList()
                };
            }

            var entity = validationResult.entity;
            if (entity is not null)
                await repository.Delete(entity);

            return new ApiResponse<FinancialPeriod>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<FinancialPeriod>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [new MessageTemplate { MessageKey = ex.Message }]
            };
        }
    }

    private async Task<(bool isValid, List<string> errors)> ValidateCreate(FinancialPeriodCreateCommand command)
    {
        var errors = new List<string>();
        var isValid = true;

        if (await repository.IsExisted(command.YearNumber))
        {
            isValid = false;
            errors.Add(ErrorMessages.FinancialPeriodWithYearNumberIsExisted);
        }

        var lastFinancialPeriod = await repository.GetLastFinancialPeriod();
        if (lastFinancialPeriod is not null && command.StartDate != lastFinancialPeriod.EndDate.AddTicks(1))
        {
            isValid = false;
            errors.Add(ErrorMessages.InvalidFinancialPeriod);
        }

        return (isValid, errors);
    }

    private async Task<(bool isValid, List<string> errors, FinancialPeriod? entity)> ValidateUpdate(
        FinancialPeriodUpdateCommand command)
    {
        var errors = new List<string>();
        var entity = await repository.Get(command.Id);
        if (entity is null)
        {
            return (false, [$"{nameof(FinancialPeriod)} with Id: {command.Id} not found"], null);
        }

        if (command.YearNumber != entity.YearNumber)
        {
            if (await repository.IsExisted(command.YearNumber))
            {
                errors.Add(ErrorMessages.FinancialPeriodWithYearNumberIsExisted);
                return (false, errors, entity);
            }

            if (await repository.IsUsedInEntries(command.Id))
            {
                errors.Add(ErrorMessages.FinancialTransactionIsUsedInEntriesCannotUpdateYearName);
                return (false, errors, entity);
            }

            if (await repository.IsUsedInInventoryTransactions(command.Id))
            {
                errors.Add(ErrorMessages.FinancialTransactionIsUsedInInventoryTransactionsCannotUpdateYearName);
                return (false, errors, entity);
            }
        }

        if (command.PeriodTypeByMonth < entity.PeriodTypeByMonth)
        {
            var newEndDate = entity.StartDate.AddMonths(command.PeriodTypeByMonth).AddTicks(-1);
            if (!await repository.IsLastFinancialPeriod(command.Id))
                errors.Add(ErrorMessages.NotLastPeriod);

            if (await entryRepository.EntryAfterThisDateExist(newEndDate))
            {
                errors.Add(ErrorMessages.ThereAreEntriesAfterNewEndDate);
                return (false, errors, entity);
            }

            if (await inventoryUsageQueries.TransactionExistsAfterDateAsync(newEndDate))
            {
                errors.Add(ErrorMessages.ThereAreInventoryTransactionsAfterNewEndDate);
                return (false, errors, entity);
            }
        }

        return (errors.Count == 0, errors, entity);
    }

    private async Task<(bool isValid, List<string> errors, FinancialPeriod? entity)> ValidateDelete(Guid id)
    {
        var entity = await repository.Get(id);
        if (entity is null)
            return (false, [ErrorMessages.RecordNotFound], null);

        var errors = new List<string>();

        if (!await repository.IsLastFinancialPeriod(id))
            errors.Add(ErrorMessages.NotLastPeriod);

        if (await repository.IsUsedInEntries(id))
        {
            errors.Add(ErrorMessages.FinancialPeriodIsUsedInEntries);
            return (false, errors, entity);
        }

        if (await repository.IsUsedInInventoryTransactions(id))
        {
            errors.Add(ErrorMessages.FinancialPeriodIsUsedInInventoryTransactions);
            return (false, errors, entity);
        }

        return (errors.Count == 0, errors, entity);
    }
}
