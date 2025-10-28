using FluentValidation;
using Inventory.Domain.Models.DTOS;

namespace Inventory.Application.Validators.CommandValidators.Items;

public class ItemPackingUnitPriceValidator : AbstractValidator<ItemPackingUnitSellingPriceDto>
{ 
    public ItemPackingUnitPriceValidator() {

        _ = RuleFor(e => e.SellingPriceId).NotEmpty().WithMessage("SellingPriceIsRequired");
        _ = RuleFor(e => e.Amount).GreaterThan(0).WithMessage("PriceAmountIsRequired");
    }
}