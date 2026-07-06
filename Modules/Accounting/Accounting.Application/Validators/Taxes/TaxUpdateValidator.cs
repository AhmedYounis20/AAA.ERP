using Accounting.Application.Commands.Taxes;
using Accounting.Application.Validators.Account.CommandValidators.SubLeadgers.BaseSubLeadgers;
using Accounting.Domain.Models.Entities.Account.SubLeadgers;
using FluentValidation;
using ERP.BuildingBlocks.Domain.Entities;

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
