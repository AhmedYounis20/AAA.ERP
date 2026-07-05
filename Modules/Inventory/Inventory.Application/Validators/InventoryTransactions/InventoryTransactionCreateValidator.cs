using ERP.BuildingBlocks.Application.Validators.Account.ComandValidators.BaseCommandValidators.CreateCommandValidators;
using Inventory.Application.Commands.InventoryTransactions;
using Inventory.Domain.Models.Entities.InventoryTransactions;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Application.Validators.InventoryTransactions;

public class InventoryTransactionCreateValidator : BaseCreateValidator<InventoryTransactionCreateCommand, InventoryTransaction>
{
    public InventoryTransactionCreateValidator()
    {
        _ = RuleFor(e => e.TransactionType).IsInEnum().WithMessage(ValidationMessages.InvalidTransactionType);
        _ = RuleFor(e => e.TransactionDate).NotEmpty().WithMessage(ValidationMessages.TransactionDateIsRequired2);
        _ = RuleFor(e => e.TransactionPartyId).NotEmpty().WithMessage(ValidationMessages.TransactionPartyIdIsRequired);
        _ = RuleFor(e => e.BranchId).NotEmpty().WithMessage(ValidationMessages.BranchIdIsRequired);
        _ = RuleFor(e => e.DocumentNumber).MaximumLength(100).WithMessage(ValidationMessages.DocumentNumberMaxLength);
        _ = RuleFor(e => e.Notes).MaximumLength(1000).WithMessage(ValidationMessages.NotesMaxLength);
        _ = RuleFor(e => e.Items).NotEmpty().WithMessage(ValidationMessages.ItemsRequired);
        _ = RuleForEach(e => e.Items).SetValidator(new InventoryTransactionItemCreateValidator());
    }
}

public class InventoryTransactionItemCreateValidator : AbstractValidator<InventoryTransactionItemCreateDto>
{
    public InventoryTransactionItemCreateValidator()
    {
        _ = RuleFor(e => e.VariantId).NotEmpty().WithMessage(ValidationMessages.VariantIdIsRequired);
        _ = RuleFor(e => e.PackingUnitId).NotEmpty().WithMessage(ValidationMessages.PackingUnitIdIsRequired);
        _ = RuleFor(e => e.Quantity).GreaterThan(0).WithMessage(ValidationMessages.QuantityMustBeGreaterThanZero2);
        _ = RuleFor(e => e.TotalCost).GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.TotalCostMustBeGreaterThanOrEqualToZero2);
    }
}
