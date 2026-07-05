using Accounting.Application.Commands.Taxes;
using Accounting.Domain.Entities;
using Accounting.Application.Validators.Account.ComandValidators.SubLeadgers.BaseSubLeadgers;
using FluentValidation;
using Shared.BaseEntities;
using Shared.Constants;

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
            .WithMessage(ErrorMessages.ChartOfAccountIdIsRequired)
            .When(e => e.NodeType.Equals(NodeType.Domain));
    }
}
