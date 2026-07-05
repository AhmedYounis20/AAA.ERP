using AAA.ERP.OutputDtos.BaseDtos;
using FluentValidation;
using System.Data;
using Domain.Account.InputModels.BaseInputModels;

namespace ERP.BuildingBlocks.Application.Validators.Account.InputValidators.BaseValidators;

public class BaseInputValidator<TEntity> : AbstractValidator<TEntity> where TEntity : BaseInputModel
{
    public BaseInputValidator()
    {
        _ = RuleFor(e => e.Notes).MaximumLength(300);
    }
}
