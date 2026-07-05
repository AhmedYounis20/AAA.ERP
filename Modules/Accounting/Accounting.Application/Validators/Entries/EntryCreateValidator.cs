using Accounting.Application.Validators.Entries;
using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Accounting.Application.Commands.Entries;
using Accounting.Domain.Models.Entities.Entries;
using FluentValidation;
using Shared.Constants;

namespace Accounting.Application.Validators.Entries;

public class EntryCreateValidator : BaseCreateValidator<ComplexEntryCreateCommand, Entry>
{
    public EntryCreateValidator() : base()
    {

        _ = RuleFor(e => e.BranchId).NotEmpty().WithMessage(ErrorMessages.BranchIdIsRequired);
        _ = RuleFor(e => e.CurrencyId).NotEmpty().WithMessage(ErrorMessages.CurrencyIdIsRequired);
        _ = RuleFor(e => e.FinancialPeriodId).NotEmpty().WithMessage(ErrorMessages.FinancialPeriodIdIsRequired);
        _ = RuleFor(e => e.Notes).MaximumLength(500).WithMessage(ErrorMessages.NotesMaximumLength);
        _ = RuleFor(e => e.ReceiverName).MaximumLength(100).WithMessage(ErrorMessages.ReceiverNameMaximumLength);
        _ = RuleFor(e => e.DocumentNumber).MaximumLength(100).WithMessage(ErrorMessages.DocumentNumberMaximumLength);
        _ = RuleFor(e => e.FinancialTransactions).NotEmpty().WithMessage(ErrorMessages.EntryFinancialTransactionsRequired);
        _ = RuleForEach(e => e.CostCenters).SetValidator(new EntryCostCenterValidator()).When(e => e.CostCenters != null && e.CostCenters.Any());
    }
}