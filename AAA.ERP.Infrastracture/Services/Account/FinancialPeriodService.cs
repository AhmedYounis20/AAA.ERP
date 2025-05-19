using Domain.Account.Commands.FinancialPeriods;
using Domain.Account.Models.Entities.FinancialPeriods;
using ERP.Application.Services.Account;

namespace ERP.Infrastracture.Services.Account;

public class FinancialPeriodService :
    BaseService<FinancialPeriod, FinancialPeriodCreateCommand, FinancialPeriodUpdateCommand>, IFinancialPeriodService
{
    IFinancialPeriodRepository _repository;
    TimeSpan tick = new TimeSpan(0, 0, 0, 0, 1);

    public FinancialPeriodService(IFinancialPeriodRepository repository) : base(repository)
    {
        _repository = repository;
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

        if (result.entity?.StartDate < DateTime.Now)
        {
            result.isValid = false;
            result.errors.Add("FinancialPeriodCurrentOrPastUpdateError");
        }

        return result;
    }
}