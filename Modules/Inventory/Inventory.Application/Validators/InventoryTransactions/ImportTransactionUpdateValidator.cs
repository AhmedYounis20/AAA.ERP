using Inventory.Application.Commands.InventoryTransactions;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Application.Validators.InventoryTransactions;

public class ImportTransactionUpdateValidator : AbstractValidator<ImportTransactionUpdateCommand>
{
    public ImportTransactionUpdateValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(ValidationMessages.TransactionIDIsRequired);

        RuleFor(x => x.TransactionDate)
            .NotEmpty()
            .WithMessage(ValidationMessages.TransactionDateIsRequired);

        RuleFor(x => x.TransactionPartyId)
            .NotEmpty()
            .WithMessage(ValidationMessages.TransactionPartyIsRequired);

        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage(ValidationMessages.BranchIsRequired);

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage(ValidationMessages.AtLeastOneItemIsRequired);

        RuleForEach(x => x.Items).SetValidator(new InventoryTransactionItemCreateDtoValidator());
    }
}
