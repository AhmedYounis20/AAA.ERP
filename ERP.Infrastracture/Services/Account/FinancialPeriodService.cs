using ERP.Application.Repositories.Account;
using ERP.Application.Services.Account;
using ERP.Domain.Commands.Account.FinancialPeriods;
using ERP.Domain.Models.Dtos.FinancialPeriods;
using ERP.Domain.Models.Entities.Account.FinancialPeriods;

namespace ERP.Infrastracture.Services.Account;

public class FinancialPeriodService :
    BaseService<FinancialPeriod, FinancialPeriodCreateCommand, FinancialPeriodUpdateCommand>, IFinancialPeriodService
{
    IFinancialPeriodRepository _repository;
    IUnitOfWork _unitofWork;
    TimeSpan tick = new TimeSpan(0, 0, 0, 0, 1);

    public FinancialPeriodService(IFinancialPeriodRepository repository, IUnitOfWork unitofWork) : base(repository)
    {
        _repository = repository;
        _unitofWork = unitofWork;
    }

    public override async Task<ApiResponse<FinancialPeriod>> Create(FinancialPeriodCreateCommand command,
        bool isValidate = true)
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
                    ErrorMessages = validationResult.errors
                };
            }

            FinancialPeriod? lastFinancialPeriod = await _repository.GetLastFinancialPeriod();
            FinancialPeriod entity = command.Adapt<FinancialPeriod>();
            if (lastFinancialPeriod != null)
                entity.StartDate = lastFinancialPeriod.EndDate.AddTicks(1);


            entity.EndDate = entity.StartDate.AddMonths(entity.PeriodTypeByMonth).AddTicks(-1);


            entity = await _repository.Add(entity);

            return new ApiResponse<FinancialPeriod>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entity,
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<FinancialPeriod>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
            };
        }
    }

    public async Task<ApiResponse<FinancialPeriod>> GetCurrentFinancailPeriod()
    {
        FinancialPeriod? currentFinancialPeriod = await _repository.GetCurrentFinancialPeroid();
        if (currentFinancialPeriod == null)
        {
            return new ApiResponse<FinancialPeriod>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                ErrorMessages = new List<string> { "NotFoundCurrentFinancialPeriod" }
            };
        }
        else
        {
            return new ApiResponse<FinancialPeriod>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = currentFinancialPeriod
            };
        }
    }

    public override async Task<ApiResponse<FinancialPeriod>> Update(FinancialPeriodUpdateCommand command,
        bool isValidate = false)
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
                    ErrorMessages = validationResult.errors
                };
            }

            FinancialPeriod? entity = validationResult.entity;
            if (entity != null)
            {
                entity.YearNumber = command.YearNumber;
                if (command.PeriodTypeByMonth != entity.PeriodTypeByMonth)
                {
                    entity.PeriodTypeByMonth = command.PeriodTypeByMonth;
                    entity.EndDate = entity.StartDate.AddMonths(entity.PeriodTypeByMonth).AddTicks(-1);
                }
                await _repository.Update(entity);
            }

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
            };
        }
    }

    protected override async Task<(bool isValid, List<string> errors)> ValidateCreate(
        FinancialPeriodCreateCommand command)
    {
        var result = await base.ValidateCreate(command);
        bool isExisted = await _repository.IsExisted(command.YearNumber);
        if (isExisted)
        {
            result.isValid = false;
            result.errors.Add("FinancialPeriodWithYearNumberIsExisted");
        }

        return result;
    }

    protected override async Task<(bool isValid, List<string> errors, FinancialPeriod? entity)> ValidateUpdate(FinancialPeriodUpdateCommand command)
    {
        var result = await base.ValidateUpdate(command);

        if (!result.isValid || result.entity == null)
            return result;

        if (command.YearNumber != result.entity.YearNumber)
        {
            var isUsedInEntries = await _repository.IsUsedInEntries(command.Id);
            if (isUsedInEntries)
            {
                result.isValid = false;
                result.errors.Add("FinancialTransactionIsUsedInEntriesCannotUpdateYearName");
                return result;
            }

            var IsUsedInInventoryTransactions = await _repository.IsUsedInInventoryTransactions(command.Id);
            if (isUsedInEntries)
            {
                result.isValid = false;
                result.errors.Add("FinancialTransactionIsUsedInInventoryTransactionsCannotUpdateYearName");
                return result;
            }
        }
        if (command.PeriodTypeByMonth < result.entity.PeriodTypeByMonth)
        {
            var newEndDate = result.entity.StartDate.AddMonths(command.PeriodTypeByMonth).AddTicks(-1);
            var isLast = await _repository.IsLastFinancialPeriod(command.Id);
            if (!isLast)
            {
                result.isValid = false;
                result.errors.Add("NotLastPeriod");
            }

            var isEntryAfterDateExist = await _unitofWork.EntryRepository.EntryAfterThisDateExist(newEndDate);
            if (isEntryAfterDateExist)
            {
                result.isValid = false;
                result.errors.Add("ThereAreEntriesAfterNewEndDate");
                return result;
            }

            var isInventoryTransactionAfterDateExist = await _unitofWork.InventoryTransactionRepository.TransactionAfterThisDateExist(newEndDate);
            if (isInventoryTransactionAfterDateExist)
            {
                result.isValid = false;
                result.errors.Add("ThereAreInventoryTrasnsactionsAfterNewEndDate");
                return result;
            }
        }

        return result;
    }

    public async Task<ApiResponse<List<FinancialPeriodDto>>> GetDtos()
    {
        try
        {
            var entities = await _repository.GetDtos();
            return new ApiResponse<List<FinancialPeriodDto>>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<FinancialPeriodDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }


    public async Task<ApiResponse<FinancialPeriodDto?>> GetDto(Guid id)
    {
        try
        {
            var entities = await _repository.GetDto(id);
            return new ApiResponse<FinancialPeriodDto?>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<FinancialPeriodDto?>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }

    public async Task<ApiResponse<FinancialPeriodDto>> GetNextDefaultdata()
    {
        try
        {
            var entities = await _repository.GetNextDefaultdata();
            return new ApiResponse<FinancialPeriodDto>
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = entities
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<FinancialPeriodDto>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = new List<string> { ex.Message }
            };
        }
    }
    protected override async Task<(bool isValid, List<string> errors, FinancialPeriod? entity)> ValidateDelete(Guid id)
    {
        var result = await base.ValidateDelete(id);
        if (!result.isValid)
            return result;

        var isLast = await _repository.IsLastFinancialPeriod(id);
        if (!isLast)
        {
            result.isValid = false;
            result.errors.Add("NotLastPeriod");
        }

        var isEntryAfterDateExist = await _repository.IsUsedInEntries(id);
        if (isEntryAfterDateExist)
        {
            result.isValid = false;
            result.errors.Add("FinancialPeriodIsUsedInEntries");
            return result;
        }   

        var isInventoryTransactionAfterDateExist = await _repository.IsUsedInInventoryTransactions(id);
        if (isInventoryTransactionAfterDateExist)
        {
            result.isValid = false;
            result.errors.Add("FinancialPeriodIsUsedInInventoryTransactions");
            return result;
        }

        return result;
    }
    
}