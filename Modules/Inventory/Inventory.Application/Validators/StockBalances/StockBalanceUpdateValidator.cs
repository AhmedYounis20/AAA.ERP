using ERP.BuildingBlocks.Application.Validators.Account.CommandValidators.BaseCommandValidators.UpdateCommandValidators;
using Inventory.Application.Commands.StockBalances;
using Inventory.Domain.Models.Entities.StockBalances;
using FluentValidation;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Application.Validators.StockBalances;

public class StockBalanceUpdateValidator : BaseUpdateValidator<StockBalanceUpdateCommand, StockBalance>
{
    public StockBalanceUpdateValidator()
    {
        _ = RuleFor(e => e.VariantId).NotEmpty().WithMessage(ValidationMessages.ItemIdIsRequired);
        _ = RuleFor(e => e.PackingUnitId).NotEmpty().WithMessage(ValidationMessages.PackingUnitIdIsRequired);
        _ = RuleFor(e => e.BranchId).NotEmpty().WithMessage(ValidationMessages.BranchIdIsRequired);
        _ = RuleFor(e => e.CurrentBalance).GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.CurrentBalanceMustBeGreaterThanOrEqualToZero);
        _ = RuleFor(e => e.MinimumBalance).GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.MinimumBalanceMustBeGreaterThanOrEqualToZero);
        _ = RuleFor(e => e.MaximumBalance).GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.MaximumBalanceMustBeGreaterThanOrEqualToZero);
        _ = RuleFor(e => e.UnitCost).GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.UnitCostMustBeGreaterThanOrEqualToZero);
        _ = RuleFor(e => e.TotalCost).GreaterThanOrEqualTo(0).WithMessage(ValidationMessages.TotalCostMustBeGreaterThanOrEqualToZero2);
    }
}
