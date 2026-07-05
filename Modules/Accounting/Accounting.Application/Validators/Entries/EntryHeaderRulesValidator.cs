using Accounting.Application.Commands.Common;
using ERP.BuildingBlocks.Application.Constants;
using FluentValidation;

namespace Accounting.Application.Validators.Entries;

/// <summary>Shared FluentValidation rules for entry command headers (branch, currency, period, notes).</summary>
public sealed class EntryHeaderRulesValidator : AbstractValidator<IEntryHeaderCommand>
{
    public EntryHeaderRulesValidator()
    {
        RuleFor(x => x.BranchId).NotEmpty().WithMessage(ValidationMessages.BranchIdIsRequired);
        RuleFor(x => x.CurrencyId).NotEmpty().WithMessage(ValidationMessages.CurrencyIdIsRequired);
        RuleFor(x => x.FinancialPeriodId).NotEmpty().WithMessage(ValidationMessages.FinancialPeriodIdIsRequired);
        RuleFor(x => x.Notes).MaximumLength(500).WithMessage(ValidationMessages.NotesMaximumLength);
        RuleFor(x => x.ReceiverName).MaximumLength(100).WithMessage(ValidationMessages.ReceiverNameMaximumLength);
        RuleFor(x => x.DocumentNumber).MaximumLength(100).WithMessage(ValidationMessages.DocumentNumberMaximumLength);
    }
}
