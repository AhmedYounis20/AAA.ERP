using Inventory.Application.Commands.InventoryTransactions;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Application.Validators.InventoryTransactions;

public class ImportTransactionCreateValidator : AbstractValidator<ImportTransactionCreateCommand>
{
    public ImportTransactionCreateValidator()
    {
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
            .WithMessage(ValidationMessages.AtLeastOneLineItemIsRequired);

        RuleForEach(x => x.Items).SetValidator(new InventoryTransactionItemCreateDtoValidator());
    }
}

public class InventoryTransactionItemCreateDtoValidator : AbstractValidator<InventoryTransactionItemCreateDto>
{
    public InventoryTransactionItemCreateDtoValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty()
            .WithMessage(ValidationMessages.VariantIdIsRequired);

        RuleFor(x => x.PackingUnitId)
            .NotEmpty()
            .WithMessage(ValidationMessages.PackingUnitIsRequired);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.QuantityMustBeGreaterThanZero);

        RuleFor(x => x.TotalCost)
            .GreaterThanOrEqualTo(0)
            .WithMessage(ValidationMessages.TotalCostMustBeGreaterThanOrEqualToZero);
    }
}
