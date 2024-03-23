using AAA.ERP.Models.Entities.FinancialPeriods;
using AAA.ERP.Repositories.Interfaces;
using AAA.ERP.Responses;
using AAA.ERP.Validators.BussinessValidator.BaseBussinessValidators.Impelementation;
using AAA.ERP.Validators.BussinessValidator.Interfaces;

namespace AAA.ERP.Validators.BussinessValidator.Impelementation;

public class FinancialPeriodBussinessValidator : BaseBussinessValidator<FinancialPeriod>, IFinancialPeriodBussinessValidator
{
    private IFinancialPeriodRepository _repository;
    public FinancialPeriodBussinessValidator(IFinancialPeriodRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public override async Task<(bool IsValid, List<string> ListOfErrors, FinancialPeriod? entity)> ValidateCreateBussiness(FinancialPeriod inputModel)
    {
        bool isExisted = await _repository.IsExisted(inputModel.YearNumber);
        if (isExisted)
        {
            return (false, new List<string> { "FinancialPeriodWithYearNumberIsExisted" }, null);
        }

        return await base.ValidateCreateBussiness(inputModel);
    }

    public override async Task<(bool IsValid, List<string> ListOfErrors, FinancialPeriod? entity)> ValidateUpdateBussiness(FinancialPeriod inputModel)
    {
        var validationResult = await base.ValidateUpdateBussiness(inputModel);
        if (!validationResult.IsValid)
            return validationResult;
        
        if (validationResult.entity?.StartDate < DateTime.Now)
        {
            return (false,
                    new List<string> { "FinancialPeriodCurrentOrPastUpdateError" },
                    validationResult.entity
            );
        }

        return validationResult;
    }
}