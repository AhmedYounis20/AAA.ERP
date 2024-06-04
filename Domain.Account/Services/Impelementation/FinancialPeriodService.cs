using AAA.ERP.Models.Entities.Currencies;
using AAA.ERP.Services.BaseServices.impelemtation;
using AAA.ERP.Repositories.Interfaces;
using AAA.ERP.Services.Interfaces;
using AAA.ERP.Validators.BussinessValidator.Interfaces;
using AAA.ERP.Responses;
using AAA.ERP.Models.Entities.FinancialPeriods;

namespace AAA.ERP.Services.Impelementation;

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

    public override async Task<ApiResponse> Create(FinancialPeriod entity, bool isValidate = true)
    {
        var validationResult = await _bussinessValidator.ValidateCreateBussiness(entity);
        if (!validationResult.IsValid)
        {
            return new ApiResponse
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

    public async Task<ApiResponse> GetCurrentFinancailPeriod()
    {
        FinancialPeriod? currentFinancialPeriod = await _repository.GetCurrentFinancialPeroid();
        if(currentFinancialPeriod == null)
        {
            return new ApiResponse
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.NotFound,
                ErrorMessages = new List<string> { "NotFoundCurrentFinancialPeriod" }
            };
        }
        else
        {
            return new ApiResponse
            {
                IsSuccess = true,
                StatusCode = HttpStatusCode.OK,
                Result = currentFinancialPeriod
            };
        }
    }

    public override async Task<ApiResponse> Update(FinancialPeriod entity, bool isValidate = false)
    {
        var validationResult = await _bussinessValidator.ValidateUpdateBussiness(entity);
        if (!validationResult.IsValid)
        {
            return new ApiResponse
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