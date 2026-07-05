using Accounting.Domain.Models.Entities.Entries;
using FluentValidation;
using Shared.Constants;

namespace Accounting.Application.Validators.Entries;

public class EntryCostCenterValidator : AbstractValidator<EntryCostCenter>
{
    public EntryCostCenterValidator() : base()
    {
        _ = RuleFor(e => e.CostCenterId).NotEmpty().When(e => e.Amount != 0).WithMessage(ErrorMessages.AmountIsRequired);
        _ = RuleFor(e => e.Amount).GreaterThan(0).When(e => e.CostCenterId != null).WithMessage(ErrorMessages.CostCenterIsRequired);
    }
}