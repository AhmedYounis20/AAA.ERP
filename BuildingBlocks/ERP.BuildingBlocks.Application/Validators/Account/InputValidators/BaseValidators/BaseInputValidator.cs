using FluentValidation;
using ERP.BuildingBlocks.Application.Commands.BaseInputModels;

namespace ERP.BuildingBlocks.Application.Validators.Account.InputValidators.BaseValidators;

public class BaseInputValidator<TEntity> : AbstractValidator<TEntity> where TEntity : BaseInputModel
{
    public BaseInputValidator()
    {
        _ = RuleFor(e => e.Notes).MaximumLength(300);
    }
}
