using AAA.ERP.Validators.BussinessValidator.Interfaces;
using Domain.Account.Models.Entities.FinancialPeriods;
using Domain.Account.Repositories.Interfaces;
using Domain.Account.Services.BaseServices.impelemtation;
using Domain.Account.Services.Interfaces;
using Shared.Responses;

namespace Domain.Account.Services.Impelementation;

public class FinancialPeriodService : BaseService<FinancialPeriod>, IFinancialPeriodService
{
    IFinancialPeriodRepository _repository;
    IFinancialPeriodBussinessValidator _bussinessValidator;
    TimeSpan tick = new TimeSpan(0, 0, 0, 0, 1);
    public FinancialPeriodService(IFinancialPeriodRepository repository,
                           IFinancialPeriodBussinessValidator bussinessValidator) : base(repository, bussinessValidator)
    {
        _repository = repository;
        _bussinessValidator = bussinessValidator;
    }

    public override async Task<ApiResponse<FinancialPeriod>> Create(FinancialPeriod entity, bool isValidate = true)
    {
        var validationResult = await _bussinessValidator.ValidateCreateBussiness(entity);
        if (!validationResult.IsValid)
        {
            return new ApiResponse<FinancialPeriod>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = validationResult.ListOfErrors
            };
        }

        FinancialPeriod? lastFinancialPeriod = await _repository.GetLastFinancialPeriod();

        if (lastFinancialPeriod != null)
            entity.StartDate = lastFinancialPeriod.EndDate.AddTicks(1);

        entity.EndDate = entity.StartDate.AddMonths(entity.PeriodTypeByMonth).AddTicks(-1);

        return await base.Create(entity, false);
    }

    public async Task<ApiResponse<FinancialPeriod>> GetCurrentFinancailPeriod()
    {
        FinancialPeriod? currentFinancialPeriod = await _repository.GetCurrentFinancialPeroid();
        if(currentFinancialPeriod == null)
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

    public override async Task<ApiResponse<FinancialPeriod>> Update(FinancialPeriod entity, bool isValidate = false)
    {
        var validationResult = await _bussinessValidator.ValidateUpdateBussiness(entity);
        if (!validationResult.IsValid)
        {
            return new ApiResponse<FinancialPeriod>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessages = validationResult.ListOfErrors
            };
        }
        if (validationResult.entity != null)
            validationResult.entity.YearNumber = entity.YearNumber;

        return await base.Update(validationResult.entity ?? new(), false);
    }
}