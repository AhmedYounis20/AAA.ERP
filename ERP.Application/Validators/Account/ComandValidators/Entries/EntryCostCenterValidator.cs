using ERP.Domain.Models.Entities.Account.Entries;
using FluentValidation;

namespace ERP.Application.Validators.Account.ComandValidators.Entries;

public class EntryCostCenterValidator : AbstractValidator<EntryCostCenter>
{
    public EntryCostCenterValidator() : base()
    {
        _ = RuleFor(e => e.CostCenterId).NotEmpty().When(e => e.Amount != 0).WithMessage("AmountIsRequired");
        _ = RuleFor(e => e.Amount).GreaterThan(0).When(e => e.CostCenterId != null).WithMessage("CostCenterIsRequired");
    }
}