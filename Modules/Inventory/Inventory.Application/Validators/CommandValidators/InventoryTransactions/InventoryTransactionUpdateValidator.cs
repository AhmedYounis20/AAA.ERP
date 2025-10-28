using FluentValidation;
using Inventory.Domain.Commands.InventoryTransactions;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using Inventory.Domain.Models.DTOS;
using Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;

namespace Inventory.Application.Validators.CommandValidators.InventoryTransactions;

public class InventoryTransactionUpdateValidator : BaseUpdateValidator<InventoryTransactionUpdateCommand, InventoryTransaction>
{
    public InventoryTransactionUpdateValidator()
    {
        _ = RuleFor(e => e.TransactionType).IsInEnum().WithMessage("InvalidTransactionType");
        _ = RuleFor(e => e.TransactionDate).NotEmpty().WithMessage("TransactionDateIsRequired");
        _ = RuleFor(e => e.TransactionPartyId).NotEmpty().WithMessage("TransactionPartyIdIsRequired");
        _ = RuleFor(e => e.BranchId).NotEmpty().WithMessage("BranchIdIsRequired");
        _ = RuleFor(e => e.DocumentNumber).MaximumLength(100).WithMessage("DocumentNumberMaxLength");
        _ = RuleFor(e => e.Notes).MaximumLength(1000).WithMessage("NotesMaxLength");
        _ = RuleFor(e => e.Items).NotEmpty().WithMessage("ItemsRequired");
        _ = RuleForEach(e => e.Items).SetValidator(new InventoryTransactionItemUpdateValidator());
    }
}

public class InventoryTransactionItemUpdateValidator : AbstractValidator<InventoryTransactionItemUpdateDto>
{
    public InventoryTransactionItemUpdateValidator()
    {
        _ = RuleFor(e => e.Id).NotEmpty().WithMessage("IdIsRequired");
        _ = RuleFor(e => e.ItemId).NotEmpty().WithMessage("ItemIdIsRequired");
        _ = RuleFor(e => e.PackingUnitId).NotEmpty().WithMessage("PackingUnitIdIsRequired");
        _ = RuleFor(e => e.Quantity).GreaterThan(0).WithMessage("QuantityMustBeGreaterThanZero");
        _ = RuleFor(e => e.TotalCost).GreaterThanOrEqualTo(0).WithMessage("TotalCostMustBeGreaterThanOrEqualToZero");
    }
} 