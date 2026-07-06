using Accounting.Application.Commands.Taxes;
using Accounting.Application.Validators.Account.CommandValidators.SubLeadgers.BaseSubLeadgers;
using Accounting.Domain.Models.Entities.Account.SubLeadgers;
using FluentValidation;
using ERP.BuildingBlocks.Domain.Entities;
using ERP.BuildingBlocks.Application.Constants;

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
            .WithMessage(ValidationMessages.ChartOfAccountIdIsRequired)
            .When(e => e.NodeType.Equals(NodeType.Domain));
    }
}
