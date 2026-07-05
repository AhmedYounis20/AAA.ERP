using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using Inventory.Application.Commands.StockBalances;
using ERP.Domain.Models.Entities.Inventory.Sizes;
using FluentValidation;

namespace Inventory.Application.Validators.StockBalances;

public class StockBalanceUpdateValidator : BaseUpdateValidator<StockBalanceUpdateCommand, StockBalance>
{
    public StockBalanceUpdateValidator()
    {
        _ = RuleFor(e => e.VariantId).NotEmpty().WithMessage("ItemIdIsRequired");
        _ = RuleFor(e => e.PackingUnitId).NotEmpty().WithMessage("PackingUnitIdIsRequired");
        _ = RuleFor(e => e.BranchId).NotEmpty().WithMessage("BranchIdIsRequired");
        _ = RuleFor(e => e.CurrentBalance).GreaterThanOrEqualTo(0).WithMessage("CurrentBalanceMustBeGreaterThanOrEqualToZero");
        _ = RuleFor(e => e.MinimumBalance).GreaterThanOrEqualTo(0).WithMessage("MinimumBalanceMustBeGreaterThanOrEqualToZero");
        _ = RuleFor(e => e.MaximumBalance).GreaterThanOrEqualTo(0).WithMessage("MaximumBalanceMustBeGreaterThanOrEqualToZero");
        _ = RuleFor(e => e.UnitCost).GreaterThanOrEqualTo(0).WithMessage("UnitCostMustBeGreaterThanOrEqualToZero");
        _ = RuleFor(e => e.TotalCost).GreaterThanOrEqualTo(0).WithMessage("TotalCostMustBeGreaterThanOrEqualToZero");
    }
}
