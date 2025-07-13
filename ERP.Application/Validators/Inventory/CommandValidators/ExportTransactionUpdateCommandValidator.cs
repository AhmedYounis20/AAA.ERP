using ERP.Domain.Commands.Inventory.InventoryTransactions;
using FluentValidation;

namespace ERP.Application.Validators.Inventory.CommandValidators;

public class ExportTransactionUpdateCommandValidator : AbstractValidator<ExportTransactionUpdateCommand>
{
    public ExportTransactionUpdateCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Transaction ID is required");

        RuleFor(x => x.TransactionDate)
            .NotEmpty()
            .WithMessage("Transaction date is required");

        RuleFor(x => x.TransactionPartyId)
            .NotEmpty()
            .WithMessage("Transaction party is required");

        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("Branch is required");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("At least one item is required");

        RuleForEach(x => x.Items).SetValidator(new InventoryTransactionItemCreateDtoValidator());
    }
} 