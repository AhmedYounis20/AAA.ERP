using ERP.Domain.Commands.Inventory.InventoryTransactions;
using FluentValidation;

namespace ERP.Application.Validators.Inventory.CommandValidators;

public class ImportTransactionCreateCommandValidator : AbstractValidator<ImportTransactionCreateCommand>
{
    public ImportTransactionCreateCommandValidator()
    {
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

public class InventoryTransactionItemCreateDtoValidator : AbstractValidator<InventoryTransactionItemCreateDto>
{
    public InventoryTransactionItemCreateDtoValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Item is required");

        RuleFor(x => x.PackingUnitId)
            .NotEmpty()
            .WithMessage("Packing unit is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero");

        RuleFor(x => x.TotalCost)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total cost must be greater than or equal to zero");
    }
} 