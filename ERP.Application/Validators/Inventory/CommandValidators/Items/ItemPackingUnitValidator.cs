using ERP.Domain.Models.Entities.Inventory.Items;
using FluentValidation;

namespace ERP.Application.Validators.Inventory.CommandValidators.Items;

public class ItemPackingUnitValidator : AbstractValidator<ItemPackingUnitDto>
{ 
    public ItemPackingUnitValidator() {

        _ = RuleFor(e => e.PackingUnitId).NotEmpty().WithMessage("PackingUnitIsRequired");
        _ = RuleFor(e => e.AverageCostPrice).GreaterThan(0).WithMessage("AverageCostIsRequired");
        _ = RuleFor(e => e.LastCostPrice).GreaterThan(0).WithMessage("LastCostPriceIsRequired");
        _ = RuleFor(e => e.PartsCount).GreaterThan(0).WithMessage("PartsCountIsRequired");
        _ = RuleFor(e => e.SellingPrices).NotEmpty().WithMessage("PackingUnitSellingPriceIsRequired");
        _ = RuleForEach(e => e.SellingPrices).SetValidator(new ItemPackingUnitPriceValidator()).WithMessage("PartsCountIsRequired");
    }
}