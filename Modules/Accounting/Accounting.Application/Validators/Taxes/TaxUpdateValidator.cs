using Accounting.Application.Commands.Taxes;
using Accounting.Domain.Entities;
using ERP.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;
using FluentValidation;
using Shared.BaseEntities;

namespace Accounting.Application.Validators.Taxes;

public class TaxUpdateValidator : BaseSubLeadgerUpdateValidator<TaxUpdateCommand, Tax>
{
    public TaxUpdateValidator() : base()
    {
        _ = RuleFor(e => e.Percent)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100)
            .When(e => e.NodeType.Equals(NodeType.Domain));
    }
}
