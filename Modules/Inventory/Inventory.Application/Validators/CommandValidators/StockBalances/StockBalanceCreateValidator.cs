using FluentValidation;
using Inventory.Domain.Commands.StockBalances;
using Inventory.Domain.Models.Entities.StockBalances;
using Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;

namespace Inventory.Application.Validators.CommandValidators.StockBalances;

public class StockBalanceCreateValidator : BaseCreateValidator<StockBalanceCreateCommand, StockBalance>
{
    public StockBalanceCreateValidator()
    {
        _ = RuleFor(e => e.ItemId).NotEmpty().WithMessage("ItemIdIsRequired");
        _ = RuleFor(e => e.PackingUnitId).NotEmpty().WithMessage("PackingUnitIdIsRequired");
        _ = RuleFor(e => e.BranchId).NotEmpty().WithMessage("BranchIdIsRequired");
        _ = RuleFor(e => e.CurrentBalance).GreaterThanOrEqualTo(0).WithMessage("CurrentBalanceMustBeGreaterThanOrEqualToZero");
        _ = RuleFor(e => e.MinimumBalance).GreaterThanOrEqualTo(0).WithMessage("MinimumBalanceMustBeGreaterThanOrEqualToZero");
        _ = RuleFor(e => e.MaximumBalance).GreaterThanOrEqualTo(0).WithMessage("MaximumBalanceMustBeGreaterThanOrEqualToZero");
        _ = RuleFor(e => e.UnitCost).GreaterThanOrEqualTo(0).WithMessage("UnitCostMustBeGreaterThanOrEqualToZero");
        _ = RuleFor(e => e.TotalCost).GreaterThanOrEqualTo(0).WithMessage("TotalCostMustBeGreaterThanOrEqualToZero");
    }
} 