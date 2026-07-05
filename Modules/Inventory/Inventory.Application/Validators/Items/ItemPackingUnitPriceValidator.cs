using ERP.Domain.Models.Entities.Inventory.Items;
using FluentValidation;

namespace Inventory.Application.Validators.Items;

public class ItemPackingUnitPriceValidator : AbstractValidator<ItemPackingUnitSellingPriceDto>
{ 
    public ItemPackingUnitPriceValidator() {

        _ = RuleFor(e => e.SellingPriceId).NotEmpty().WithMessage("SellingPriceIsRequired");
        _ = RuleFor(e => e.Amount).GreaterThan(0).WithMessage("PriceAmountIsRequired");
    }
}