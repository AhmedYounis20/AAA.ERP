using Accounting.Application.Commands.Taxes;
using Accounting.Domain.Entities;
using ERP.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;
using FluentValidation;
using Shared.BaseEntities;

namespace Accounting.Application.Validators.Taxes;

public class TaxCreateValidator : BaseSubLeadgerCreateValidator<TaxCreateCommand, Tax>
{
    public TaxCreateValidator() : base()
    {
        _ = RuleFor(e => e.Percent)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(100)
            .When(e => e.NodeType.Equals(NodeType.Domain));
        _ = RuleFor(e => e.ChartOfAccountId)
            .NotEmpty()
            .WithMessage("ChartOfAccountIdIsRequired")
            .When(e => e.NodeType.Equals(NodeType.Domain));
    }
}
